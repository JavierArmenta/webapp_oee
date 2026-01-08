using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCausasYCategoriasParoManual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Paso 1: Eliminar la foreign key de RegistrosParos a CausasParo
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosParos_CausasParo_CausaParoId",
                schema: "linealytics",
                table: "RegistrosParos");

            // Paso 2: Eliminar el índice de CausaParoId
            migrationBuilder.DropIndex(
                name: "IX_RegistrosParos_CausaParoId",
                schema: "linealytics",
                table: "RegistrosParos");

            // Paso 3: Eliminar la columna CausaParoId de RegistrosParos
            migrationBuilder.DropColumn(
                name: "CausaParoId",
                schema: "linealytics",
                table: "RegistrosParos");

            // Paso 4: Eliminar la tabla CausasParo (esto automáticamente eliminará la FK con CategoriasParo)
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS linealytics.""CausasParo"" CASCADE;");

            // Paso 5: Eliminar la tabla CategoriasParo
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS linealytics.""CategoriasParo"" CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nota: Esta migración no es reversible ya que los datos se perderán permanentemente
            // Si necesitas revertir, deberás recrear las tablas manualmente desde una migración anterior
            throw new NotSupportedException("Esta migración no puede ser revertida. Los datos de CausasParo y CategoriasParo se han eliminado permanentemente.");
        }
    }
}
