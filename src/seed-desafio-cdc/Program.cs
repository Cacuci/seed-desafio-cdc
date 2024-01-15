using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

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

app.MapPost("api/authors", async ([FromBody] AuthorDTO request, DataContext context) =>
{
    bool exist = await context.Authors.AnyAsync(author => author.Email.Contains(request.Email, StringComparison.InvariantCultureIgnoreCase));

    if (exist)
    {
        return Results.BadRequest("Esse endereço de email já está em uso");
    }

    var authorModel = request.MapToModel();

    await context.Authors.AddAsync(authorModel);

    await context.SaveChangesAsync();

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
    var result = await context.Authors.Select(author => new AuthorDTO()
    {
        Email = author.Email,
        Name = author.Name,
        Description = author.Description
    }).ToListAsync();

    return result;
})
.WithName("GetAuthors")
.WithOpenApi(option => new OpenApiOperation(option)
{
    Summary = "Obtém o cadastro dos autores",
    Description = "Está API pode ser usada para retornar os autores.\r\n///\r\n/// " +
                  "Não há parâmetros obrigatórios para esta API."
});

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

public partial class Author
{
    public Author(string email, string name, string description)
    {
        Email = email;
        Name = name;
        Description = description;

        Validation();
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime DateRegister { get; } = DateTime.UtcNow;

    public void Validation()
    {
        if (string.IsNullOrEmpty(Email))
        {
            throw new Exception("Email. Campo obrigatório não fornecido");
        }

        if (string.IsNullOrEmpty(Name))
        {
            throw new Exception("Name. Campo obrigatório não fornecido");
        }

        if (string.IsNullOrEmpty(Description))
        {
            throw new Exception("Description. Campo obrigatório não fornecido");
        }

        if (Name.Length < 3)
        {
            throw new Exception("Name. Deve conter ao menos {3} caracteres");
        }

        if (Description.Length < 3)
        {
            throw new Exception("Description. Deve conter ao menos {3} caracteres");
        }

        Match emailValid = Regex.Match(Email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

        if (!emailValid.Success)
        {
            throw new Exception("Email em formato inválido");
        }
    }
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
