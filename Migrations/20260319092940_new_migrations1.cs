using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RtspViewer.Migrations
{
    /// <inheritdoc />
    public partial class new_migrations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Cameras",
                newName: "Cameras",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "Port",
                schema: "public",
                table: "Cameras",
                newName: "PollingInterval");

            migrationBuilder.RenameColumn(
                name: "MonitoringPost",
                schema: "public",
                table: "Cameras",
                newName: "WebUrl");

            migrationBuilder.AddColumn<string>(
                name: "ApiRequest",
                schema: "public",
                table: "Cameras",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FdaId",
                schema: "public",
                table: "Cameras",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationLocation",
                schema: "public",
                table: "Cameras",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonitoringPostId",
                schema: "public",
                table: "Cameras",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picket",
                schema: "public",
                table: "Cameras",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                schema: "public",
                table: "Cameras",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MonitoringPost",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    IsMobile = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringPost", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_MonitoringPostId",
                schema: "public",
                table: "Cameras",
                column: "MonitoringPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_MonitoringPost_MonitoringPostId",
                schema: "public",
                table: "Cameras",
                column: "MonitoringPostId",
                principalSchema: "public",
                principalTable: "MonitoringPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_MonitoringPost_MonitoringPostId",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropTable(
                name: "MonitoringPost",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_Cameras_MonitoringPostId",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "ApiRequest",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "FdaId",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "InstallationLocation",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "MonitoringPostId",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "Picket",
                schema: "public",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                schema: "public",
                table: "Cameras");

            migrationBuilder.RenameTable(
                name: "Cameras",
                schema: "public",
                newName: "Cameras");

            migrationBuilder.RenameColumn(
                name: "WebUrl",
                table: "Cameras",
                newName: "MonitoringPost");

            migrationBuilder.RenameColumn(
                name: "PollingInterval",
                table: "Cameras",
                newName: "Port");
        }
    }
}
