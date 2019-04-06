using Ordering.Domain.Aggregates;
using Ordering.Domain.Events;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain
{
    public class Order : Entity
    {
        private readonly string _userId;
        private readonly string _username;
        private readonly Address _address;
        private readonly int _cardTypeId;
        private readonly string _cardNumber;
        private readonly string _cardSecurityNumber;
        private readonly string _cardHolderName;
        private readonly DateTime _expirationDate;
        private OrderStatus _orderStatus;
        private List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order(
            string userId,
            string username,
            Address address,
            int cardTypeId,
            string cardNumber,
            string cardSecurityNumber,
            string cardHolderName,
            DateTime expirationDate)
        {
            _userId = userId;
            _username = username;
            _address = address;
            _cardTypeId = cardTypeId;
            _cardNumber = cardNumber;
            _cardSecurityNumber = cardSecurityNumber;
            _cardHolderName = cardHolderName;
            _expirationDate = expirationDate;
            _orderStatus = OrderStatus.Submitted;
            _orderItems = new List<OrderItem>();

            AddDomainEvent(new OrderStatusChangedToSubmittedDomainEvent());
        }

        public void AddOrderItem(int productId,
            string productName,
            decimal price,
            int quantity = 1)
        {
            var item = _orderItems.SingleOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.AddQuantity(quantity);
            }
            else
            {
                _orderItems.Add(new OrderItem(productId, productName, price, quantity));
            }
        }

        public decimal GetItemsTotalExcludingVat() =>
            _orderItems.Aggregate(0M, (price, item) => price + item.Price);

        public decimal GetItemsVat() =>
            _orderItems.Aggregate(0M, (vat, item) => vat + item.CalculateVat());

        public void SetOrderUnderProcessStatus()
        {
            if (_orderStatus == OrderStatus.Submitted)
            {
                AddDomainEvent(new OrderStatusChangedToUnderProcessDomainEvent());
                _orderStatus = OrderStatus.UnderProcess;
            }
        }

        public void SetOrderConfirmedStatus()
        {
            if (_orderStatus == OrderStatus.UnderProcess)
            {
                AddDomainEvent(new OrderStatusChangedToUnderProcessDomainEvent());
                _orderStatus = OrderStatus.Confirmed;
            }
        }

        public void SetOrderCancelledStatus()
        {
            if (_orderStatus == OrderStatus.Shipped || _orderStatus == OrderStatus.Invoiced)
            {
                throw new Exception("Cannot cancel order at this point");
            }

            AddDomainEvent(new OrderStatusChangedToCancelledDomainEvent());
            _orderStatus = OrderStatus.Cancelled;
        }

        public void SetOrderCancelledWhenStockIsRejected(IEnumerable<int> stockRejectedItems)
        {
            if (_orderStatus == OrderStatus.UnderProcess) {
                _orderStatus = OrderStatus.Cancelled;

                var rejectedItemNames = _orderItems
                    .Where(i => stockRejectedItems.Contains(i.ProductId))
                    .Select(i => i.ProductName);
                AddDomainEvent(new OrderItemStockRejectedDomainEvent(rejectedItemNames));
            }
        }

        public void SetOrderShippedStatus()
        {
            if (_orderStatus != OrderStatus.Invoiced)
            {
                throw new Exception("Cannot ship an unpaid order");
            }

            AddDomainEvent(new OrderStatusChangedToShippedDomainEvent());
            _orderStatus = OrderStatus.Shipped;
        }

        public void SetOrderDeliveredStatus()
        {
            if (_orderStatus == OrderStatus.Shipped)
            {
                AddDomainEvent(new OrderStatusChangedToShippedDomainEvent());
                _orderStatus = OrderStatus.Delivered;
            }
        }

        public void SetOrderInvoicedStatus()
        {
            if (_orderStatus == OrderStatus.Confirmed)
            {
                AddDomainEvent(new OrderStatusChangedToInvoicedDomainEvent());
                _orderStatus = OrderStatus.Invoiced;
            }
        }
    }
}
