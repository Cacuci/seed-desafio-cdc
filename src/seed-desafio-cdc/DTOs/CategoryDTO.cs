using System.ComponentModel.DataAnnotations;

public record CategoryDTO
{
    public CategoryDTO() { }

    public CategoryDTO(Category category)
    {
        Name = category.Name;
    }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    [StringLength(100, ErrorMessage = "O {0} deve conter ao menos {3} caracteres", MinimumLength = 3)]
    public string Name { get; set; }

    public Category MapToModel()
    {
        return new Category(Name);
    }
}