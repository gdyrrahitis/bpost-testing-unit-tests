using Ordering.Domain.Aggregates;
using System;
using Xunit;
using static Xunit.Assert;

namespace Ordering.Domain.Unit.Tests.Aggregates.OrderItemTests
{
    public class AddQuantity
    {
        private const int DefaultProductId = 1;
        private const string DefaultProductName = "Random Product";
        private const decimal DefaultPrice = 100M;

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldThrowArgumentExceptionWhenQuantityIsLessThanOrEqualToZero(int quantity)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice);

            // Act
            var exception = Throws<ArgumentException>(() => orderItem.AddQuantity(quantity));

            // Assert
            Equal("Invalid quantity", exception.Message);
            Equal(1, orderItem.Quantity);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(8, 9)]
        [InlineData(50, 51)]
        public void ShouldIncreaseDefaultItemQuantityByTheFactorThatIsPassed(int quantity, int expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice);

            // Act
            orderItem.AddQuantity(quantity);

            // Assert
            Equal(expected, orderItem.Quantity);
        }

        [Theory]
        [InlineData(5, 1, 6)]
        [InlineData(6, 8, 14)]
        [InlineData(11, 50, 61)]
        public void ShouldIncreaseItemQuantityByTheFactorThatIsPassed(
            int baseQuantity, 
            int quantity, 
            int expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice, baseQuantity);

            // Act
            orderItem.AddQuantity(quantity);

            // Assert
            Equal(expected, orderItem.Quantity);
        }

        [Theory]
        [InlineData(1, 1, 3)]
        [InlineData(1, 2, 4)]
        [InlineData(50, 5, 56)]
        public void ShouldAddMultipleTimesToItemQuantity(int first, int second, int expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice);

            // Act
            orderItem.AddQuantity(first);
            orderItem.AddQuantity(second);

            // Assert
            Equal(expected, orderItem.Quantity);
        }
    }
}
