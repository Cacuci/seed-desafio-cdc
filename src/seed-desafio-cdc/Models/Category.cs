public class Category
{
    public Category(string name)
    {
        Name = name;

        Validation();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }

    // EF Relation
    public ICollection<Book> Books { get; private set; }

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
