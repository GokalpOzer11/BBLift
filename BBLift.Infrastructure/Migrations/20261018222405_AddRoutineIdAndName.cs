using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBLift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutineIdAndName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WorkoutSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoutineId",
                table: "WorkoutSessions",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "RoutineId",
                table: "WorkoutSessions");
        }
    }
}
