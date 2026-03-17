using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparkFlow.Server.Migrations
{
    /// <inheritdoc />
    public partial class MigrateSessionsToPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_sessions_created_at_utc",
                schema: "public",
                table: "sessions",
                column: "created_at_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sessions_created_at_utc",
                schema: "public",
                table: "sessions");
        }
    }
}
