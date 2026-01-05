using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameRolesToDepartamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperadorRolesOperador",
                schema: "operadores");

            migrationBuilder.DropTable(
                name: "RolesOperador",
                schema: "operadores");

            migrationBuilder.CreateTable(
                name: "DepartamentosOperador",
                schema: "operadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartamentosOperador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperadorDepartamentos",
                schema: "operadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperadorId = table.Column<int>(type: "integer", nullable: false),
                    DepartamentoOperadorId = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperadorDepartamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperadorDepartamentos_DepartamentosOperador_DepartamentoOpe~",
                        column: x => x.DepartamentoOperadorId,
                        principalSchema: "operadores",
                        principalTable: "DepartamentosOperador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperadorDepartamentos_Operadores_OperadorId",
                        column: x => x.OperadorId,
                        principalSchema: "operadores",
                        principalTable: "Operadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartamentosOperador_Nombre",
                schema: "operadores",
                table: "DepartamentosOperador",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperadorDepartamentos_DepartamentoOperadorId",
                schema: "operadores",
                table: "OperadorDepartamentos",
                column: "DepartamentoOperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperadorDepartamentos_OperadorId_DepartamentoOperadorId",
                schema: "operadores",
                table: "OperadorDepartamentos",
                columns: new[] { "OperadorId", "DepartamentoOperadorId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperadorDepartamentos",
                schema: "operadores");

            migrationBuilder.DropTable(
                name: "DepartamentosOperador",
                schema: "operadores");

            migrationBuilder.CreateTable(
                name: "RolesOperador",
                schema: "operadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesOperador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperadorRolesOperador",
                schema: "operadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperadorId = table.Column<int>(type: "integer", nullable: false),
                    RolOperadorId = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperadorRolesOperador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperadorRolesOperador_Operadores_OperadorId",
                        column: x => x.OperadorId,
                        principalSchema: "operadores",
                        principalTable: "Operadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperadorRolesOperador_RolesOperador_RolOperadorId",
                        column: x => x.RolOperadorId,
                        principalSchema: "operadores",
                        principalTable: "RolesOperador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperadorRolesOperador_OperadorId_RolOperadorId",
                schema: "operadores",
                table: "OperadorRolesOperador",
                columns: new[] { "OperadorId", "RolOperadorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperadorRolesOperador_RolOperadorId",
                schema: "operadores",
                table: "OperadorRolesOperador",
                column: "RolOperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesOperador_Nombre",
                schema: "operadores",
                table: "RolesOperador",
                column: "Nombre",
                unique: true);
        }
    }
}
