using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlteradoAtributosNulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CANCELADA",
                table: "TB_CONSULTA_AGENDADA",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "APROVADO",
                table: "TB_CONSULTA_AGENDADA",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CANCELADA",
                table: "TB_CONSULTA_AGENDADA",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "APROVADO",
                table: "TB_CONSULTA_AGENDADA",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
