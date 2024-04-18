using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary1.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, computedColumnSql: "CASE WHEN \"FirstName\" IS NULL THEN \"LastName\" WHEN \"LastName\"  IS NULL THEN \"FirstName\" ELSE \"FirstName\" || ' ' || \"LastName\" END", stored: true),
                    Username = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    PersonalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    Country = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Language = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    LastCompany = table.Column<Guid>(type: "uuid", nullable: true),
                    Settings = table.Column<string>(type: "text", nullable: true),
                    BetaRole = table.Column<bool>(type: "boolean", nullable: true),
                    EmailLastValidated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PersonalCodeLastValidated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserNotes = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    ExternalId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserGuid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
