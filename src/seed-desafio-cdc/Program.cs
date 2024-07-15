using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using seed_desafio_cdc;
using seed_desafio_cdc.Context;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.CustomSchemaIds(s => s.ToString());

    var path = AppContext.BaseDirectory;

    foreach (var name in Directory.GetFiles(path, "*.xml"))
    {
        setup.IncludeXmlComments(filePath: name);
    }
});

builder.Services.AddDbContext<DataContext>(option =>
{
    // option.UseInMemoryDatabase("DesafioDB");
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<BookService>();

builder.Services.AddScoped<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/categories", async ([FromBody] CategoryDTO request, CategoryService service, CancellationToken token) =>
{
    try
    {
        await service.RegisterCategoryAsync(request, token);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }

    return TypedResults.Created();
})
.WithName("PostCategories")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Realiza o cadastro de categoria",
    Description = "Use esta API para realizar o cadastramento de uma categoria. Todos os detalhes devem ser passados no corpo da requisição."
})
.Produces((int)HttpStatusCode.Created)
.Produces((int)HttpStatusCode.BadRequest);

app.MapPost("api/authors", async ([FromBody] AuthorDTO request, AuthorService service, CancellationToken token) =>
{
    try
    {
        await service.RegisterAuthorAsync(request, token);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }

    return TypedResults.Created();
})
.WithName("PostAuthors")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Realiza o cadastro do autor",
    Description = "Use esta API para realizar o cadastramento do autor. Todos os detalhes devem ser passados no corpo da requisição."
})
.Produces((int)HttpStatusCode.Created)
.Produces((int)HttpStatusCode.BadRequest);

app.MapGet("api/authors", async (DataContext context) =>
{
    var result = await context.Authors.Select(author => new AuthorDTO(author)).ToListAsync();

    return result;
})
.WithName("GetAuthors")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Obtém o cadastro dos autores",
    Description = "Está API pode ser usada para retornar os autores.\r\n///\r\n/// " +
                  "Não há parâmetros obrigatórios para esta API."
});

app.MapGet("api/books", async (DataContext context) =>
{
    var result = await context.Books.Include(book => book.Category)
                                    .Include(book => book.Author)
                                    .Select(book => new BookResponseDTO(book)).ToListAsync();

    return result;

})
.WithName("GetBookDetail")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Obtém o cadastro de livros",
    Description = "Está API pode ser usada para retornar os livros.\r\n///\r\n/// " +
                  "Não há parâmetros obrigatórios para esta API."
});

app.MapGet("api/books/{id}/detail", async ([FromRoute] Guid id, DataContext context, CancellationToken token) =>
{
    var book = await context.Books.FindAsync(id, token);

    if (book is null)
    {
        return Results.NotFound();
    }

    var result = new BookDetailResponseDTO(book);

    return TypedResults.Ok(result);
})
.WithName("GetBooks")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Obtém o cadastro de livros",
    Description = "Está API pode ser usada para retornar os livros.\r\n///\r\n/// " +
                  "Não há parâmetros obrigatórios para esta API."
});

app.MapPost("api/books", async ([FromBody] BookDTO request, BookService service, CancellationToken token) =>
{
    try
    {
        await service.RegisterBookAsync(request, token);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }

    return TypedResults.Created();
})
.WithName("PostBooks")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Realiza o cadastro do livro",
    Description = "Use esta API para realizar o cadastramento do livro. Todos os detalhes devem ser passados no corpo da requisição."
})
.Produces((int)HttpStatusCode.Created)
.Produces((int)HttpStatusCode.BadRequest);

app.Run();

//public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
//{
//    public DbSet<Author> Authors { get; set; }
//    public DbSet<Category> Categories { get; set; }
//    public DbSet<Book> Books { get; set; }
//}

//public class AuthorMapping : IEntityTypeConfiguration<Author>
//{
//    public void Configure(EntityTypeBuilder<Author> builder)
//    {
//        builder.HasKey(c => c.Id);
//        builder.HasIndex(c => c.Email).IsUnique();
//    }
//}

//public class CategoryMapping : IEntityTypeConfiguration<Category>
//{
//    public void Configure(EntityTypeBuilder<Category> builder)
//    {
//        builder.HasKey(c => c.Id);
//        builder.HasIndex(c => c.Name).IsUnique();
//    }
//}

//public class BookMapping : IEntityTypeConfiguration<Book>
//{
//    public void Configure(EntityTypeBuilder<Book> builder)
//    {
//        builder.HasKey(c => c.Id);

//        builder.HasIndex(c => c.Title).IsUnique();

//        builder.HasOne(c => c.Author)
//               .WithOne()
//               .HasForeignKey<Author>("Id")
//               .IsRequired();

//        builder.HasOne(c => c.Category)
//               .WithOne()
//               .HasForeignKey<Category>("Id")
//               .IsRequired();

//        builder.Property(c => c.Price).HasPrecision(10, 2);
//    }
//}