using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBotonesWithDepartamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Botones_Areas_AreaId",
                schema: "planta",
                table: "Botones");

            migrationBuilder.DropIndex(
                name: "IX_Botones_DireccionMAC",
                schema: "planta",
                table: "Botones");

            migrationBuilder.DropColumn(
                name: "DireccionIP",
                schema: "planta",
                table: "Botones");

            migrationBuilder.DropColumn(
                name: "DireccionMAC",
                schema: "planta",
                table: "Botones");

            migrationBuilder.RenameColumn(
                name: "AreaId",
                schema: "planta",
                table: "Botones",
                newName: "DepartamentoOperadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Botones_AreaId",
                schema: "planta",
                table: "Botones",
                newName: "IX_Botones_DepartamentoOperadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Botones_DepartamentosOperador_DepartamentoOperadorId",
                schema: "planta",
                table: "Botones",
                column: "DepartamentoOperadorId",
                principalSchema: "operadores",
                principalTable: "DepartamentosOperador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Botones_DepartamentosOperador_DepartamentoOperadorId",
                schema: "planta",
                table: "Botones");

            migrationBuilder.RenameColumn(
                name: "DepartamentoOperadorId",
                schema: "planta",
                table: "Botones",
                newName: "AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Botones_DepartamentoOperadorId",
                schema: "planta",
                table: "Botones",
                newName: "IX_Botones_AreaId");

            migrationBuilder.AddColumn<string>(
                name: "DireccionIP",
                schema: "planta",
                table: "Botones",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionMAC",
                schema: "planta",
                table: "Botones",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Botones_DireccionMAC",
                schema: "planta",
                table: "Botones",
                column: "DireccionMAC");

            migrationBuilder.AddForeignKey(
                name: "FK_Botones_Areas_AreaId",
                schema: "planta",
                table: "Botones",
                column: "AreaId",
                principalSchema: "planta",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
