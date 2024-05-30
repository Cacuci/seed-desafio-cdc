using Microsoft.EntityFrameworkCore;

namespace seed_desafio_cdc
{
    public class CategoryService(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task RegisterCategoryAsync(CategoryDTO categoryDTO, CancellationToken token)
        {
            bool exist = await _context.Categories.AnyAsync(category => category.Name.Equals(categoryDTO.Name, StringComparison.InvariantCultureIgnoreCase), token);

            if (exist)
            {
                throw new Exception("Categoria já existente");
            }

            var category = categoryDTO.MapToModel();

            await _context.Categories.AddAsync(category, token);

            await _context.SaveChangesAsync(token);
        }
    }
}
