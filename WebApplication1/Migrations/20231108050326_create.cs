using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceTrip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    PriceLow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTrip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    passWord = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imageCCCD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imageBike = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    licensePlates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: true),
                    lockOutEndDateUtc = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    startLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    distance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    time = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    timeBook = table.Column<DateTime>(type: "datetime2", nullable: true),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isPaid = table.Column<bool>(type: "bit", nullable: true),
                    locationIP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trip_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trip_UserId",
                table: "Trip",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_roleId",
                table: "UserRole",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_userId",
                table: "UserRole",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceTrip");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
