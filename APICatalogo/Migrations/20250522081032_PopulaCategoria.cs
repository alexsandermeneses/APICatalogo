using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImagemUrl)Values('Bebidas','bebida.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl)Values('Lanches','lanche.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl)Values('Sobremesa','sobremesa.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
