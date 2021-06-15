using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestTask.Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Surname = table.Column<string>(type: "TEXT", nullable: true),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyCustomer = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyExecutor = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    LeaderId = table.Column<int>(type: "INTEGER", nullable: true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Finish = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Priority = table.Column<byte>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Employee_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProject",
                columns: table => new
                {
                    ExecutorProjectsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExecutorsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProject", x => new { x.ExecutorProjectsId, x.ExecutorsId });
                    table.ForeignKey(
                        name: "FK_EmployeeProject_Employee_ExecutorsId",
                        column: x => x.ExecutorsId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeProject_Project_ExecutorProjectsId",
                        column: x => x.ExecutorProjectsId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "Email", "Name", "Patronymic", "Surname" },
                values: new object[] { 1, "igor.p@yandex.ru", "Игорь", "Платонович", "Панфлов" });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "Email", "Name", "Patronymic", "Surname" },
                values: new object[] { 2, "stepan.knyazev@mail.ru", "Степан", "Даниилович", "Князев" });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "Email", "Name", "Patronymic", "Surname" },
                values: new object[] { 3, "kornilov224@gmail.com", "Аверьян", "Демьянович", "Корнилов" });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "CompanyCustomer", "CompanyExecutor", "EmployeeId", "Finish", "LeaderId", "Name", "Priority", "Start" },
                values: new object[] { 1, "Customer", "Executor", 1, new DateTime(2021, 6, 24, 22, 20, 56, 957, DateTimeKind.Local).AddTicks(6920), 1, "SuperProject", (byte)3, new DateTime(2021, 6, 14, 22, 20, 56, 957, DateTimeKind.Local).AddTicks(126) });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "CompanyCustomer", "CompanyExecutor", "EmployeeId", "Finish", "LeaderId", "Name", "Priority", "Start" },
                values: new object[] { 2, "Customer2", "Executor2", 2, new DateTime(2021, 6, 27, 22, 20, 56, 957, DateTimeKind.Local).AddTicks(7426), 2, "SuperProject2", (byte)1, new DateTime(2021, 6, 14, 22, 20, 56, 957, DateTimeKind.Local).AddTicks(7423) });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "CompanyCustomer", "CompanyExecutor", "EmployeeId", "Finish", "LeaderId", "Name", "Priority", "Start" },
                values: new object[] { 3, "Customer3", "Executor3", 3, new DateTime(2021, 6, 30, 22, 20, 56, 957, DateTimeKind.Local).AddTicks(7432), 3, "SuperProject3", (byte)2, new DateTime(2021, 6, 14, 22, 20, 56, 957, DateTimeKind.Local).AddTicks(7431) });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProject_ExecutorsId",
                table: "EmployeeProject",
                column: "ExecutorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EmployeeId",
                table: "Project",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_LeaderId",
                table: "Project",
                column: "LeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeProject");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
