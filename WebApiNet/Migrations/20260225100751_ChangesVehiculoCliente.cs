using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiNet.Migrations
{
    /// <inheritdoc />
    public partial class ChangesVehiculoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alquiler_Cliente_ClienteDni",
                table: "Alquiler");

            migrationBuilder.DropForeignKey(
                name: "FK_Alquiler_Vehiculo_VehiculoMatricula",
                table: "Alquiler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculo",
                table: "Vehiculo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alquiler",
                table: "Alquiler");

            migrationBuilder.RenameTable(
                name: "Vehiculo",
                newName: "Vehiculos");

            migrationBuilder.RenameTable(
                name: "Cliente",
                newName: "Clientes");

            migrationBuilder.RenameTable(
                name: "Alquiler",
                newName: "Alquileres");

            migrationBuilder.RenameIndex(
                name: "IX_Alquiler_VehiculoMatricula",
                table: "Alquileres",
                newName: "IX_Alquileres_VehiculoMatricula");

            migrationBuilder.RenameIndex(
                name: "IX_Alquiler_ClienteDni",
                table: "Alquileres",
                newName: "IX_Alquileres_ClienteDni");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Clientes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Clientes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Clientes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Clientes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Clientes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Clientes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Clientes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Clientes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos",
                column: "Matricula");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Dni");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alquileres",
                table: "Alquileres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alquileres_Clientes_ClienteDni",
                table: "Alquileres",
                column: "ClienteDni",
                principalTable: "Clientes",
                principalColumn: "Dni",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Alquileres_Vehiculos_VehiculoMatricula",
                table: "Alquileres",
                column: "VehiculoMatricula",
                principalTable: "Vehiculos",
                principalColumn: "Matricula",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alquileres_Clientes_ClienteDni",
                table: "Alquileres");

            migrationBuilder.DropForeignKey(
                name: "FK_Alquileres_Vehiculos_VehiculoMatricula",
                table: "Alquileres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alquileres",
                table: "Alquileres");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Vehiculos",
                newName: "Vehiculo");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "Cliente");

            migrationBuilder.RenameTable(
                name: "Alquileres",
                newName: "Alquiler");

            migrationBuilder.RenameIndex(
                name: "IX_Alquileres_VehiculoMatricula",
                table: "Alquiler",
                newName: "IX_Alquiler_VehiculoMatricula");

            migrationBuilder.RenameIndex(
                name: "IX_Alquileres_ClienteDni",
                table: "Alquiler",
                newName: "IX_Alquiler_ClienteDni");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculo",
                table: "Vehiculo",
                column: "Matricula");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente",
                column: "Dni");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alquiler",
                table: "Alquiler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alquiler_Cliente_ClienteDni",
                table: "Alquiler",
                column: "ClienteDni",
                principalTable: "Cliente",
                principalColumn: "Dni",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alquiler_Vehiculo_VehiculoMatricula",
                table: "Alquiler",
                column: "VehiculoMatricula",
                principalTable: "Vehiculo",
                principalColumn: "Matricula",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
