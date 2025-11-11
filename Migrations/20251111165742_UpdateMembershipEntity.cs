using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ispk.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembershipEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "membershipTypeId",
                table: "Membership",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MembershipType",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipType", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Membership_membershipTypeId",
                table: "Membership",
                column: "membershipTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_MembershipType_membershipTypeId",
                table: "Membership",
                column: "membershipTypeId",
                principalTable: "MembershipType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membership_MembershipType_membershipTypeId",
                table: "Membership");

            migrationBuilder.DropTable(
                name: "MembershipType");

            migrationBuilder.DropIndex(
                name: "IX_Membership_membershipTypeId",
                table: "Membership");

            migrationBuilder.DropColumn(
                name: "membershipTypeId",
                table: "Membership");
        }
    }
}
