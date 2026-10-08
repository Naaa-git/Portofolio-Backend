using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddI18nTranslatableFields : Migration
    {
        // Postgres won't auto-cast text/text[] -> jsonb. Since this app is still pre-launch
        // (no real user data to preserve), every column is converted via a constant USING
        // clause instead of trying to carry old values forward — the seeder repopulates
        // bilingual content right after migrations run.
        private static readonly (string Table, string Column)[] JsonbColumns =
        [
            ("Skills", "Category"),
            ("Projects", "ShortDescription"),
            ("Projects", "LongDescription"),
            ("Projects", "Category"),
            ("Profiles", "Tagline"),
            ("Profiles", "RoleAlternatives"),
            ("Profiles", "Role"),
            ("Profiles", "BioExtended"),
            ("Profiles", "Bio"),
            ("OutsideCodeIntros", "Paragraph2"),
            ("OutsideCodeIntros", "Paragraph1"),
            ("OutsideCodeBooks", "Note"),
            ("MovieTakes", "Take"),
            ("LifeInspirations", "Note"),
            ("LifeInspirations", "Aspect"),
            ("Experiences", "Role"),
            ("Experiences", "Description"),
            ("AwayFromKeyboardItems", "Title"),
            ("AwayFromKeyboardItems", "Note"),
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var (table, column) in JsonbColumns)
            {
                migrationBuilder.Sql(
                    $"ALTER TABLE \"{table}\" ALTER COLUMN \"{column}\" TYPE jsonb USING '{{}}'::jsonb;");
                migrationBuilder.Sql(
                    $"ALTER TABLE \"{table}\" ALTER COLUMN \"{column}\" SET NOT NULL;");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Skills\" ALTER COLUMN \"Category\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Projects\" ALTER COLUMN \"ShortDescription\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Projects\" ALTER COLUMN \"LongDescription\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Projects\" ALTER COLUMN \"Category\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Profiles\" ALTER COLUMN \"Tagline\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Profiles\" ALTER COLUMN \"RoleAlternatives\" TYPE text[] USING '{}'::text[];");
            migrationBuilder.Sql("ALTER TABLE \"Profiles\" ALTER COLUMN \"Role\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Profiles\" ALTER COLUMN \"BioExtended\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Profiles\" ALTER COLUMN \"Bio\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"OutsideCodeIntros\" ALTER COLUMN \"Paragraph2\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"OutsideCodeIntros\" ALTER COLUMN \"Paragraph1\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"OutsideCodeBooks\" ALTER COLUMN \"Note\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"OutsideCodeBooks\" ALTER COLUMN \"Note\" DROP NOT NULL;");
            migrationBuilder.Sql("ALTER TABLE \"MovieTakes\" ALTER COLUMN \"Take\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"LifeInspirations\" ALTER COLUMN \"Note\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"LifeInspirations\" ALTER COLUMN \"Aspect\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Experiences\" ALTER COLUMN \"Role\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"Experiences\" ALTER COLUMN \"Description\" TYPE text[] USING '{}'::text[];");
            migrationBuilder.Sql("ALTER TABLE \"AwayFromKeyboardItems\" ALTER COLUMN \"Title\" TYPE text USING '';");
            migrationBuilder.Sql("ALTER TABLE \"AwayFromKeyboardItems\" ALTER COLUMN \"Note\" TYPE text USING '';");
        }
    }
}
