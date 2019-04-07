using Ordering.Domain.Aggregates;
using System;
using Xunit;
using static Xunit.Assert;

namespace Ordering.Domain.Unit.Tests.Aggregates.OrderItemTests
{
    public class CalculateVat
    {
        private const int DefaultProductId = 1;
        private const string DefaultProductName = "Random Product";

        [Theory]
        [InlineData(10.0, 2.3)]
        [InlineData(54.8, 12.604)]
        [InlineData(212, 48.76)]
        public void ShouldCalculateTwentyThreePercentVatOfOriginalPriceForSingleUnit(decimal price, decimal expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, price);

            // Act
            var vat = orderItem.CalculateVat();

            // Assert
            Equal(expected, vat);
        }

        [Theory]
        [InlineData(10.0, 6, 13.8)]
        [InlineData(54.8, 2, 25.208)]
        [InlineData(212, 4, 195.04)]
        public void ShouldCalculateTwentyThreePercentVatOfOriginalPriceForMultipleUnit(
            decimal price, 
            int quantity,
            decimal expected)
        {
            // Arrange
            var orderItem = new OrderItem(DefaultProductId, DefaultProductName, price, quantity);

            // Act
            var vat = orderItem.CalculateVat();

            // Assert
            Equal(expected, vat);
        }
    }
}
