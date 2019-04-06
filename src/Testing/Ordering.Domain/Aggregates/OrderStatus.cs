namespace Ordering.Domain.Aggregates
{
    public enum OrderStatus
    {
        UnderProcess,
        Submitted,
        Confirmed,
        Cancelled,
        Shipped,
        Delivered,
        Invoiced
    }
}
