namespace seed_desafio_cdc_tests.csproj
{
    public class CategoryTest
    {
        //BDD
        //Given: dado um determinado contexto(tem-se determinada reação);
        //When: quando ocorrer algo;
        //Then: então se espera algo.
        //Exemplo: dada(given) uma nova promoção, quando(then) ela for lançada oficialmente, então (then) será enviada uma notificação a um determinado grupo.

        [Fact(DisplayName = "Validar Nome não fornecido (Vazio/nulo)")]
        public void Given_Name_When_VazioOuNulo_Then_DeveLancarExcecao()
        {
            //Arrange & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Category(name: string.Empty);
            });

            Assert.Equal("Name. Campo obrigatório não fornecido", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Category(name: null);
            });

            Assert.Equal("Name. Campo obrigatório não fornecido", ex.Message);
        }

        [Fact(DisplayName = "Validar Nome menor que o tamanho minimo (3 caracteres)")]
        public void Give_Name_When_MenorQueOTamanhoMinimo_DeveLancarExcecao()
        {
            //Arragne & Act & Assert

            var ex = Assert.Throws<Exception>(() =>
            {
                new Category(name: "R");
            });

            Assert.Equal("Name. Deve conter ao menos {3} caracteres", ex.Message);

            ex = Assert.Throws<Exception>(() =>
            {
                new Category(name: "RE");
            });

            Assert.Equal("Name. Deve conter ao menos {3} caracteres", ex.Message);
        }
    }
}
