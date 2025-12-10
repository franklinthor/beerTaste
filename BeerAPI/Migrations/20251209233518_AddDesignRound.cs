using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignRound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DesignRoundEnabled",
                table: "Tastings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DesignRoundStarted",
                table: "Tastings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DesignRankings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TastingId = table.Column<string>(type: "text", nullable: false),
                    ParticipantId = table.Column<string>(type: "text", nullable: false),
                    RankingsJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignRankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignRankings_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignRankings_Tastings_TastingId",
                        column: x => x.TastingId,
                        principalTable: "Tastings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesignRankings_ParticipantId",
                table: "DesignRankings",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignRankings_TastingId_ParticipantId",
                table: "DesignRankings",
                columns: new[] { "TastingId", "ParticipantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesignRankings");

            migrationBuilder.DropColumn(
                name: "DesignRoundEnabled",
                table: "Tastings");

            migrationBuilder.DropColumn(
                name: "DesignRoundStarted",
                table: "Tastings");
        }
    }
}
