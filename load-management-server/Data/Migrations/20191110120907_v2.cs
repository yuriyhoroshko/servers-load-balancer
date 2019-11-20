using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "WorkerServers");

            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "WorkerServers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerServers",
                table: "WorkerServers",
                column: "WorkerServerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerServers",
                table: "WorkerServers");

            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "WorkerServers");

            migrationBuilder.RenameTable(
                name: "WorkerServers",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "WorkerServerID");
        }
    }
}
