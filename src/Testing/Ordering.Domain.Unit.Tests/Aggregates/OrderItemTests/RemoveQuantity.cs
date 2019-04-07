using Ordering.Domain.Aggregates;
using System;
using Xunit;
using static Xunit.Assert;

namespace Ordering.Domain.Unit.Tests.Aggregates.OrderItemTests
{
    public class RemoveQuantity
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
            var exception = Throws<ArgumentException>(() => orderItem.RemoveQuantity(quantity));

            // Assert
            Equal("Invalid quantity", exception.Message);
            Equal(1, orderItem.Quantity);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 6)]
        [InlineData(5, 9)]
        public void ShouldThrowArgumentExceptionWhenQuantityToRemoveIsGreaterThanOrEqualToOriginal(
            int originalQuantity, int toRemove)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice, originalQuantity);

            // Act
            var exception = Throws<ArgumentException>(() => orderItem.RemoveQuantity(toRemove));

            // Assert
            Equal("Invalid quantity", exception.Message);
            Equal(originalQuantity, orderItem.Quantity);
        }

        [Theory]
        [InlineData(5, 1, 4)]
        [InlineData(2, 1, 1)]
        [InlineData(10, 5, 5)]
        public void ShouldDecreaseItemQuantityByTheFactorThatIsPassed(
            int originalQuantity,
            int toRemove, 
            int expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice, originalQuantity);

            // Act
            orderItem.RemoveQuantity(toRemove);

            // Assert
            Equal(expected, orderItem.Quantity);
        }

        [Theory]
        [InlineData(10, 1, 1, 8)]
        [InlineData(9, 1, 2, 6)]
        [InlineData(50, 30, 5, 15)]
        public void ShouldRemoveItemsMultipleTimes(
            int originalQuantity,
            int first, 
            int second, 
            int expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, DefaultPrice, originalQuantity);

            // Act
            orderItem.RemoveQuantity(first);
            orderItem.RemoveQuantity(second);

            // Assert
            Equal(expected, orderItem.Quantity);
        }
    }
}
