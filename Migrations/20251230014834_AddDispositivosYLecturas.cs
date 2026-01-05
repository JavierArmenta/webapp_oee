using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDispositivosYLecturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dispositivos",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CodigoDispositivo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TipoDispositivo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UltimoContador = table.Column<int>(type: "integer", nullable: true),
                    FechaUltimaLectura = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispositivos_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalSchema: "planta",
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LecturasContador",
                schema: "linealytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    DispositivoId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: true),
                    MetricasMaquinaId = table.Column<int>(type: "integer", nullable: true),
                    Contador = table.Column<int>(type: "integer", nullable: false),
                    ContadorAnterior = table.Column<int>(type: "integer", nullable: true),
                    UnidadesProducidas = table.Column<int>(type: "integer", nullable: true),
                    FechaLectura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturasContador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LecturasContador_Dispositivos_DispositivoId",
                        column: x => x.DispositivoId,
                        principalSchema: "linealytics",
                        principalTable: "Dispositivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LecturasContador_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalSchema: "planta",
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LecturasContador_MetricasMaquina_MetricasMaquinaId",
                        column: x => x.MetricasMaquinaId,
                        principalSchema: "linealytics",
                        principalTable: "MetricasMaquina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LecturasContador_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalSchema: "linealytics",
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_MaquinaId_CodigoDispositivo",
                schema: "linealytics",
                table: "Dispositivos",
                columns: new[] { "MaquinaId", "CodigoDispositivo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LecturasContador_DispositivoId",
                schema: "linealytics",
                table: "LecturasContador",
                column: "DispositivoId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturasContador_MaquinaId_FechaLectura",
                schema: "linealytics",
                table: "LecturasContador",
                columns: new[] { "MaquinaId", "FechaLectura" });

            migrationBuilder.CreateIndex(
                name: "IX_LecturasContador_MetricasMaquinaId",
                schema: "linealytics",
                table: "LecturasContador",
                column: "MetricasMaquinaId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturasContador_ProductoId",
                schema: "linealytics",
                table: "LecturasContador",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturasContador",
                schema: "linealytics");

            migrationBuilder.DropTable(
                name: "Dispositivos",
                schema: "linealytics");
        }
    }
}
