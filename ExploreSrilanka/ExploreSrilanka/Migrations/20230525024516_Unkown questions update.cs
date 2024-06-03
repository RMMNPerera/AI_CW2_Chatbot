using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreSrilanka.Migrations
{
    public partial class Unkownquestionsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "UnkownQuestions",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "UnkownQuestions");
        }
    }
}
