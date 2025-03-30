using LMS.Domain.Entities.Orders;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Orders
{
    public class CartItemRepository : BaseRepository<CartItem>
    {
        private readonly AppDbContext _context;
        public CartItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem is not null)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                throw new Exception("CartItem not found");
            }
        }
    }
}
