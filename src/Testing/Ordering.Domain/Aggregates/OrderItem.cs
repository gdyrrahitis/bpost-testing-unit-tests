using System;

namespace Ordering.Domain.Aggregates
{
    public class OrderItem: Entity
    {
        private const decimal Vat = 0.23M;
        public int ProductId { get; }
        public string ProductName { get; }
        public decimal Price { get; }
        public int Quantity { get; private set;  }

        public OrderItem(
            int productId,
            string productName,
            decimal price,
            int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Invalid quantity");
            }

            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Invalid quantity");
            }

            Quantity += quantity;
        }

        public void RemoveQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Invalid quantity");
            }

            if (quantity >= Quantity)
            {
                throw new ArgumentException("Invalid quantity");
            }

            Quantity -= quantity;
        }

        public decimal CalculateVat() => (Price * Quantity) * Vat;
    }
}
