using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public record BookDetailResponseDTO
{
    public BookDetailResponseDTO(Book book)
    {
        Id = book.Id;
        Title = book.Title;
        Overview = book.Overview;
        Summary = book.Summary;
        Price = book.Price;
        PageNumber = book.PageNumber;
        Isbn = book.Isbn;
        Publication = book.Publication;
        Author = new AuthorDTO(book.Author);
        Category = new CategoryDTO(book.Category);
    }

    [Required(ErrorMessage = "Id de identificação")]
    public Guid Id { get; set; }

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

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public DateOnly Publication { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public CategoryDTO Category { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public AuthorDTO Author { get; set; } 
}