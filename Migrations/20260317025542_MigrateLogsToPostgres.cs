using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparkFlow.Server.Migrations
{
    /// <inheritdoc />
    public partial class MigrateLogsToPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "device_id",
                schema: "public",
                table: "logs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_logs_device_id",
                schema: "public",
                table: "logs",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_logs_timestamp_utc",
                schema: "public",
                table: "logs",
                column: "timestamp_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_logs_device_id",
                schema: "public",
                table: "logs");

            migrationBuilder.DropIndex(
                name: "IX_logs_timestamp_utc",
                schema: "public",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "device_id",
                schema: "public",
                table: "logs");
        }
    }
}
