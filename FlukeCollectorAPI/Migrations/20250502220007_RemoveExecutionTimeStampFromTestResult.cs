﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlukeCollectorAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExecutionTimeStampFromTestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionTimeStamp",
                table: "TestResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutionTimeStamp",
                table: "TestResults",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
