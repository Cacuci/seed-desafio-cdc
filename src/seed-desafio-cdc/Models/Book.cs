public class Book
{
    /// <summary>
    /// Utilizado pelo Entity Framework
    /// </summary>    
    private Book(string title, string overview, string? summary, decimal price, int pageNumber, string isbn)
    {
        Title = title;
        Overview = overview;
        Summary = summary;
        Price = price;
        PageNumber = pageNumber;
        Isbn = isbn;

        Validation();
    }

    public Book(string title, string overview, string? summary, decimal price, int pageNumber, string isbn, Category category, Author author)
    {
        Title = title;
        Overview = overview;
        Summary = summary;
        Price = price;
        PageNumber = pageNumber;
        Isbn = isbn;
        Category = category;
        Author = author;

        Validation();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Title { get; private set; }

    public string Overview { get; private set; }

    public string? Summary { get; private set; }

    public decimal Price { get; private set; }

    public int PageNumber { get; private set; }

    public string Isbn { get; private set; }

    public DateOnly Publication { get; private set; } = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);

    public Guid CategoryId { get; private set; }

    public Guid? AuthorId { get; private set; }

    public Category Category { get; private set; }

    public Author Author { get; private set; }

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
            throw new Exception("Overview. Deve conter no maximo 500 caracteres");
        }

        if (Price is < 20 or > 999999999999.99m)
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

        if (Publication < new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
        {
            throw new Exception("Publication. A data não pode menor ou igual ao dia atual");
        }

        if (string.IsNullOrEmpty(Category.Name))
        {
            throw new Exception("Category. Campo obrigatório não fornecido");
        }

        if (string.IsNullOrEmpty(Author.Name))
        {
            throw new Exception("Author. Campo obrigatório não fornecido");
        }
    }
}