using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WesternInn_AA_JH_SFP.Data.Migrations
{
    public partial class updateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingGuest");

            migrationBuilder.DropTable(
                name: "BookingRoom");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_GuestEmail",
                table: "Booking",
                column: "GuestEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RoomID",
                table: "Booking",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Guest_GuestEmail",
                table: "Booking",
                column: "GuestEmail",
                principalTable: "Guest",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Room_RoomID",
                table: "Booking",
                column: "RoomID",
                principalTable: "Room",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Guest_GuestEmail",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Room_RoomID",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_GuestEmail",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_RoomID",
                table: "Booking");

            migrationBuilder.CreateTable(
                name: "BookingGuest",
                columns: table => new
                {
                    TheBookingsBookingID = table.Column<int>(type: "INTEGER", nullable: false),
                    TheGuestEmail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingGuest", x => new { x.TheBookingsBookingID, x.TheGuestEmail });
                    table.ForeignKey(
                        name: "FK_BookingGuest_Booking_TheBookingsBookingID",
                        column: x => x.TheBookingsBookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingGuest_Guest_TheGuestEmail",
                        column: x => x.TheGuestEmail,
                        principalTable: "Guest",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingRoom",
                columns: table => new
                {
                    TheBookingsBookingID = table.Column<int>(type: "INTEGER", nullable: false),
                    TheRoomID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoom", x => new { x.TheBookingsBookingID, x.TheRoomID });
                    table.ForeignKey(
                        name: "FK_BookingRoom_Booking_TheBookingsBookingID",
                        column: x => x.TheBookingsBookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRoom_Room_TheRoomID",
                        column: x => x.TheRoomID,
                        principalTable: "Room",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingGuest_TheGuestEmail",
                table: "BookingGuest",
                column: "TheGuestEmail");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoom_TheRoomID",
                table: "BookingRoom",
                column: "TheRoomID");
        }
    }
}
