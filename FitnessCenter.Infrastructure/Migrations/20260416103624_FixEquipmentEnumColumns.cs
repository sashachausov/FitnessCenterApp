using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixEquipmentEnumColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("TempEquipmentType", table: "Equipment", nullable: false, defaultValue: 1);
            migrationBuilder.AddColumn<int>("TempManufacturer", table: "Equipment", nullable: false, defaultValue: 1);
            migrationBuilder.AddColumn<int>("TempEquipmentStatus", table: "Equipment", nullable: false, defaultValue: 1);

            // Конвертация данных (замените строки на соответствующие числовые значения вашего enum)
            // Для EquipmentType
            migrationBuilder.Sql("UPDATE Equipment SET TempEquipmentType = CASE EquipmentType " +
                "WHEN 'Cardio' THEN 1 " +
                "WHEN 'Strength' THEN 2 " +
                "WHEN 'FreeWeights' THEN 3 " +
                "WHEN 'Functional' THEN 4 " +
                "WHEN 'Accessories' THEN 5 ELSE 1 END");

            // Для Manufacturer (подставьте ваши значения)
            migrationBuilder.Sql("UPDATE Equipment SET TempManufacturer = CASE Manufacturer " +
                "WHEN 'Technogym' THEN 1 " +
                "WHEN 'Life Fitness' THEN 2 " +
                "WHEN 'Precor' THEN 3 " +
                "WHEN 'Matrix' THEN 4 " +
                "WHEN 'Hammer Strength' THEN 5 " +
                "WHEN 'Eleiko' THEN 6 " +
                "WHEN 'Другое' THEN 99 ELSE 1 END");

            // Для EquipmentStatus
            migrationBuilder.Sql("UPDATE Equipment SET TempEquipmentStatus = CASE EquipmentStatus " +
                "WHEN 'Good' THEN 1 " +
                "WHEN 'NeedsMaintenance' THEN 2 " +
                "WHEN 'UnderRepair' THEN 3 " +
                "WHEN 'Decommissioned' THEN 4 ELSE 1 END");

            // Удаляем старые столбцы
            migrationBuilder.DropColumn(name: "EquipmentType", table: "Equipment");
            migrationBuilder.DropColumn(name: "Manufacturer", table: "Equipment");
            migrationBuilder.DropColumn(name: "EquipmentStatus", table: "Equipment");

            // Переименовываем временные в исходные
            migrationBuilder.RenameColumn(name: "TempEquipmentType", table: "Equipment", newName: "EquipmentType");
            migrationBuilder.RenameColumn(name: "TempManufacturer", table: "Equipment", newName: "Manufacturer");
            migrationBuilder.RenameColumn(name: "TempEquipmentStatus", table: "Equipment", newName: "EquipmentStatus");

            //migrationBuilder.DropColumn(
            //    name: "CreatedAt",
            //    table: "Halls");

            //migrationBuilder.DropColumn(
            //    name: "UpdatedAt",
            //    table: "Halls");

            //migrationBuilder.DropColumn(
            //    name: "CreatedAt",
            //    table: "Equipment");

            //migrationBuilder.DropColumn(
            //    name: "UpdatedAt",
            //    table: "Equipment");

            //migrationBuilder.RenameColumn(
            //    name: "Id",
            //    table: "Halls",
            //    newName: "HallID");

            //migrationBuilder.RenameColumn(
            //    name: "Id",
            //    table: "Equipment",
            //    newName: "EquipmentID");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Manufacturer",
            //    table: "Equipment",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "EquipmentType",
            //    table: "Equipment",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "EquipmentStatus",
            //    table: "Equipment",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 1,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldMaxLength: 50,
            //    oldDefaultValue: "Good");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HallID",
                table: "Halls",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EquipmentID",
                table: "Equipment",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Halls",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Halls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Equipment",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EquipmentType",
                table: "Equipment",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EquipmentStatus",
                table: "Equipment",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Good",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Equipment",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Equipment",
                type: "datetime2",
                nullable: true);
        }
    }
}
