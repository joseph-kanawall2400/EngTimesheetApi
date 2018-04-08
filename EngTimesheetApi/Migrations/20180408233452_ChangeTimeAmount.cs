using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EngTimesheet.Migrations
{
    public partial class ChangeTimeAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Times");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Times",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Times");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Times",
                nullable: false,
                defaultValue: 0);
        }
    }
}
