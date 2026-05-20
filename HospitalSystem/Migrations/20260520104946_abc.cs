using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.Migrations
{
    /// <inheritdoc />
    public partial class abc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Bloco = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TipoSanguineo = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CPF = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CRM_COREN = table.Column<string>(type: "TEXT", nullable: false),
                    DepartamentoId = table.Column<int>(type: "INTEGER", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Turno = table.Column<string>(type: "TEXT", nullable: true),
                    EspecialidadePrincipal = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CPF = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionario_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FichasMedicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HistoricoAlergias = table.Column<string>(type: "TEXT", nullable: false),
                    PacienteId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichasMedicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichasMedicas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Diagnostico = table.Column<string>(type: "TEXT", nullable: false),
                    PacienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedicoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Funcionario_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_MedicoId",
                table: "Consultas",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_FichasMedicas_PacienteId",
                table: "FichasMedicas",
                column: "PacienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_DepartamentoId",
                table: "Funcionario",
                column: "DepartamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "FichasMedicas");

            migrationBuilder.DropTable(
                name: "Funcionario");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Departamentos");
        }
    }
}
