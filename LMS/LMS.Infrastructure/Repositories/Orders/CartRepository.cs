using LMS.Domain.Entities.Orders;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Orders
{
    public class CartRepository : BaseRepository<Cart>
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if(cart is not null)
            {
                await _context.Entry(cart)
                    .Collection(c => c.CartItems)
                    .LoadAsync();
                var cartItems = cart.CartItems.ToList();

                _context.CartItems.RemoveRange(cartItems);

                cart.IsCheckedOut = true;
            }
            else
            {
                throw new Exception("Cart not found");
            }
        }
    }
}
