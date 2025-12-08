using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tastings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    HostName = table.Column<string>(type: "text", nullable: false),
                    IsBlind = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentBeerIndex = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tastings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Brewery = table.Column<string>(type: "text", nullable: false),
                    Style = table.Column<string>(type: "text", nullable: false),
                    Abv = table.Column<double>(type: "double precision", nullable: false),
                    VolumeMl = table.Column<int>(type: "integer", nullable: false),
                    IsCatalogBeer = table.Column<bool>(type: "boolean", nullable: false),
                    TastingId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beers_Tastings_TastingId",
                        column: x => x.TastingId,
                        principalTable: "Tastings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TastingId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Tastings_TastingId",
                        column: x => x.TastingId,
                        principalTable: "Tastings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TastingId = table.Column<string>(type: "text", nullable: false),
                    ParticipantId = table.Column<string>(type: "text", nullable: false),
                    BeerId = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Beers_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Beers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Tastings_TastingId",
                        column: x => x.TastingId,
                        principalTable: "Tastings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Beers",
                columns: new[] { "Id", "Abv", "Brewery", "IsCatalogBeer", "Name", "Style", "TastingId", "VolumeMl" },
                values: new object[,]
                {
                    { "1", 5.2000000000000002, "Ægir Brugghús ehf.", true, "Ægir Jólalager", "Christmas Ale", null, 330 },
                    { "10", 8.5, "Corsendonk", true, "Corsendonk Christmas Ale", "Christmas Ale", null, 330 },
                    { "11", 5.5999999999999996, "Gæðingur-Öl ehf", true, "Dagsson jólabjór með bubblum", "Christmas Ale", null, 330 },
                    { "12", 6.0, "Einstök Ölgerð", true, "Einstök Winter Ale", "Winter Ale", null, 330 },
                    { "2", 7.5, "RVK Bruggfélag", true, "Ákaflega gaman þá Double IPA", "IPA", null, 440 },
                    { "3", 7.0, "Royal Unibrew A/S", true, "Albani Jule Bryg Blaa Lys", "Ale", null, 330 },
                    { "4", 10.5, "Lady Brewery ehf.", true, "Antichrist Imperial Stout", "Imperial Stout", null, 330 },
                    { "5", 5.4000000000000004, "Borg Brugghús", true, "Askasleikir nr. 45 Amber Ale", "Ale", null, 330 },
                    { "6", 6.7000000000000002, "Borg Brugghús", true, "Áttavittur nr. 116 Double Bock", "Ale", null, 330 },
                    { "7", 6.5, "Lady Brewery ehf.", true, "Be Merry Me Berry", "Ale", null, 330 },
                    { "8", 5.0, "Ölgerðin Egill Skallagrímsson", true, "Boli X Mas", "Ale", null, 330 },
                    { "9", 0.0, "Borg", true, "Brió 0%", "Non-Alcoholic", null, 330 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beers_TastingId",
                table: "Beers",
                column: "TastingId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_TastingId",
                table: "Participants",
                column: "TastingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BeerId",
                table: "Ratings",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ParticipantId",
                table: "Ratings",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TastingId_ParticipantId_BeerId",
                table: "Ratings",
                columns: new[] { "TastingId", "ParticipantId", "BeerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Beers");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Tastings");
        }
    }
}
