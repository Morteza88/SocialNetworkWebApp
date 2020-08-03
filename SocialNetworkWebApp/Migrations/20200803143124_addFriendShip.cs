using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetworkWebApp.Migrations
{
    public partial class addFriendShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("085b9b23-72b0-4e56-b965-be9aaf2cdcb9"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("3987f970-a4e2-4635-babd-00f66fce70a0"), new Guid("5c2eceeb-8aa4-4b97-b612-9f81ea227cbe") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5c2eceeb-8aa4-4b97-b612-9f81ea227cbe"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3987f970-a4e2-4635-babd-00f66fce70a0"));

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FromUserId = table.Column<Guid>(nullable: false),
                    ToUserId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.FromUserId, x.ToUserId });
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("ae1f5b64-f25d-4751-82dc-d7a2648c4edb"), "5a5d17af-18f3-487b-8c6b-65f37e074026", "Admin role", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("a2312f01-1c53-4995-bc01-4435658be33e"), "2879eaf4-a513-4ec0-b9e5-b3a2da902835", "User role", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("f5fec375-3110-4a59-9178-995ce98ae875"), 0, "352a3598-7edf-40f5-967a-b1fd1e00886b", "admin@email.com", true, "Administrator", true, false, null, "admin@email.com", "admin", "AQAAAAEAACcQAAAAEH6yZK+YQ7z12uTkR7rJ6sZuEd1KIJ3TrrU/yxc9fJkQoLUpXPzP2ERGGA//nKkojg==", null, false, "170dc09e-3aa6-4160-9bee-5e67085a53e0", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("f5fec375-3110-4a59-9178-995ce98ae875"), new Guid("ae1f5b64-f25d-4751-82dc-d7a2648c4edb") });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ToUserId",
                table: "Friendships",
                column: "ToUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a2312f01-1c53-4995-bc01-4435658be33e"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("f5fec375-3110-4a59-9178-995ce98ae875"), new Guid("ae1f5b64-f25d-4751-82dc-d7a2648c4edb") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ae1f5b64-f25d-4751-82dc-d7a2648c4edb"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f5fec375-3110-4a59-9178-995ce98ae875"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("5c2eceeb-8aa4-4b97-b612-9f81ea227cbe"), "0acc1b7b-c60f-4f90-95f8-63187db5012d", "Admin role", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("085b9b23-72b0-4e56-b965-be9aaf2cdcb9"), "4aeea832-e6f8-468f-9e9d-8ba3d04bad35", "User role", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("3987f970-a4e2-4635-babd-00f66fce70a0"), 0, "01ff4109-29ad-42a0-b6d4-ec29b3e131db", "admin@email.com", true, "Administrator", true, false, null, "admin@email.com", "admin", "AQAAAAEAACcQAAAAEMKxivD+9jf/yLcHD9KfuNsZO9J9HcJVXFMnR8z9g6OSZBaRUa9e9+pJlTzT6e6mHw==", null, false, "79a13267-3a6b-4b21-85c3-d2563b236a98", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("3987f970-a4e2-4635-babd-00f66fce70a0"), new Guid("5c2eceeb-8aa4-4b97-b612-9f81ea227cbe") });
        }
    }
}
