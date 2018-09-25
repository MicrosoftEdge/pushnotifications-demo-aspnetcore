using Microsoft.EntityFrameworkCore.Migrations;

namespace PushnotificationsDemo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PushSubscription",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Endpoint = table.Column<string>(nullable: false),
                    ExpirationTime = table.Column<double>(nullable: true),
                    P256Dh = table.Column<string>(nullable: false),
                    Auth = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushSubscription", x => x.P256Dh);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PushSubscription");
        }
    }
}
