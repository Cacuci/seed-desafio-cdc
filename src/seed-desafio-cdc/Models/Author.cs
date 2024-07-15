using System.Text.RegularExpressions;

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

    // EF Relation
    public ICollection<Book> Books { get; private set; }

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