using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetworkWebApp.Migrations
{
    public partial class AutoCreateAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Friendships",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("41cf977f-936e-4cef-af39-7a1d8fc2c502"), "503421d6-5821-4844-ba5c-c47b87a52132", "Admin role", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("043dcc82-f0db-43a9-8cd7-0c8c85f39a68"), "d0245bc1-145a-4793-8691-c9a4cf48e4fa", "User role", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("8e15f3c7-4e5a-498a-8286-b0ed0efe9bbb"), 0, "dc62e94a-6e4f-4159-8c26-f49137f06d32", "admin@email.com", true, "Administrator", true, false, null, "admin@email.com", "admin", "AQAAAAEAACcQAAAAEO8TVrleA914YftkqGaFjXKJhlutyEqIe1H+MBEljoU6wvXe4lK0eBHqmz1l8Qc4Vg==", null, false, "4f22629b-b8fd-4ab9-94b1-e18afa4b7b19", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("8e15f3c7-4e5a-498a-8286-b0ed0efe9bbb"), new Guid("41cf977f-936e-4cef-af39-7a1d8fc2c502") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("043dcc82-f0db-43a9-8cd7-0c8c85f39a68"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("8e15f3c7-4e5a-498a-8286-b0ed0efe9bbb"), new Guid("41cf977f-936e-4cef-af39-7a1d8fc2c502") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("41cf977f-936e-4cef-af39-7a1d8fc2c502"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8e15f3c7-4e5a-498a-8286-b0ed0efe9bbb"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Friendships",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

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
        }
    }
}
