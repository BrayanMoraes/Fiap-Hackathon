using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_MEDICO_ESPECIALIDADE",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MEDICO_ESPECIALIDADE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_PACIENTE",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SENHA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PACIENTE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_MEDICO",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CRM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_ESPECIALIDADE = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SENHA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MEDICO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_MEDICO_TB_MEDICO_ESPECIALIDADE_ID_ESPECIALIDADE",
                        column: x => x.ID_ESPECIALIDADE,
                        principalTable: "TB_MEDICO_ESPECIALIDADE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TB_AGENDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_MEDICO = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HORARIO = table.Column<TimeSpan>(type: "time", nullable: false),
                    DATA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VALOR_CONSULTA = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AGENDA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_AGENDA_TB_MEDICO_ID_MEDICO",
                        column: x => x.ID_MEDICO,
                        principalTable: "TB_MEDICO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONSULTA_AGENDADA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_PACIENTE = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_MEDICO = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID_AGENDA = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APROVADO = table.Column<bool>(type: "bit", nullable: false),
                    CANCELADA = table.Column<bool>(type: "bit", nullable: false),
                    MOTIVO_CANCELAMENTO = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONSULTA_AGENDADA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TB_CONSULTA_AGENDADA_TB_AGENDA_ID_AGENDA",
                        column: x => x.ID_AGENDA,
                        principalTable: "TB_AGENDA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TB_CONSULTA_AGENDADA_TB_MEDICO_ID_MEDICO",
                        column: x => x.ID_MEDICO,
                        principalTable: "TB_MEDICO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TB_CONSULTA_AGENDADA_TB_PACIENTE_ID_PACIENTE",
                        column: x => x.ID_PACIENTE,
                        principalTable: "TB_PACIENTE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_AGENDA_ID_MEDICO",
                table: "TB_AGENDA",
                column: "ID_MEDICO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONSULTA_AGENDADA_ID_AGENDA",
                table: "TB_CONSULTA_AGENDADA",
                column: "ID_AGENDA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONSULTA_AGENDADA_ID_MEDICO",
                table: "TB_CONSULTA_AGENDADA",
                column: "ID_MEDICO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONSULTA_AGENDADA_ID_PACIENTE",
                table: "TB_CONSULTA_AGENDADA",
                column: "ID_PACIENTE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MEDICO_ID_ESPECIALIDADE",
                table: "TB_MEDICO",
                column: "ID_ESPECIALIDADE");

            migrationBuilder.Sql(@"
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Cardiologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Dermatologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Gastroenterologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Neurologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Pediatria');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Psiquiatria');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Ortopedia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Oftalmologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Oncologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Endocrinologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Geriatria');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Reumatologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Anestesiologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Urologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Nefrologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Hematologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Angiologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Imunologia');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Cirurgia Geral');
    INSERT INTO TB_MEDICO_ESPECIALIDADE (ID, DESCRICAO) VALUES (NEWID(), 'Cirurgia Plástica');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CONSULTA_AGENDADA");

            migrationBuilder.DropTable(
                name: "TB_AGENDA");

            migrationBuilder.DropTable(
                name: "TB_PACIENTE");

            migrationBuilder.DropTable(
                name: "TB_MEDICO");

            migrationBuilder.DropTable(
                name: "TB_MEDICO_ESPECIALIDADE");

            migrationBuilder.Sql(@"
    DELETE FROM TB_MEDICO_ESPECIALIDADE WHERE DESCRICAO IN (
        'Cardiologia', 'Dermatologia', 'Gastroenterologia', 'Neurologia', 
        'Pediatria', 'Psiquiatria', 'Ortopedia', 'Oftalmologia', 
        'Oncologia', 'Endocrinologia', 'Geriatria', 'Reumatologia', 
        'Anestesiologia', 'Urologia', 'Nefrologia', 'Hematologia', 
        'Angiologia', 'Imunologia', 'Cirurgia Geral', 'Cirurgia Plástica'
    );
");
        }
    }
}
