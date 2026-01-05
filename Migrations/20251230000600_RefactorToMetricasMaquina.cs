using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToMetricasMaquina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Paso 1: Eliminar triggers antiguos que dependen de SesionProduccionId
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trigger_actualizar_sesion_desde_paro ON linealytics.""RegistrosParos"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS linealytics.actualizar_sesion_desde_paro();");

            // Paso 2: Eliminar vistas que referencian la estructura antigua
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaEficienciaPorTurno"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaParosPorCategoria"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaSesionesDetalladas"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaParetoCausasParo"";");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS linealytics.""VistaOeePorMaquina"";");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosParos_SesionesProduccion_SesionProduccionId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosParos_SesionProduccionId_FechaHoraInicio",
                schema: "linealytics",
                table: "RegistrosParos");

            // Eliminar completamente la columna SesionProduccionId
            migrationBuilder.DropColumn(
                name: "SesionProduccionId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.AddColumn<int>(
                name: "MaquinaId",
                schema: "linealytics",
                table: "RegistrosParos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MetricasMaquinaId",
                schema: "linealytics",
                table: "RegistrosParos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MetricasMaquina",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    TurnoId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TiempoDisponibleMinutos = table.Column<int>(type: "integer", nullable: false),
                    TiempoProduccionMinutos = table.Column<int>(type: "integer", nullable: false),
                    TiempoParoMinutos = table.Column<int>(type: "integer", nullable: false),
                    UnidadesProducidas = table.Column<int>(type: "integer", nullable: false),
                    UnidadesDefectuosas = table.Column<int>(type: "integer", nullable: false),
                    UnidadesBuenas = table.Column<int>(type: "integer", nullable: false),
                    DisponibilidadPorcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    RendimientoPorcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CalidadPorcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    OeePorcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Cerrada = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricasMaquina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetricasMaquina_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalSchema: "planta",
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetricasMaquina_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalSchema: "linealytics",
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetricasMaquina_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalSchema: "linealytics",
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_MaquinaId_FechaHoraInicio",
                schema: "linealytics",
                table: "RegistrosParos",
                columns: new[] { "MaquinaId", "FechaHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_MetricasMaquinaId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "MetricasMaquinaId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricasMaquina_MaquinaId_FechaInicio",
                schema: "linealytics",
                table: "MetricasMaquina",
                columns: new[] { "MaquinaId", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_MetricasMaquina_ProductoId",
                schema: "linealytics",
                table: "MetricasMaquina",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_MetricasMaquina_TurnoId",
                schema: "linealytics",
                table: "MetricasMaquina",
                column: "TurnoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosParos_Maquinas_MaquinaId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "MaquinaId",
                principalSchema: "planta",
                principalTable: "Maquinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosParos_MetricasMaquina_MetricasMaquinaId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "MetricasMaquinaId",
                principalSchema: "linealytics",
                principalTable: "MetricasMaquina",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // ========== NUEVO TRIGGER: Actualizar MetricasMaquina al insertar/actualizar paro ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION linealytics.actualizar_metricas_desde_paro()
                RETURNS TRIGGER AS $$
                BEGIN
                    -- Solo actualizar si el paro está asociado a una métrica
                    IF NEW.""MetricasMaquinaId"" IS NOT NULL THEN
                        -- Actualizar tiempo de paro de la métrica
                        UPDATE linealytics.""MetricasMaquina""
                        SET ""TiempoParoMinutos"" = (
                            SELECT COALESCE(SUM(""DuracionMinutos""), 0)
                            FROM linealytics.""RegistrosParos""
                            WHERE ""MetricasMaquinaId"" = NEW.""MetricasMaquinaId""
                            AND ""DuracionMinutos"" IS NOT NULL
                        ),
                        ""TiempoProduccionMinutos"" = ""TiempoDisponibleMinutos"" - (
                            SELECT COALESCE(SUM(""DuracionMinutos""), 0)
                            FROM linealytics.""RegistrosParos""
                            WHERE ""MetricasMaquinaId"" = NEW.""MetricasMaquinaId""
                            AND ""DuracionMinutos"" IS NOT NULL
                        )
                        WHERE ""Id"" = NEW.""MetricasMaquinaId"";
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trigger_actualizar_metricas_desde_paro
                AFTER INSERT OR UPDATE ON linealytics.""RegistrosParos""
                FOR EACH ROW
                WHEN (NEW.""DuracionMinutos"" IS NOT NULL AND NEW.""MetricasMaquinaId"" IS NOT NULL)
                EXECUTE FUNCTION linealytics.actualizar_metricas_desde_paro();
            ");

            // ========== NUEVO TRIGGER: Calcular OEE en MetricasMaquina ==========
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION linealytics.calcular_oee_metricas()
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
                CREATE TRIGGER trigger_calcular_oee_metricas
                BEFORE INSERT OR UPDATE ON linealytics.""MetricasMaquina""
                FOR EACH ROW
                EXECUTE FUNCTION linealytics.calcular_oee_metricas();
            ");

            // ========== NUEVAS VISTAS ACTUALIZADAS ==========

            // Vista 1: OEE por Máquina (ahora basada en MetricasMaquina)
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaOeePorMaquina"" AS
                SELECT
                    m.""Id"" AS ""MaquinaId"",
                    m.""Codigo"" AS ""MaquinaCodigo"",
                    m.""Nombre"" AS ""MaquinaNombre"",
                    e.""Nombre"" AS ""EstacionNombre"",
                    l.""Nombre"" AS ""LineaNombre"",
                    a.""Nombre"" AS ""AreaNombre"",
                    COUNT(mm.""Id"") AS ""TotalMetricas"",
                    AVG(mm.""DisponibilidadPorcentaje"") AS ""DisponibilidadPromedio"",
                    AVG(mm.""RendimientoPorcentaje"") AS ""RendimientoPromedio"",
                    AVG(mm.""CalidadPorcentaje"") AS ""CalidadPromedio"",
                    AVG(mm.""OeePorcentaje"") AS ""OeePromedio"",
                    SUM(mm.""TiempoDisponibleMinutos"") AS ""TiempoDisponibleTotal"",
                    SUM(mm.""TiempoProduccionMinutos"") AS ""TiempoProduccionTotal"",
                    SUM(mm.""TiempoParoMinutos"") AS ""TiempoParoTotal"",
                    SUM(mm.""UnidadesProducidas"") AS ""UnidadesProducidasTotal"",
                    SUM(mm.""UnidadesDefectuosas"") AS ""UnidadesDefectuosasTotal"",
                    SUM(mm.""UnidadesBuenas"") AS ""UnidadesBuenasTotal""
                FROM planta.""Maquinas"" m
                INNER JOIN planta.""Estaciones"" e ON m.""EstacionId"" = e.""Id""
                INNER JOIN planta.""Lineas"" l ON e.""LineaId"" = l.""Id""
                INNER JOIN planta.""Areas"" a ON l.""AreaId"" = a.""Id""
                LEFT JOIN linealytics.""MetricasMaquina"" mm ON m.""Id"" = mm.""MaquinaId""
                GROUP BY m.""Id"", m.""Codigo"", m.""Nombre"", e.""Nombre"", l.""Nombre"", a.""Nombre"";
            ");

            // Vista 2: Pareto de Causas de Paro (sin cambios significativos)
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

            // Vista 3: Paros por Categoría (actualizada para usar MaquinaId)
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
                    COUNT(DISTINCT rp.""MaquinaId"") AS ""MaquinasAfectadas""
                FROM linealytics.""CategoriasParo"" cat
                LEFT JOIN linealytics.""CausasParo"" cp ON cat.""Id"" = cp.""CategoriaParoId""
                LEFT JOIN linealytics.""RegistrosParos"" rp ON cp.""Id"" = rp.""CausaParoId""
                WHERE rp.""DuracionMinutos"" IS NOT NULL
                GROUP BY cat.""Id"", cat.""Nombre"", cat.""Color"", cat.""EsPlaneado""
                ORDER BY ""TiempoTotalMinutos"" DESC;
            ");

            // Vista 4: Métricas detalladas
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaMetricasDetalladas"" AS
                SELECT
                    mm.""Id"" AS ""MetricaId"",
                    mm.""FechaInicio"",
                    mm.""FechaFin"",
                    m.""Codigo"" AS ""MaquinaCodigo"",
                    m.""Nombre"" AS ""MaquinaNombre"",
                    e.""Nombre"" AS ""EstacionNombre"",
                    l.""Nombre"" AS ""LineaNombre"",
                    a.""Nombre"" AS ""AreaNombre"",
                    t.""Nombre"" AS ""TurnoNombre"",
                    p.""Codigo"" AS ""ProductoCodigo"",
                    p.""Nombre"" AS ""ProductoNombre"",
                    mm.""TiempoDisponibleMinutos"",
                    mm.""TiempoProduccionMinutos"",
                    mm.""TiempoParoMinutos"",
                    mm.""UnidadesProducidas"",
                    mm.""UnidadesDefectuosas"",
                    mm.""UnidadesBuenas"",
                    mm.""DisponibilidadPorcentaje"",
                    mm.""RendimientoPorcentaje"",
                    mm.""CalidadPorcentaje"",
                    mm.""OeePorcentaje"",
                    mm.""Cerrada"",
                    COUNT(rp.""Id"") AS ""NumeroParos"",
                    COUNT(CASE WHEN rp.""EsMicroParo"" = TRUE THEN 1 END) AS ""NumeroMicroParos""
                FROM linealytics.""MetricasMaquina"" mm
                INNER JOIN planta.""Maquinas"" m ON mm.""MaquinaId"" = m.""Id""
                INNER JOIN planta.""Estaciones"" e ON m.""EstacionId"" = e.""Id""
                INNER JOIN planta.""Lineas"" l ON e.""LineaId"" = l.""Id""
                INNER JOIN planta.""Areas"" a ON l.""AreaId"" = a.""Id""
                INNER JOIN linealytics.""Turnos"" t ON mm.""TurnoId"" = t.""Id""
                LEFT JOIN linealytics.""Productos"" p ON mm.""ProductoId"" = p.""Id""
                LEFT JOIN linealytics.""RegistrosParos"" rp ON mm.""Id"" = rp.""MetricasMaquinaId""
                GROUP BY mm.""Id"", m.""Codigo"", m.""Nombre"", e.""Nombre"", l.""Nombre"", a.""Nombre"",
                         t.""Nombre"", p.""Codigo"", p.""Nombre"";
            ");

            // Vista 5: Eficiencia por Turno (basada en MetricasMaquina)
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW linealytics.""VistaEficienciaPorTurno"" AS
                SELECT
                    t.""Id"" AS ""TurnoId"",
                    t.""Nombre"" AS ""TurnoNombre"",
                    DATE(mm.""FechaInicio"") AS ""Fecha"",
                    COUNT(mm.""Id"") AS ""NumeroMetricas"",
                    AVG(mm.""DisponibilidadPorcentaje"") AS ""DisponibilidadPromedio"",
                    AVG(mm.""RendimientoPorcentaje"") AS ""RendimientoPromedio"",
                    AVG(mm.""CalidadPorcentaje"") AS ""CalidadPromedio"",
                    AVG(mm.""OeePorcentaje"") AS ""OeePromedio"",
                    SUM(mm.""UnidadesProducidas"") AS ""UnidadesProducidas"",
                    SUM(mm.""UnidadesBuenas"") AS ""UnidadesBuenas"",
                    SUM(mm.""TiempoParoMinutos"") AS ""TiempoParoTotal""
                FROM linealytics.""Turnos"" t
                LEFT JOIN linealytics.""MetricasMaquina"" mm ON t.""Id"" = mm.""TurnoId""
                WHERE mm.""Id"" IS NOT NULL
                GROUP BY t.""Id"", t.""Nombre"", DATE(mm.""FechaInicio"")
                ORDER BY DATE(mm.""FechaInicio"") DESC, t.""Nombre"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosParos_Maquinas_MaquinaId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosParos_MetricasMaquina_MetricasMaquinaId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosParos_SesionesProduccion_SesionProduccionId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropTable(
                name: "MetricasMaquina",
                schema: "linealytics");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosParos_MaquinaId_FechaHoraInicio",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosParos_MetricasMaquinaId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosParos_SesionProduccionId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropColumn(
                name: "MaquinaId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.DropColumn(
                name: "MetricasMaquinaId",
                schema: "linealytics",
                table: "RegistrosParos");

            migrationBuilder.AlterColumn<int>(
                name: "SesionProduccionId",
                schema: "linealytics",
                table: "RegistrosParos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_SesionProduccionId_FechaHoraInicio",
                schema: "linealytics",
                table: "RegistrosParos",
                columns: new[] { "SesionProduccionId", "FechaHoraInicio" });

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosParos_SesionesProduccion_SesionProduccionId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "SesionProduccionId",
                principalSchema: "linealytics",
                principalTable: "SesionesProduccion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
