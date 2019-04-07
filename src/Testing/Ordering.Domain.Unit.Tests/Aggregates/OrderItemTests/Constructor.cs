using Ordering.Domain.Aggregates;
using System;
using Xunit;
using static Xunit.Assert;

namespace Ordering.Domain.Unit.Tests.Aggregates.OrderItemTests
{
    public class Constructor
    {
        private const int DefaultProductId = 1;
        private const string DefaultProductName = "Random Product";
        private const decimal DefaultPrice = 100M;

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldThrowArgumentExceptionIfQuantityIsLessThanOrEqualToZero(int quantity)
        {
            // Arrange | Act
            var exception = Throws<ArgumentException>(() =>
                new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice, quantity));

            // Assert
            Equal("Invalid quantity", exception.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(50)]
        public void ShouldCreateOrderItemInstanceSuccessfully(int quantity)
        {
            // Arrange | Act
            var orderItem = GetOrderItem(quantity);

            // Assert
            Equal(DefaultPrice, orderItem.Price);
            Equal(DefaultProductId, orderItem.ProductId);
            Equal(DefaultProductName, orderItem.ProductName);
            Equal(quantity, orderItem.Quantity);
        }

        private OrderItem GetOrderItem(int quantity) =>
            new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice, quantity);
    }
}
