using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLinealyticsTriggersAndViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ========== TRIGGER 1: Calcular duración de paros automáticamente ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION linealytics.calcular_duracion_paro()
                RETURNS TRIGGER AS $$
                BEGIN
                    -- Solo calcular si hay fecha de fin
                    IF NEW.""FechaHoraFin"" IS NOT NULL THEN
                        -- Calcular duración en minutos
                        NEW.""DuracionMinutos"" := EXTRACT(EPOCH FROM (NEW.""FechaHoraFin"" - NEW.""FechaHoraInicio"")) / 60;

                        -- Detectar micro-paro (menos de 5 minutos)
                        IF NEW.""DuracionMinutos"" < 5 THEN
                            NEW.""EsMicroParo"" := TRUE;
                        END IF;

                        -- Si no se ha cerrado, marcarlo como cerrado
                        IF NEW.""Estado"" = 'Abierto' OR NEW.""Estado"" = 'EnAtencion' THEN
                            NEW.""Estado"" := 'Cerrado';
                            NEW.""FechaCierre"" := NOW();
                        END IF;
                    END IF;

                    -- Actualizar fecha de modificación
                    NEW.""FechaModificacion"" := NOW();

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trigger_calcular_duracion_paro
                BEFORE INSERT OR UPDATE ON linealytics.""RegistrosParos""
                FOR EACH ROW
                EXECUTE FUNCTION linealytics.calcular_duracion_paro();
            ");

            // ========== TRIGGER 2: Actualizar contadores de sesión al insertar/actualizar paro ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION linealytics.actualizar_sesion_desde_paro()
                RETURNS TRIGGER AS $$
                BEGIN
                    -- Actualizar tiempo de paro de la sesión
                    UPDATE linealytics.""SesionesProduccion""
                    SET ""TiempoParoMinutos"" = (
                        SELECT COALESCE(SUM(""DuracionMinutos""), 0)
                        FROM linealytics.""RegistrosParos""
                        WHERE ""SesionProduccionId"" = NEW.""SesionProduccionId""
                        AND ""DuracionMinutos"" IS NOT NULL
                    ),
                    ""TiempoProduccionMinutos"" = ""TiempoDisponibleMinutos"" - (
                        SELECT COALESCE(SUM(""DuracionMinutos""), 0)
                        FROM linealytics.""RegistrosParos""
                        WHERE ""SesionProduccionId"" = NEW.""SesionProduccionId""
                        AND ""DuracionMinutos"" IS NOT NULL
                    )
                    WHERE ""Id"" = NEW.""SesionProduccionId"";

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trigger_actualizar_sesion_desde_paro
                AFTER INSERT OR UPDATE ON linealytics.""RegistrosParos""
                FOR EACH ROW
                WHEN (NEW.""DuracionMinutos"" IS NOT NULL)
                EXECUTE FUNCTION linealytics.actualizar_sesion_desde_paro();
            ");

            // ========== TRIGGER 3: Calcular OEE automáticamente al actualizar sesión ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION linealytics.calcular_oee_sesion()
                RETURNS TRIGGER AS $$
                BEGIN
                    -- Calcular Disponibilidad (Availability)
                    IF NEW.""TiempoDisponibleMinutos"" > 0 THEN
                        NEW.""DisponibilidadPorcentaje"" := (NEW.""TiempoProduccionMinutos""::decimal / NEW.""TiempoDisponibleMinutos""::decimal) * 100;
                    ELSE
                        NEW.""DisponibilidadPorcentaje"" := 0;
                    END IF;

                    -- Calcular Rendimiento (Performance)
                    IF NEW.""ProductoId"" IS NOT NULL AND NEW.""TiempoProduccionMinutos"" > 0 THEN
                        DECLARE
                            tiempo_ciclo_segundos INTEGER;
                            unidades_teoricas DECIMAL;
                        BEGIN
                            -- Obtener tiempo de ciclo del producto
                            SELECT ""TiempoCicloSegundos"" INTO tiempo_ciclo_segundos
                            FROM linealytics.""Productos""
                            WHERE ""Id"" = NEW.""ProductoId"";

                            IF tiempo_ciclo_segundos > 0 THEN
                                -- Calcular unidades teóricas que deberían haberse producido
                                unidades_teoricas := (NEW.""TiempoProduccionMinutos"" * 60) / tiempo_ciclo_segundos;

                                IF unidades_teoricas > 0 THEN
                                    NEW.""RendimientoPorcentaje"" := (NEW.""UnidadesProducidas""::decimal / unidades_teoricas) * 100;
                                ELSE
                                    NEW.""RendimientoPorcentaje"" := 0;
                                END IF;
                            ELSE
                                NEW.""RendimientoPorcentaje"" := 0;
                            END IF;
                        END;
                    ELSE
                        NEW.""RendimientoPorcentaje"" := 0;
                    END IF;

                    -- Calcular Calidad (Quality)
                    IF NEW.""UnidadesProducidas"" > 0 THEN
                        NEW.""CalidadPorcentaje"" := (NEW.""UnidadesBuenas""::decimal / NEW.""UnidadesProducidas""::decimal) * 100;
                    ELSE
                        NEW.""CalidadPorcentaje"" := 0;
                    END IF;

                    -- Calcular OEE (Overall Equipment Effectiveness)
                    NEW.""OeePorcentaje"" := (NEW.""DisponibilidadPorcentaje"" * NEW.""RendimientoPorcentaje"" * NEW.""CalidadPorcentaje"") / 10000;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trigger_calcular_oee_sesion
                BEFORE INSERT OR UPDATE ON linealytics.""SesionesProduccion""
                FOR EACH ROW
                EXECUTE FUNCTION linealytics.calcular_oee_sesion();
            ");

            // ========== VISTA 1: Vista de análisis OEE por máquina ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaOeePorMaquina"" AS
                SELECT
                    m.""Id"" AS ""MaquinaId"",
                    m.""Codigo"" AS ""MaquinaCodigo"",
                    m.""Nombre"" AS ""MaquinaNombre"",
                    e.""Nombre"" AS ""EstacionNombre"",
                    l.""Nombre"" AS ""LineaNombre"",
                    a.""Nombre"" AS ""AreaNombre"",
                    COUNT(s.""Id"") AS ""TotalSesiones"",
                    AVG(s.""DisponibilidadPorcentaje"") AS ""DisponibilidadPromedio"",
                    AVG(s.""RendimientoPorcentaje"") AS ""RendimientoPromedio"",
                    AVG(s.""CalidadPorcentaje"") AS ""CalidadPromedio"",
                    AVG(s.""OeePorcentaje"") AS ""OeePromedio"",
                    SUM(s.""TiempoDisponibleMinutos"") AS ""TiempoDisponibleTotal"",
                    SUM(s.""TiempoProduccionMinutos"") AS ""TiempoProduccionTotal"",
                    SUM(s.""TiempoParoMinutos"") AS ""TiempoParoTotal"",
                    SUM(s.""UnidadesProducidas"") AS ""UnidadesProducidasTotal"",
                    SUM(s.""UnidadesDefectuosas"") AS ""UnidadesDefectuosasTotal"",
                    SUM(s.""UnidadesBuenas"") AS ""UnidadesBuenasTotal""
                FROM planta.""Maquinas"" m
                INNER JOIN planta.""Estaciones"" e ON m.""EstacionId"" = e.""Id""
                INNER JOIN planta.""Lineas"" l ON e.""LineaId"" = l.""Id""
                INNER JOIN planta.""Areas"" a ON l.""AreaId"" = a.""Id""
                LEFT JOIN linealytics.""SesionesProduccion"" s ON m.""Id"" = s.""MaquinaId""
                GROUP BY m.""Id"", m.""Codigo"", m.""Nombre"", e.""Nombre"", l.""Nombre"", a.""Nombre"";
            ");

            // ========== VISTA 2: Vista de Pareto de causas de paro ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaParetoCausasParo"" AS
                WITH causas_totales AS (
                    SELECT
                        cp.""Id"" AS ""CausaParoId"",
                        cp.""Nombre"" AS ""CausaNombre"",
                        cat.""Nombre"" AS ""CategoriaNombre"",
                        cat.""Color"" AS ""CategoriaColor"",
                        cat.""EsPlaneado"" AS ""EsPlaneado"",
                        COUNT(rp.""Id"") AS ""Frecuencia"",
                        SUM(rp.""DuracionMinutos"") AS ""TiempoTotalMinutos"",
                        AVG(rp.""DuracionMinutos"") AS ""TiempoPromedioMinutos""
                    FROM linealytics.""CausasParo"" cp
                    INNER JOIN linealytics.""CategoriasParo"" cat ON cp.""CategoriaParoId"" = cat.""Id""
                    LEFT JOIN linealytics.""RegistrosParos"" rp ON cp.""Id"" = rp.""CausaParoId""
                    WHERE rp.""DuracionMinutos"" IS NOT NULL
                    GROUP BY cp.""Id"", cp.""Nombre"", cat.""Nombre"", cat.""Color"", cat.""EsPlaneado""
                ),
                totales_generales AS (
                    SELECT
                        SUM(""Frecuencia"") AS ""FrecuenciaTotal"",
                        SUM(""TiempoTotalMinutos"") AS ""TiempoGrandTotal""
                    FROM causas_totales
                )
                SELECT
                    ct.*,
                    (ct.""Frecuencia""::decimal / tg.""FrecuenciaTotal"" * 100) AS ""PorcentajeFrecuencia"",
                    (ct.""TiempoTotalMinutos""::decimal / tg.""TiempoGrandTotal"" * 100) AS ""PorcentajeTiempo"",
                    SUM((ct.""TiempoTotalMinutos""::decimal / tg.""TiempoGrandTotal"" * 100))
                        OVER (ORDER BY ct.""TiempoTotalMinutos"" DESC) AS ""PorcentajeAcumulado""
                FROM causas_totales ct
                CROSS JOIN totales_generales tg
                WHERE ct.""Frecuencia"" > 0
                ORDER BY ct.""TiempoTotalMinutos"" DESC;
            ");

            // ========== VISTA 3: Vista de sesiones con detalles completos ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaSesionesDetalladas"" AS
                SELECT
                    s.""Id"" AS ""SesionId"",
                    s.""FechaInicio"",
                    s.""FechaFin"",
                    m.""Codigo"" AS ""MaquinaCodigo"",
                    m.""Nombre"" AS ""MaquinaNombre"",
                    e.""Nombre"" AS ""EstacionNombre"",
                    l.""Nombre"" AS ""LineaNombre"",
                    a.""Nombre"" AS ""AreaNombre"",
                    t.""Nombre"" AS ""TurnoNombre"",
                    p.""Codigo"" AS ""ProductoCodigo"",
                    p.""Nombre"" AS ""ProductoNombre"",
                    s.""TiempoDisponibleMinutos"",
                    s.""TiempoProduccionMinutos"",
                    s.""TiempoParoMinutos"",
                    s.""UnidadesProducidas"",
                    s.""UnidadesDefectuosas"",
                    s.""UnidadesBuenas"",
                    s.""DisponibilidadPorcentaje"",
                    s.""RendimientoPorcentaje"",
                    s.""CalidadPorcentaje"",
                    s.""OeePorcentaje"",
                    s.""Cerrada"",
                    COUNT(rp.""Id"") AS ""NumeroParos"",
                    COUNT(CASE WHEN rp.""EsMicroParo"" = TRUE THEN 1 END) AS ""NumeroMicroParos""
                FROM linealytics.""SesionesProduccion"" s
                INNER JOIN planta.""Maquinas"" m ON s.""MaquinaId"" = m.""Id""
                INNER JOIN planta.""Estaciones"" e ON m.""EstacionId"" = e.""Id""
                INNER JOIN planta.""Lineas"" l ON e.""LineaId"" = l.""Id""
                INNER JOIN planta.""Areas"" a ON l.""AreaId"" = a.""Id""
                INNER JOIN linealytics.""Turnos"" t ON s.""TurnoId"" = t.""Id""
                LEFT JOIN linealytics.""Productos"" p ON s.""ProductoId"" = p.""Id""
                LEFT JOIN linealytics.""RegistrosParos"" rp ON s.""Id"" = rp.""SesionProduccionId""
                GROUP BY s.""Id"", m.""Codigo"", m.""Nombre"", e.""Nombre"", l.""Nombre"", a.""Nombre"",
                         t.""Nombre"", p.""Codigo"", p.""Nombre"";
            ");

            // ========== VISTA 4: Vista de paros por categoría ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaParosPorCategoria"" AS
                SELECT
                    cat.""Id"" AS ""CategoriaId"",
                    cat.""Nombre"" AS ""CategoriaNombre"",
                    cat.""Color"" AS ""CategoriaColor"",
                    cat.""EsPlaneado"",
                    COUNT(rp.""Id"") AS ""TotalParos"",
                    SUM(rp.""DuracionMinutos"") AS ""TiempoTotalMinutos"",
                    AVG(rp.""DuracionMinutos"") AS ""TiempoPromedioMinutos"",
                    COUNT(CASE WHEN rp.""EsMicroParo"" = TRUE THEN 1 END) AS ""TotalMicroParos"",
                    COUNT(DISTINCT rp.""SesionProduccionId"") AS ""SesionesAfectadas""
                FROM linealytics.""CategoriasParo"" cat
                LEFT JOIN linealytics.""CausasParo"" cp ON cat.""Id"" = cp.""CategoriaParoId""
                LEFT JOIN linealytics.""RegistrosParos"" rp ON cp.""Id"" = rp.""CausaParoId""
                WHERE rp.""DuracionMinutos"" IS NOT NULL
                GROUP BY cat.""Id"", cat.""Nombre"", cat.""Color"", cat.""EsPlaneado""
                ORDER BY ""TiempoTotalMinutos"" DESC;
            ");

            // ========== VISTA 5: Vista de eficiencia por turno ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaEficienciaPorTurno"" AS
                SELECT
                    t.""Id"" AS ""TurnoId"",
                    t.""Nombre"" AS ""TurnoNombre"",
                    DATE(s.""FechaInicio"") AS ""Fecha"",
                    COUNT(s.""Id"") AS ""NumeroSesiones"",
                    AVG(s.""DisponibilidadPorcentaje"") AS ""DisponibilidadPromedio"",
                    AVG(s.""RendimientoPorcentaje"") AS ""RendimientoPromedio"",
                    AVG(s.""CalidadPorcentaje"") AS ""CalidadPromedio"",
                    AVG(s.""OeePorcentaje"") AS ""OeePromedio"",
                    SUM(s.""UnidadesProducidas"") AS ""UnidadesProducidas"",
                    SUM(s.""UnidadesBuenas"") AS ""UnidadesBuenas"",
                    SUM(s.""TiempoParoMinutos"") AS ""TiempoParoTotal""
                FROM linealytics.""Turnos"" t
                LEFT JOIN linealytics.""SesionesProduccion"" s ON t.""Id"" = s.""TurnoId""
                WHERE s.""Id"" IS NOT NULL
                GROUP BY t.""Id"", t.""Nombre"", DATE(s.""FechaInicio"")
                ORDER BY DATE(s.""FechaInicio"") DESC, t.""Nombre"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar vistas en orden inverso
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaEficienciaPorTurno"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaParosPorCategoria"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaSesionesDetalladas"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaParetoCausasParo"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaOeePorMaquina"";");

            // Eliminar triggers
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trigger_calcular_oee_sesion ON linealytics.""SesionesProduccion"";");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trigger_actualizar_sesion_desde_paro ON linealytics.""RegistrosParos"";");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trigger_calcular_duracion_paro ON linealytics.""RegistrosParos"";");

            // Eliminar funciones
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS linealytics.calcular_oee_sesion();");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS linealytics.actualizar_sesion_desde_paro();");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS linealytics.calcular_duracion_paro();");
        }
    }
}
