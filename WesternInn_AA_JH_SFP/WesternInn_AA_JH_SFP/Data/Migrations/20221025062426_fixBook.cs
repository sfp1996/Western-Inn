using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WesternInn_AA_JH_SFP.Data.Migrations
{
    public partial class fixBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOut",
                table: "Booking",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckIn",
                table: "Booking",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOut",
                table: "Booking",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckIn",
                table: "Booking",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
