using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetAuction.DAL.MSSQL.Migrations
{
    public partial class CorrectNameing1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutctionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutctionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Autctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Autctions_AutctionStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AutctionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Biddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AutctionId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biddings_Autctions_AutctionId",
                        column: x => x.AutctionId,
                        principalTable: "Autctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostMin = table.Column<decimal>(type: "decimal", nullable: false),
                    PhotoCurrentId = table.Column<int>(type: "int", nullable: true),
                    AutctionRef = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_Autctions_AutctionRef",
                        column: x => x.AutctionRef,
                        principalTable: "Autctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lots_LotCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "LotCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarCurrentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageIds_Lots_LotId",
                        column: x => x.LotId,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImageIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AutctionStatuses",
                columns: new[] { "Id", "DescriptionStatus", "NameStatus" },
                values: new object[,]
                {
                    { 1, "Auction is started", "Start" },
                    { 2, "Auction is finished", "Finish" },
                    { 3, "Auction isn't started", "Is not started" }
                });

            migrationBuilder.InsertData(
                table: "LotCategories",
                columns: new[] { "Id", "DescriptionCategory", "NameCategory" },
                values: new object[,]
                {
                    { 1, "The arts are a very wide range of human practices of creative expression, storytelling and cultural participation. They encompass multiple diverse and plural modes of thinking, doing and being, in an extremely broad range of media. Both highly dynamic and a characteristically constant feature of human life, they have developed into innovative, stylized and sometimes intricate forms.", "Arts" },
                    { 2, "A book is a medium for recording information in the form of writing or images, typically composed of many pages (made of papyrus, parchment, vellum, or paper) bound together and protected by a cover.[1] The technical term for this physical arrangement is codex (plural, codices). In the history of hand-held physical supports for extended written compositions or records, the codex replaces its predecessor, the scroll. A single sheet in a codex is a leaf and each side of a leaf is a page.", "Books" },
                    { 3, "A true antique (Latin: antiquus; 'old', 'ancient') is an item perceived as having value because of its aesthetic or historical significance, and often defined as at least 100 years old (or some other limit), although the term is often used loosely to describe any object that is old.[1] An antique is usually an item that is collected or desirable because of its age, beauty, rarity, condition, utility, personal emotional connection, and/or other unique features. It is an object that represents a previous era or time period in human history. Vintage and collectible are used to describe items that are old, but do not meet the 100-year criterion.", "Antiques" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "1", "Customer" },
                    { "2", "Owner" },
                    { "3", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autctions_StatusId",
                table: "Autctions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_AutctionId",
                table: "Biddings",
                column: "AutctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_UserId",
                table: "Biddings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageIds_LotId",
                table: "ImageIds",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageIds_UserId",
                table: "ImageIds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_AutctionRef",
                table: "Lots",
                column: "AutctionRef",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_AuthorId",
                table: "Lots",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_CategoryId",
                table: "Lots",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_PhotoCurrentId",
                table: "Lots",
                column: "PhotoCurrentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarCurrentId",
                table: "Users",
                column: "AvatarCurrentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Biddings_Users_UserId",
                table: "Biddings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_ImageIds_PhotoCurrentId",
                table: "Lots",
                column: "PhotoCurrentId",
                principalTable: "ImageIds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Users_AuthorId",
                table: "Lots",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ImageIds_AvatarCurrentId",
                table: "Users",
                column: "AvatarCurrentId",
                principalTable: "ImageIds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autctions_AutctionStatuses_StatusId",
                table: "Autctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Autctions_AutctionRef",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageIds_Users_UserId",
                table: "ImageIds");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Users_AuthorId",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageIds_Lots_LotId",
                table: "ImageIds");

            migrationBuilder.DropTable(
                name: "Biddings");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "AutctionStatuses");

            migrationBuilder.DropTable(
                name: "Autctions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "ImageIds");

            migrationBuilder.DropTable(
                name: "LotCategories");
        }
    }
}
