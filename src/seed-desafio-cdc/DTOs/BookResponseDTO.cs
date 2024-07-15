using System.ComponentModel.DataAnnotations;

public record BookResponseDTO
{
    public BookResponseDTO(Book book)
    {
        Id = book.Id;
        Title = book.Title;
    }

    [Required(ErrorMessage = "Id de identificação")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Campo obrigatório não fornecido")]
    public string Title { get; set; }
}
