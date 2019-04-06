using System.Collections.Generic;

namespace Ordering.Domain.Events
{
    public class OrderItemStockRejectedDomainEvent : IDomainEvent
    {
        private IEnumerable<string> _rejectedItemNames;

        public OrderItemStockRejectedDomainEvent(IEnumerable<string> rejectedItemNames) => 
            _rejectedItemNames = rejectedItemNames;
    }
}
