using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLinealyticsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "linealytics");

            migrationBuilder.CreateTable(
                name: "CategoriasParo",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EsPlaneado = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasParo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TiempoCicloSegundos = table.Column<int>(type: "integer", nullable: false),
                    UnidadesPorCiclo = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DuracionMinutos = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CausasParo",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoriaParoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CodigoInterno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RequiereMantenimiento = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CausasParo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CausasParo_CategoriasParo_CategoriaParoId",
                        column: x => x.CategoriaParoId,
                        principalSchema: "linealytics",
                        principalTable: "CategoriasParo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SesionesProduccion",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    TurnoId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    Observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Cerrada = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesProduccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionesProduccion_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalSchema: "planta",
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SesionesProduccion_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalSchema: "linealytics",
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SesionesProduccion_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalSchema: "linealytics",
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosParos",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SesionProduccionId = table.Column<int>(type: "integer", nullable: false),
                    CausaParoId = table.Column<int>(type: "integer", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DuracionMinutos = table.Column<int>(type: "integer", nullable: true),
                    OperadorResponsableId = table.Column<int>(type: "integer", nullable: true),
                    OperadorSolucionaId = table.Column<int>(type: "integer", nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Solucion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EsMicroParo = table.Column<bool>(type: "boolean", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaAtencion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosParos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosParos_CausasParo_CausaParoId",
                        column: x => x.CausaParoId,
                        principalSchema: "linealytics",
                        principalTable: "CausasParo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosParos_Operadores_OperadorResponsableId",
                        column: x => x.OperadorResponsableId,
                        principalSchema: "operadores",
                        principalTable: "Operadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegistrosParos_Operadores_OperadorSolucionaId",
                        column: x => x.OperadorSolucionaId,
                        principalSchema: "operadores",
                        principalTable: "Operadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegistrosParos_SesionesProduccion_SesionProduccionId",
                        column: x => x.SesionProduccionId,
                        principalSchema: "linealytics",
                        principalTable: "SesionesProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosProduccionHora",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SesionProduccionId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnidadesProducidas = table.Column<int>(type: "integer", nullable: false),
                    UnidadesDefectuosas = table.Column<int>(type: "integer", nullable: false),
                    UnidadesBuenas = table.Column<int>(type: "integer", nullable: false),
                    TiempoProduccionMinutos = table.Column<int>(type: "integer", nullable: false),
                    TiempoParoMinutos = table.Column<int>(type: "integer", nullable: false),
                    OperadorId = table.Column<int>(type: "integer", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosProduccionHora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosProduccionHora_Operadores_OperadorId",
                        column: x => x.OperadorId,
                        principalSchema: "operadores",
                        principalTable: "Operadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegistrosProduccionHora_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalSchema: "linealytics",
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosProduccionHora_SesionesProduccion_SesionProduccion~",
                        column: x => x.SesionProduccionId,
                        principalSchema: "linealytics",
                        principalTable: "SesionesProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialCambiosParos",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistroParoId = table.Column<int>(type: "integer", nullable: false),
                    CampoModificado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ValorAnterior = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ValorNuevo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UsuarioModifica = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialCambiosParos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialCambiosParos_RegistrosParos_RegistroParoId",
                        column: x => x.RegistroParoId,
                        principalSchema: "linealytics",
                        principalTable: "RegistrosParos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasParo_Nombre",
                schema: "linealytics",
                table: "CategoriasParo",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CausasParo_CategoriaParoId_Nombre",
                schema: "linealytics",
                table: "CausasParo",
                columns: new[] { "CategoriaParoId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialCambiosParos_RegistroParoId",
                schema: "linealytics",
                table: "HistorialCambiosParos",
                column: "RegistroParoId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Codigo",
                schema: "linealytics",
                table: "Productos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_CausaParoId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "CausaParoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_Estado",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_OperadorResponsableId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "OperadorResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_OperadorSolucionaId",
                schema: "linealytics",
                table: "RegistrosParos",
                column: "OperadorSolucionaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParos_SesionProduccionId_FechaHoraInicio",
                schema: "linealytics",
                table: "RegistrosParos",
                columns: new[] { "SesionProduccionId", "FechaHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosProduccionHora_OperadorId",
                schema: "linealytics",
                table: "RegistrosProduccionHora",
                column: "OperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosProduccionHora_ProductoId",
                schema: "linealytics",
                table: "RegistrosProduccionHora",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosProduccionHora_SesionProduccionId_FechaHora",
                schema: "linealytics",
                table: "RegistrosProduccionHora",
                columns: new[] { "SesionProduccionId", "FechaHora" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SesionesProduccion_MaquinaId_FechaInicio",
                schema: "linealytics",
                table: "SesionesProduccion",
                columns: new[] { "MaquinaId", "FechaInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_SesionesProduccion_ProductoId",
                schema: "linealytics",
                table: "SesionesProduccion",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesProduccion_TurnoId",
                schema: "linealytics",
                table: "SesionesProduccion",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_Nombre",
                schema: "linealytics",
                table: "Turnos",
                column: "Nombre",
                unique: true);

            // Insertar categorías de paro estándar
            migrationBuilder.InsertData(
                schema: "linealytics",
                table: "CategoriasParo",
                columns: new[] { "Nombre", "Descripcion", "Color", "EsPlaneado", "Activo", "FechaCreacion" },
                values: new object[,]
                {
                    { "Falla Mecánica", "Paros causados por fallas en componentes mecánicos", "#DC3545", false, true, DateTime.UtcNow },
                    { "Falla Eléctrica", "Paros relacionados con el sistema eléctrico", "#FFC107", false, true, DateTime.UtcNow },
                    { "Falla Neumática/Hidráulica", "Problemas en sistemas neumáticos o hidráulicos", "#17A2B8", false, true, DateTime.UtcNow },
                    { "Cambio de Producto", "Tiempo para cambiar configuración de producto", "#6C757D", true, true, DateTime.UtcNow },
                    { "Ajuste de Máquina", "Ajustes y calibraciones requeridas", "#FD7E14", false, true, DateTime.UtcNow },
                    { "Falta de Material", "Ausencia de materia prima o componentes", "#E83E8C", false, true, DateTime.UtcNow },
                    { "Falta de Personal", "Personal insuficiente o ausente", "#6610F2", false, true, DateTime.UtcNow },
                    { "Problema de Calidad", "Detección de problemas de calidad", "#D63384", false, true, DateTime.UtcNow },
                    { "Mantenimiento Preventivo", "Mantenimiento programado", "#20C997", true, true, DateTime.UtcNow },
                    { "Mantenimiento Correctivo", "Reparaciones no programadas", "#DC3545", false, true, DateTime.UtcNow },
                    { "Limpieza Programada", "Limpieza y sanitización programada", "#0DCAF0", true, true, DateTime.UtcNow },
                    { "Sin Orden de Producción", "No hay orden de producción asignada", "#6C757D", true, true, DateTime.UtcNow },
                    { "Otros", "Otros paros no clasificados", "#6C757D", false, true, DateTime.UtcNow }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialCambiosParos",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "RegistrosProduccionHora",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "RegistrosParos",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "CausasParo",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "SesionesProduccion",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "CategoriasParo",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "Productos",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "Turnos",
                schema: "linealytics");
        }
    }
}
