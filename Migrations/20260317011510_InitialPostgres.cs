using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SparkFlow.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_workers_id",
                schema: "public",
                table: "workers");

            migrationBuilder.DropIndex(
                name: "IX_accounts_id",
                schema: "public",
                table: "accounts");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "public",
                table: "workers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "public",
                table: "sessions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "result_type",
                schema: "public",
                table: "sessions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "public",
                table: "logs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "public",
                table: "accounts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_status",
                schema: "public",
                table: "sessions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_game_id",
                schema: "public",
                table: "accounts",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_next_run_at_utc",
                schema: "public",
                table: "accounts",
                column: "next_run_at_utc");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_status",
                schema: "public",
                table: "accounts",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sessions_status",
                schema: "public",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "IX_accounts_game_id",
                schema: "public",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_next_run_at_utc",
                schema: "public",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_status",
                schema: "public",
                table: "accounts");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                schema: "public",
                table: "workers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                schema: "public",
                table: "sessions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "result_type",
                schema: "public",
                table: "sessions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "public",
                table: "logs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                schema: "public",
                table: "accounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_workers_id",
                schema: "public",
                table: "workers",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_id",
                schema: "public",
                table: "accounts",
                column: "id",
                unique: true);
        }
    }
}
