using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExperienceDatesDescriptionAndProjectLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Experiences",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Experiences",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            // Parse "Mon YYYY – Mon YYYY" / "Mon YYYY – Present" into real dates
            // while Period still exists. Falls back to 2026-01-01 for anything
            // that doesn't match the expected format (confirmed safe default
            // with the project owner) — see docs/migrations/ for the writeup.
            migrationBuilder.Sql(@"
                UPDATE ""Experiences"" SET
                  ""StartDate"" = CASE WHEN split_part(""Period"", ' – ', 1) ~ '^[A-Za-z]{3} \d{4}$'
                                   THEN to_date(split_part(""Period"", ' – ', 1), 'Mon YYYY') ELSE '2026-01-01' END,
                  ""EndDate"" = CASE WHEN ""Current"" THEN NULL
                                 WHEN split_part(""Period"", ' – ', 2) ~ '^[A-Za-z]{3} \d{4}$'
                                   THEN to_date(split_part(""Period"", ' – ', 2), 'Mon YYYY') ELSE '2026-01-01' END;
            ");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "Experiences");

            // Description: array-of-bullets jsonb -> single bilingual-object jsonb.
            // The Postgres column type stays "jsonb" either way, so EF's own model
            // diff didn't flag this as a change — handled by hand. Merges old
            // bullets (newline-joined per language) instead of discarding them.
            migrationBuilder.Sql(@"
                UPDATE ""Experiences""
                SET ""Description"" = jsonb_build_object(
                  'id', (SELECT string_agg(elem->>'id', E'\n') FROM jsonb_array_elements(""Description"") elem),
                  'en', (SELECT string_agg(elem->>'en', E'\n') FROM jsonb_array_elements(""Description"") elem)
                );
            ");

            migrationBuilder.AddColumn<int>(
                name: "ExperienceId",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ExperienceId",
                table: "Projects",
                column: "ExperienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Experiences_ExperienceId",
                table: "Projects",
                column: "ExperienceId",
                principalTable: "Experiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Experiences_ExperienceId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ExperienceId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ExperienceId",
                table: "Projects");

            // Best-effort only — wraps the merged text back into a 1-element
            // array; original bullet boundaries are not recoverable either way.
            migrationBuilder.Sql(@"
                UPDATE ""Experiences"" SET ""Description"" = jsonb_build_array(""Description"");
            ");

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "Experiences",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Best-effort reconstruction — exact original wording isn't
            // recoverable, just a parseable equivalent of the same shape.
            migrationBuilder.Sql(@"
                UPDATE ""Experiences"" SET ""Period"" =
                  to_char(""StartDate"", 'Mon YYYY') || ' – ' ||
                  CASE WHEN ""Current"" THEN 'Present' ELSE to_char(""EndDate"", 'Mon YYYY') END;
            ");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Experiences");
        }
    }
}
