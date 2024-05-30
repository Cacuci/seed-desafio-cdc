using Microsoft.EntityFrameworkCore;

namespace seed_desafio_cdc
{
    public class BookService(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task RegisterBookAsync(BookDTO bookDTO, CancellationToken token){

            bool exist = await _context.Books.AnyAsync(book => book.Title.Equals(bookDTO.Title, StringComparison.InvariantCultureIgnoreCase));

            if (exist)
            {
                throw new Exception("Livro já existente");
            }

            var book = bookDTO.MapToModel();

            await _context.Books.AddAsync(book, token);

            await _context.SaveChangesAsync();
        }
    }
}