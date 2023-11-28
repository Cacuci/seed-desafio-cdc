using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    option.UseInMemoryDatabase("DesafioDB");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// <summary>
/// Realiza o cadastro do autor
/// </summary>
/// <remarks>
/// Use esta API para realizar o cadastramento do autor. Todos os detalhes devem ser passados no corpo da requisição.
/// </remarks>        
/// <response code="200">Se a requisição foi bem sucedida</response>
app.MapPost("api/authors", async ([FromBody] AuthorDTO request, DataContext context) =>
{
    var authorModel = request.MapToModel();

    await context.Authors.AddAsync(authorModel);

    await context.SaveChangesAsync();

    return Results.Ok();
})
.WithName("PostAuthors")
.WithOpenApi();

/// <summary>
/// Obtem o cadastro do autor
/// </summary>
/// <remarks>
/// Está API pode ser usada para retornar os autores.
///
/// Não há parâmetros obrigatórios para esta API.
/// </remarks>        
/// <response code="200">Se a requisição foi bem sucedida</response>
app.MapGet("api/authors", async (DataContext context) =>
{
    var result = await context.Authors.Select(author => new AuthorDTO()
    {
        Email = author.Email,
        Name = author.Name,
        Description = author.Description
    }).ToListAsync();

    return result;
})
.WithName("GetAuthors")
.WithOpenApi();

app.Run();

class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
}

class DataMapping : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(c => c.Id);
    }
}

class Author(string email, string name, string description)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Email { get; private set; } = email;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public DateTime DateRegister { get; } = DateTime.UtcNow;
}

internal record AuthorDTO
{
    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Email em formato inválido")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(100, ErrorMessage = "A {0} deve conter ao menos {3} caracteres", MinimumLength = 3)]
    public required string Name { get; set; }
    
    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(400, ErrorMessage = "A {0} deve conter ao menos {1} caractere", MinimumLength = 1)]
    public required string Description { get; set; }

    public Author MapToModel()
    {
        return new Author(Email, Name, Description);
    }
}
