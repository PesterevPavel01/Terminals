using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Terminals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CityCode = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    Type = table.Column<int>(type: "integer", maxLength: 20, nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressRegion = table.Column<string>(type: "text", nullable: true),
                    AddressCity = table.Column<string>(type: "text", nullable: true),
                    AddressStreet = table.Column<string>(type: "text", nullable: true),
                    AddressHouseNumber = table.Column<string>(type: "text", nullable: true),
                    AddressApartment = table.Column<int>(type: "integer", nullable: true),
                    WorkTime = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: false),
                    Coordinates = table.Column<string>(type: "jsonb", nullable: false),
                    Phones = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Office_AddressCity",
                table: "Office",
                column: "AddressCity");

            migrationBuilder.CreateIndex(
                name: "IX_Office_Uuid",
                table: "Office",
                column: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Office");
        }
    }
}
