using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListingImages_ListingId",
                table: "ListingImages");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Listings",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Listings",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Listings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Listings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Listings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MainImageId",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Listings",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Listings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Listings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ListingImages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ListingImages",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ListingImages",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ListingImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ListingImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ListingImages",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ListingImages",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ListingImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedAt",
                table: "Listings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_IsPromoted",
                table: "Listings",
                column: "IsPromoted");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Location",
                table: "Listings",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_MainImageId",
                table: "Listings",
                column: "MainImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_PricePerNight",
                table: "Listings",
                column: "PricePerNight");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserId_IsDeleted",
                table: "Listings",
                columns: new[] { "UserId", "IsDeleted" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_Latitude",
                table: "Listings",
                sql: "[Latitude] >= -90 AND [Latitude] <= 90");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_Longitude",
                table: "Listings",
                sql: "[Longitude] >= -180 AND [Longitude] <= 180");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_MaxGuests",
                table: "Listings",
                sql: "[MaxGuests] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_Price",
                table: "Listings",
                sql: "[PricePerNight] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ListingId_IsDeleted",
                table: "ListingImages",
                columns: new[] { "ListingId", "IsDeleted" });

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ListingImages_MainImageId",
                table: "Listings",
                column: "MainImageId",
                principalTable: "ListingImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingImages_MainImageId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CreatedAt",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_IsPromoted",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Location",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_MainImageId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_PricePerNight",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_UserId_IsDeleted",
                table: "Listings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_Latitude",
                table: "Listings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_Longitude",
                table: "Listings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_MaxGuests",
                table: "Listings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_Price",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_ListingImages_ListingId_IsDeleted",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ListingImages");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Listings",
                type: "decimal(9,6)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Listings",
                type: "decimal(9,6)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ListingId",
                table: "ListingImages",
                column: "ListingId");
        }
    }
}
