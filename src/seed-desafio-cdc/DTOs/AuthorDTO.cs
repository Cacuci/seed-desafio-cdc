using System.ComponentModel.DataAnnotations;

public record AuthorDTO
{
    public AuthorDTO() { }
    
    public AuthorDTO(Author author)
    {
        Email = author.Email;
        Name = author.Name;
        Description = author.Description;
    }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Email em formato inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(100, ErrorMessage = "A {0} deve conter ao menos {3} caracteres", MinimumLength = 3)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(400, ErrorMessage = "A {0} deve conter ao menos {1} caractere", MinimumLength = 1)]
    public string Description { get; set; }

    public Author MapToModel()
    {
        return new Author(Email, Name, Description);
    }
}