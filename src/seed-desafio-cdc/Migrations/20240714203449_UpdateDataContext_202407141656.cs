using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seed_desafio_cdc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataContext_202407141656 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update Authors set BookId = Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
