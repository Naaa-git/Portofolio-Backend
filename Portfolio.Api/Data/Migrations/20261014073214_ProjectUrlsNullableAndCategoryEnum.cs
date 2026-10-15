using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProjectUrlsNullableAndCategoryEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GithubUrl",
                table: "Projects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DemoUrl",
                table: "Projects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            // Category goes from free-text jsonb (e.g. {"id":"Enterprise","en":"Enterprise"})
            // to a fixed ProjectCategory enum (int: Personal=0, Freelance=1, Work=2). The old
            // text can't be reliably mapped onto the 3 new values, so every existing row is
            // set to Work (2) — confirmed as the safe default with the project owner; existing
            // rows are re-categorized manually afterward via the admin dropdown. See
            // docs/migrations/ for the full writeup and the backup taken before this runs in prod.
            migrationBuilder.Sql(
                "ALTER TABLE \"Projects\" ALTER COLUMN \"Category\" TYPE integer USING 2;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GithubUrl",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DemoUrl",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            // Best-effort only: the original free-text category values are not recoverable
            // either way (this migration's Up() already discarded them), so Down() just
            // restores the column's jsonb shape as an empty object, not the original content.
            migrationBuilder.Sql(
                "ALTER TABLE \"Projects\" ALTER COLUMN \"Category\" TYPE jsonb USING '{}'::jsonb;");
        }
    }
}
