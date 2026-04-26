using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorMembershipStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Удаляем индекс на MembershipStatus (если он существует)
            migrationBuilder.DropIndex(
                name: "IX_Memberships_Status",
                table: "Memberships");

            // 2. Добавляем новые столбцы
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FrozenDate",
                table: "Memberships",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FreezeDaysUsed",
                table: "Memberships",
                type: "int",
                nullable: true);

            // 3. Перенос данных
            migrationBuilder.Sql(@"
                UPDATE Memberships 
                SET Status = CASE MembershipStatus 
                    WHEN 'Active' THEN 1 
                    WHEN 'Frozen' THEN 2 
                    WHEN 'Completed' THEN 3 
                    WHEN 'Canceled' THEN 4 
                    ELSE 1 
                END
            ");

            // 4. Удаляем старый столбец
            migrationBuilder.DropColumn(
                name: "MembershipStatus",
                table: "Memberships");

            // 5. Создаём новый индекс на столбце Status (если нужен)
            //    В конфигурации EF у нас уже есть HasIndex(m => m.Status), 
            //    но можно создать явно:
            // migrationBuilder.CreateIndex(
            //     name: "IX_Memberships_Status",
            //     table: "Memberships",
            //     column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаляем новый индекс, если был создан
            migrationBuilder.DropIndex(name: "IX_Memberships_Status", table: "Memberships");

            migrationBuilder.AddColumn<string>(
                name: "MembershipStatus",
                table: "Memberships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.Sql(@"
                UPDATE Memberships 
                SET MembershipStatus = CASE Status 
                    WHEN 1 THEN 'Active' 
                    WHEN 2 THEN 'Frozen' 
                    WHEN 3 THEN 'Completed' 
                    WHEN 4 THEN 'Canceled' 
                    ELSE 'Active' 
                END
            ");

            // Восстанавливаем индекс на MembershipStatus
            migrationBuilder.CreateIndex(
                name: "IX_Memberships_Status",
                table: "Memberships",
                column: "MembershipStatus");

            migrationBuilder.DropColumn(name: "Status", table: "Memberships");
            migrationBuilder.DropColumn(name: "FrozenDate", table: "Memberships");
            migrationBuilder.DropColumn(name: "FreezeDaysUsed", table: "Memberships");
        }
    }
}
