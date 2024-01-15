namespace seed_desafio_cdc_tests.csproj
{
    public class AuthorTest
    {
        //BDD
        //Given: dado um determinado contexto(tem-se determinada rea��o);
        //When: quando ocorrer algo;
        //Then: ent�o se espera algo.
        //Exemplo: dada(given) uma nova promo��o, quando(then) ela for lan�ada oficialmente, ent�o (then) ser� enviada uma notifica��o a um determinado grupo.

        [Fact(DisplayName = "Validar Email n�o fornecido (Vazio/nulo)")]
        public void Given_Email_When_VazioOuNulo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert            

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: string.Empty, name: "Name", description: "Descri��o");
            });

            Assert.Equal("Email. Campo obrigat�rio n�o fornecido", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: null, name: "Name", description: "Descri��o");
            });

            Assert.Equal("Email. Campo obrigat�rio n�o fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Email em formato inv�lido")]
        public void Given_Email_When_Invalido_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafaelgmail.com", name: "Name", description: "Descri��o");
            });

            Assert.Equal("Email em formato inv�lido", ex.Message);
        }

        [Fact(DisplayName = "Validar Nome n�o fornecido (Vazio/nulo) ")]
        public void Given_Nome_When_VazioOuNulo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: string.Empty, description: "Descri��o");
            });

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: null, description: "Descri��o");
            });

            Assert.Equal("Name. Campo obrigat�rio n�o fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Descri��o n�o fornecida (Vazio/Nulo)")]
        public void Given_Descricao_When_VazioOuNulo_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail.com", name: "Rafael", description: string.Empty);
            });

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail.com", name: "Rafael", description: null);
            });

            Assert.Equal("Description. Campo obrigat�rio n�o fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Nome menor que o tamanho minimo (3 caracteres)")]
        public void Given_Nome_When_MenorQueOTamanhoMinimo_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: "Ra", description: "Descri��o");
            });

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: "R", description: "Descri��o");
            });

            Assert.Equal("Name. Deve conter ao menos {3} caracteres", ex.Message);
        }

        [Fact(DisplayName = "Validar Descricao menor que o tamanho minimo (3 caracteres)")]
        public void Given_Descricao_When_MenorQueOTamanhoMinimo_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail.com", name: "Rafael", description: "De");
            });

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail.com", name: "Rafael", description: "D");
            });

            Assert.Equal("Description. Deve conter ao menos {3} caracteres", ex.Message);
        }
    }
}