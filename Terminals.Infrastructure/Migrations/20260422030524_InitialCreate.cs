using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Terminals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CityCode = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressRegion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AddressStreet = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressHouseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AddressApartment = table.Column<int>(type: "integer", nullable: true),
                    WorkTime = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Coordinates = table.Column<string>(type: "jsonb", nullable: false),
                    Phones = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_offices_AddressCity",
                table: "offices",
                column: "AddressCity");

            migrationBuilder.CreateIndex(
                name: "IX_offices_AddressCity_AddressRegion",
                table: "offices",
                columns: new[] { "AddressCity", "AddressRegion" });

            migrationBuilder.CreateIndex(
                name: "IX_offices_Uuid",
                table: "offices",
                column: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offices");
        }
    }
}
