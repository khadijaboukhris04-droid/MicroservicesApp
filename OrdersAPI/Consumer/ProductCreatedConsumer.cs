using MassTransit;
using Shared.Models;

namespace OrdersAPI.Consumer
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly OrdersAPIContext _context;

        public ProductCreatedConsumer(OrdersAPIContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var newProduct = new Product
            {
                Name = context.Message.Name
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
        }
    }
}