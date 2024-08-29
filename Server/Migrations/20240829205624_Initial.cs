using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orion.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectCommunications",
                columns: table => new
                {
                    DirectCommunicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectCommunications", x => x.DirectCommunicationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DirectCommunicationUser",
                columns: table => new
                {
                    DirectCommunicationsDirectCommunicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectCommunicationUser", x => new { x.DirectCommunicationsDirectCommunicationId, x.MembersUserId });
                    table.ForeignKey(
                        name: "FK_DirectCommunicationUser_DirectCommunications_DirectCommunicationsDirectCommunicationId",
                        column: x => x.DirectCommunicationsDirectCommunicationId,
                        principalTable: "DirectCommunications",
                        principalColumn: "DirectCommunicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectCommunicationUser_Users_MembersUserId",
                        column: x => x.MembersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirectCommunicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_DirectCommunications_DirectCommunicationId",
                        column: x => x.DirectCommunicationId,
                        principalTable: "DirectCommunications",
                        principalColumn: "DirectCommunicationId");
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectCommunicationUser_MembersUserId",
                table: "DirectCommunicationUser",
                column: "MembersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DirectCommunicationId",
                table: "Messages",
                column: "DirectCommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectCommunicationUser");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "DirectCommunications");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
