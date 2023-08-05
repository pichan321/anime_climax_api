using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace anime_climax_api.Migrations
{
    /// <inheritdoc />
    public partial class aaaa1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.EnsureSchema(
            //     name: "public");

            // migrationBuilder.CreateTable(
            //     name: "Accounts",
            //     columns: table => new
            //     {
            //         ID = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            //         Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Accounts", x => x.ID);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Animes",
            //     schema: "public",
            //     columns: table => new
            //     {
            //         ID = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Name = table.Column<string>(type: "text", nullable: false),
            //         Icon = table.Column<string>(type: "text", nullable: false),
            //         Background = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Animes", x => x.ID);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Buckets",
            //     columns: table => new
            //     {
            //         ID = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         BucketName = table.Column<string>(type: "text", nullable: false),
            //         Token = table.Column<string>(type: "text", nullable: false),
            //         ShareLink = table.Column<string>(type: "text", nullable: true),
            //         Usage = table.Column<float>(type: "real", nullable: false),
            //         Capacity = table.Column<float>(type: "real", nullable: false),
            //         AccountID = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Buckets", x => x.ID);
            //         table.ForeignKey(
            //             name: "FK_Buckets_Accounts_AccountID",
            //             column: x => x.AccountID,
            //             principalTable: "Accounts",
            //             principalColumn: "ID",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Clips",
            //     schema: "public",
            //     columns: table => new
            //     {
            //         ID = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Caption = table.Column<string>(type: "text", nullable: false),
            //         Tags = table.Column<string>(type: "text", nullable: false),
            //         AnimeID = table.Column<int>(type: "integer", nullable: false),
            //         Thumbnail = table.Column<string>(type: "text", nullable: false),
            //         Link = table.Column<string>(type: "text", nullable: false),
            //         DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Clips", x => x.ID);
            //         table.ForeignKey(
            //             name: "FK_Clips_Animes_AnimeID",
            //             column: x => x.AnimeID,
            //             principalSchema: "public",
            //             principalTable: "Animes",
            //             principalColumn: "ID",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Buckets_AccountID",
            //     table: "Buckets",
            //     column: "AccountID");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clips_AnimeID",
            //     schema: "public",
            //     table: "Clips",
            //     column: "AnimeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buckets");

            migrationBuilder.DropTable(
                name: "Clips",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Animes",
                schema: "public");
        }
    }
}
