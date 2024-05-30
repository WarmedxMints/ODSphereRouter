using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODSphereRouter.Migrations
{
    /// <inheritdoc />
    public partial class StarSystems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarSystems",
                columns: table => new
                {
                    Address = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Population = table.Column<long>(type: "INTEGER", nullable: false),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Z = table.Column<double>(type: "REAL", nullable: false),
                    Powers = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerState = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarSystems", x => x.Address);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarSystems");
        }
    }
}
