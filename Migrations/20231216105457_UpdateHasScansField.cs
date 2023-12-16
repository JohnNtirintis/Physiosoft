using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Physiosoft.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHasScansField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PATIENTS",
                columns: table => new
                {
                    patient_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    vat = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    ssn = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    reg_num = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    has_reviewed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    patient_issue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PATIENTS__4D5CE4769C9C68C2", x => x.patient_id);
                });

            migrationBuilder.CreateTable(
                name: "PHYSIOS",
                columns: table => new
                {
                    physio_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHYSIOS__8BB9145E55AAA5F5", x => x.physio_id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_admin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__USERS", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "APPOINTMENTS",
                columns: table => new
                {
                    appointment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    physio_id = table.Column<int>(type: "int", nullable: true),
                    appointment_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    appointment_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    patient_issue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    has_scans = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__APPOINTMENT", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients",
                        column: x => x.patient_id,
                        principalTable: "PATIENTS",
                        principalColumn: "patient_id");
                    table.ForeignKey(
                        name: "FK_Appointments_Physios",
                        column: x => x.physio_id,
                        principalTable: "PHYSIOS",
                        principalColumn: "physio_id");
                });

            migrationBuilder.CreateIndex(
                name: "appointment_date",
                table: "APPOINTMENTS",
                column: "appointment_date");

            migrationBuilder.CreateIndex(
                name: "IX_APPOINTMENTS_physio_id",
                table: "APPOINTMENTS",
                column: "physio_id");

            migrationBuilder.CreateIndex(
                name: "patient_id",
                table: "APPOINTMENTS",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_PATIENTS_LASTNAME",
                table: "PATIENTS",
                column: "lastname");

            migrationBuilder.CreateIndex(
                name: "UQ_PATIENTS_SSN",
                table: "PATIENTS",
                column: "ssn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PHYSIOS_Lastname",
                table: "PHYSIOS",
                column: "lastname");

            migrationBuilder.CreateIndex(
                name: "UQ_PHYSIOS_TELEPHONE",
                table: "PHYSIOS",
                column: "telephone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email",
                table: "USERS",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "username",
                table: "USERS",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPOINTMENTS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "PATIENTS");

            migrationBuilder.DropTable(
                name: "PHYSIOS");
        }
    }
}
