using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProjectLongDescriptionToContribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LongDescription",
                table: "Projects",
                newName: "Contribution");

            // Column was a single translatable object ({"id":"...","en":"..."});
            // Contribution is a translatable list ([{"id":"...","en":"..."}, ...]).
            // Same jsonb column type either way, so EF's diff only caught the
            // rename — wrap the existing object as the list's first item here.
            migrationBuilder.Sql(
                """
                UPDATE "Projects" SET "Contribution" =
                    CASE WHEN "Contribution" = '{}'::jsonb THEN '[]'::jsonb
                         ELSE jsonb_build_array("Contribution") END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Best-effort: only the first item survives. Any 2nd/3rd item an
            // admin added after this migration shipped is lost on rollback.
            migrationBuilder.Sql(
                """
                UPDATE "Projects" SET "Contribution" = COALESCE("Contribution"->0, '{}'::jsonb);
                """);

            migrationBuilder.RenameColumn(
                name: "Contribution",
                table: "Projects",
                newName: "LongDescription");
        }
    }
}
