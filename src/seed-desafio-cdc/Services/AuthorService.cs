using Microsoft.EntityFrameworkCore;
using seed_desafio_cdc.Context;

namespace seed_desafio_cdc
{
    public class AuthorService(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task RegisterAuthorAsync(AuthorDTO authorDTO, CancellationToken token)
        {
            bool exist = await _context.Authors.AnyAsync(author => author.Email.Equals(authorDTO.Email, StringComparison.CurrentCultureIgnoreCase), token);

            if (exist)
            {
                throw new Exception("Esse endereço de email já está em uso");
            }

            var author = authorDTO.MapToModel();

            await _context.Authors.AddAsync(author, token);

            await _context.SaveChangesAsync(token);
        }
    }
}