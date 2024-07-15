using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public record BookDTO
{
    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(500, ErrorMessage = "O {0} deve conter ao menos {1} caracteres", MinimumLength = 1)]
    public string Overview { get; set; }

    [MaxLength(500, ErrorMessage = "O {0} deve conter no maximo {1} caracteres")]
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
    public CategoryDTO Category { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public AuthorDTO Author { get; set; }

    public Book MapToModel(Category category, Author author) => new(Title, Overview, Summary, Price, PageNumber, Isbn, category, author);

    public Author MapToAuthorModel() => new (Author.Email, Author.Name, Author.Description);

    public Category MapToCategory() => new(Category.Name);
}