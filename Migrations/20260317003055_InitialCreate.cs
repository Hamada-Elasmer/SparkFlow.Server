using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SparkFlow.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "accounts",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    game_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    next_run_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_run_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_count = table.Column<int>(type: "integer", nullable: false),
                    locked = table.Column<bool>(type: "boolean", nullable: false),
                    locked_by_session_id = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "flows",
                schema: "public",
                columns: table => new
                {
                    flow_id = table.Column<string>(type: "text", nullable: false),
                    json = table.Column<string>(type: "text", nullable: false),
                    sha256 = table.Column<string>(type: "text", nullable: false),
                    signature = table.Column<string>(type: "text", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flows", x => x.flow_id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    timestamp_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "policies",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    max_runs_per_day = table.Column<int>(type: "integer", nullable: false),
                    cooldown_minutes = table.Column<int>(type: "integer", nullable: false),
                    failure_threshold = table.Column<int>(type: "integer", nullable: false),
                    pause_duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_policies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    account_id = table.Column<string>(type: "text", nullable: false),
                    worker_id = table.Column<string>(type: "text", nullable: true),
                    flow_id = table.Column<string>(type: "text", nullable: false),
                    flow_version = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    result_type = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    assigned_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ended_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    current_session_id = table.Column<string>(type: "text", nullable: true),
                    max_concurrent_sessions = table.Column<int>(type: "integer", nullable: false),
                    last_heartbeat_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_seen_ip = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_id",
                schema: "public",
                table: "accounts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_account_id",
                schema: "public",
                table: "sessions",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_workers_id",
                schema: "public",
                table: "workers",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "flows",
                schema: "public");

            migrationBuilder.DropTable(
                name: "logs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "policies",
                schema: "public");

            migrationBuilder.DropTable(
                name: "sessions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "workers",
                schema: "public");
        }
    }
}
