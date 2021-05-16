using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatbotAPI.Migrations
{
    public partial class faqupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faq_answer_answerId",
                table: "faq");

            migrationBuilder.DropIndex(
                name: "IX_faq_answerId",
                table: "faq");

            migrationBuilder.DropColumn(
                name: "answerId",
                table: "faq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "answerId",
                table: "faq",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_faq_answerId",
                table: "faq",
                column: "answerId");

            migrationBuilder.AddForeignKey(
                name: "FK_faq_answer_answerId",
                table: "faq",
                column: "answerId",
                principalTable: "answer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
