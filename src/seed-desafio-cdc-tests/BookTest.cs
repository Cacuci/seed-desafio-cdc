namespace seed_desafio_cdc_tests.csproj
{
    //BDD
    //Given: dado um determinado contexto(tem-se determinada reação);
    //When: quando ocorrer algo;
    //Then: então se espera algo.
    //Exemplo: dada(given) uma nova promoção, quando(then) ela for lançada oficialmente, então (then) será enviada uma notificação a um determinado grupo.
    public class BookTest
    {
        [Fact(DisplayName = "Validar Title não fornecido (Vazio/nulo)")]
        public void Given_Title_When_VazioOuNulo_Then_DeveLacarExcecao()
        {
            // Arrange & Act & Assert            

            var ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: string.Empty, overview: "Overview", price: 20, pageNumber: 100, isbn: "Isbn", category: "Category", author: "Author");
            });

            Assert.Equal("Title. Campo obrigatório não fornecido", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: null, overview: "Overview", price: 20, pageNumber: 100, isbn: "Isbn", category: "Category", author: "Author");
            });
        }

        [Fact(DisplayName = "Validar Overview (Resumo) não fornecido (Vazio/nulo)")]
        public void Given_Overview_When_VazioOuNulo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: "Title", overview: string.Empty, price: 20, pageNumber: 100, isbn: "Isbn", category: "Category", author: "Author");
            });

            Assert.Equal("Overview. Campo obrigatório não fornecido", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: "Title", overview: null, price: 20, pageNumber: 100, isbn: "Isbn", category: "Category", author: "Author");
            });
        }

        [Fact(DisplayName = "Validar Price é menor que o mínimo e maior que o máximo")]
        public void Given_Overview_When_MenorQueOMinimoEMaiorQueOMaximo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: "Title", overview: "Overview", price: 19, pageNumber: 100, isbn: "Isbn", category: "Category", author: "Author");
            });

            Assert.Equal("Price. Valor deve ser maior/igual 20 e menor que 999999999999.999", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: "Title", overview: "Overview", price: 9999999999999.999m, pageNumber: 100, isbn: "Isbn", category: "Category", author: "Author");
            });

            Assert.Equal("Price. Valor deve ser maior/igual 20 e menor que 999999999999.999", ex.Message);
        }

        [Fact(DisplayName = "Validar PageNumber é menor que o mínimo ou maior que o máximo")]
        public void Given_PageNumber_When_MenorQueOMinimoEMaiorQueOMaximo_Then_DeveLancarExcecao()
        {
            // Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Book(title: "Title", overview: "Overview", price: 20, pageNumber: 99, isbn: "Isbn", category: "Category", author: "Author");
            });

            Assert.Equal("PageNumber. Valor deve ser maior/igual 100 e menor que 65535", ex.Message);
        }
    }
}
