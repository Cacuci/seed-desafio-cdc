namespace seed_desafio_cdc_tests.csproj
{
    public class AuthorTest
    {
        //BDD
        //Given: dado um determinado contexto(tem-se determinada reação);
        //When: quando ocorrer algo;
        //Then: então se espera algo.
        //Exemplo: dada(given) uma nova promoção, quando(then) ela for lançada oficialmente, então (then) será enviada uma notificação a um determinado grupo.

        [Fact(DisplayName = "Validar Email não fornecido (Vazio/nulo)")]
        public void Given_Email_When_VazioOuNulo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert            

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: string.Empty, name: "Name", description: "Descrição");
            });

            Assert.Equal("Email. Campo obrigatório não fornecido", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: null, name: "Name", description: "Descrição");
            });

            Assert.Equal("Email. Campo obrigatório não fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Email em formato inválido")]
        public void Given_Email_When_Invalido_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafaelgmail.com", name: "Name", description: "Descrição");
            });

            Assert.Equal("Email em formato inválido", ex.Message);
        }

        [Fact(DisplayName = "Validar Nome não fornecido (Vazio/nulo) ")]
        public void Given_Nome_When_VazioOuNulo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: string.Empty, description: "Descrição");
            });

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: null, description: "Descrição");
            });

            Assert.Equal("Name. Campo obrigatório não fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Descrição não fornecida (Vazio/Nulo)")]
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

            Assert.Equal("Description. Campo obrigatório não fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Nome menor que o tamanho minimo (3 caracteres)")]
        public void Given_Nome_When_MenorQueOTamanhoMinimo_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: "Ra", description: "Descrição");
            });

            ex = Assert.Throws<Exception>(() =>
            {
                new Author(email: "rafael@gmail", name: "R", description: "Descrição");
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