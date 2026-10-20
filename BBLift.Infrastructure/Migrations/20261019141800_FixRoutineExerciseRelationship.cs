using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBLift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRoutineExerciseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineExercises_Routines_RoutineId1",
                table: "RoutineExercises");

            migrationBuilder.DropIndex(
                name: "IX_RoutineExercises_RoutineId1",
                table: "RoutineExercises");

            migrationBuilder.DropColumn(
                name: "RoutineId1",
                table: "RoutineExercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoutineId1",
                table: "RoutineExercises",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoutineExercises_RoutineId1",
                table: "RoutineExercises",
                column: "RoutineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineExercises_Routines_RoutineId1",
                table: "RoutineExercises",
                column: "RoutineId1",
                principalTable: "Routines",
                principalColumn: "Id");
        }
    }
}
