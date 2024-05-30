using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODSphereRouter.Migrations
{
    /// <inheritdoc />
    public partial class RouteList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouteList",
                columns: table => new
                {
                    Address = table.Column<long>(type: "INTEGER", nullable: false),
                    System = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteList", x => x.Address);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteList");
        }
    }
}
