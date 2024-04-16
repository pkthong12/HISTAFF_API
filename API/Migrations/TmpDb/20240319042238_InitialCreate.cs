using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations.TmpDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_MUTATION_LOG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SYS_FUNCTION_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SYS_ACTION_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BEFORE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BEFORE1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BEFORE2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BEFORE3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AFTER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AFTER1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AFTER2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AFTER3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CREATED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MUTATION_LOG", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_MUTATION_LOG");
        }
    }
}
