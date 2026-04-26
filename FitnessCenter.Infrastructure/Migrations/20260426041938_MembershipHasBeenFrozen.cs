using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MembershipHasBeenFrozen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenFrozen",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenFrozen",
                table: "Memberships");
        }
    }
}
