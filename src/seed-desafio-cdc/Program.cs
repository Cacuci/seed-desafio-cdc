using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.OpenApi.Models;
using seed_desafio_cdc;
using System.ComponentModel;
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

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AuthorService>();
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

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
}

public class AuthorMapping : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Email).IsUnique();
    }
}

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Name).IsUnique();
    }
}

public class Author
{
    public Author(string email, string name, string description)
    {
        Email = email;
        Name = name;
        Description = description;

        Validation();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime DateRegister { get; } = DateTime.UtcNow;

    void Validation()
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

public class Category
{
    public Category(string name)
    {
        Name = name;

        Validation();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }

    void Validation()
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new Exception("Name. Campo obrigatório não fornecido");
        }

        if (Name.Length < 3)
        {
            throw new Exception("Name. Deve conter ao menos {3} caracteres");
        }
    }
}

public class Book
{
    public Book(string title, string overview, decimal price, int pageNumber, string isbn, string category, string author)
    {
        Title = title;
        Overview = overview;
        Price = price;
        PageNumber = pageNumber;
        Isbn = isbn;
        Category = category;
        Author = author;

        Validation();
    }

    public string Title { get; private set; }

    public string Overview { get; private set; }

    public string Summary { get; private set; }

    public decimal Price { get; private set; }

    public int PageNumber { get; private set; }

    public string Isbn { get; private set; }

    public DateOnly Publication { get; private set; } = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);

    public string Category { get; private set; }

    public string Author { get; private set; }


    void Validation()
    {
        if (string.IsNullOrEmpty(Title))
        {
            throw new Exception("Title. Campo obrigatório não fornecido");
        }

        if (string.IsNullOrEmpty(Overview))
        {
            throw new Exception("Overview. Campo obrigatório não fornecido");
        }

        if (Overview.Length >= 500)
        {
            throw new Exception("Overview. Deve conter no mãximo 500 caracteres");
        }

        if (Price is < 20 or > 999999999999.999m)
        {
            throw new Exception("Price. Valor deve ser maior/igual 20 e menor que 999999999999.999");
        }

        if (PageNumber is < 100 or > 65535)
        {
            throw new Exception("PageNumber. Valor deve ser maior/igual 100 e menor que 65535");
        }

        if (string.IsNullOrEmpty(Isbn))
        {
            throw new Exception("Isbn. Campo obrigatório não fornecido");
        }

        if (Publication < new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day))
        {
            throw new Exception("Publication. A data não pode menor ou igual ao dia atual");
        }

        if (string.IsNullOrEmpty(Category))
        {
            throw new Exception("Category. Campo obrigatório não fornecido");
        }

        if (string.IsNullOrEmpty(Author))
        {
            throw new Exception("Category. Campo obrigatório não fornecido");
        }
    }
}

public record AuthorDTO
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

public record CategoryDTO
{
    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(100, ErrorMessage = "O {0} deve conter ao menos {3} caracteres", MinimumLength = 3)]
    public required string Name { get; set; }

    public Category MapToModel()
    {
        return new Category(Name);
    }
}

public record BookDTO
{
    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(500, ErrorMessage = "O {0} deve conter ao menos {1} caracteres", MinimumLength = 1)]
    public string Overview { get; set; }

    [StringLength(500, ErrorMessage = "O {0} deve conter ao menos {1} caracteres", MinimumLength = 1)]
    public string? Summary { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [DefaultValue(20)]
    [Range(20, 999999999999.999, ErrorMessage = "Valor deve ser maior/igual 20 e menor que 999999999999.999")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [Range(100, 65535, ErrorMessage = "Valor deve ser maior/igual 100 e menor que 65535")]
    public int PageNumber { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public string Isbn { get; set; }

    public DateOnly Publication { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public string CategoryCode { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public string Author { get; set; }
}
