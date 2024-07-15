using Microsoft.EntityFrameworkCore;
using seed_desafio_cdc.Context;

namespace seed_desafio_cdc
{
    public class BookService(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task RegisterBookAsync(BookDTO bookDTO, CancellationToken token)
        {
            bool found = await _context.Books.AnyAsync(item => item.Title == bookDTO.Title);

            // bool found = await _context.Books.AnyAsync(item => item.Title.Equals(bookDTO.Title));

            if (found)
            {
                throw new Exception("Livro já existente");
            }

            Author? author = await _context.Authors.SingleOrDefaultAsync(item => item.Email == bookDTO.Author.Email);

            if (author is null)
            {
                author = bookDTO.MapToAuthorModel();

                await _context.Authors.AddAsync(author);
            }

            Category? category = await _context.Categories.SingleOrDefaultAsync(item => item.Name == bookDTO.Category.Name);

            if (category is null)
            {
                category = bookDTO.MapToCategory();

                await _context.Categories.AddAsync(category);
            }

            Book book = bookDTO.MapToModel(category, author);

            await _context.Books.AddAsync(book, token);

            await _context.SaveChangesAsync();
        }
    }
}