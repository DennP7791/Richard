﻿namespace Messages
{
    public class CustomerOrderRequestMessage
    {
        public int ProductId { get; set; }
        public int CustomerId;
    }

    public class RetailerOrderRequestMessage : CustomerOrderRequestMessage
    {
        public int OrderId { get; set; }
        public string ReplyTo { get; set; }
    }

    public class OrderRequestMessageToLocalWarehouse : RetailerOrderRequestMessage
    {
    }

    public class OrderBroadcastRequestMessage : RetailerOrderRequestMessage
    {
    }

    public class EnrichedRetailerOrderRequestMessage : RetailerOrderRequestMessage
    {
        public string Country;
    }
    public class EnrichedOrderRequestMessageToLocalWarehouse : EnrichedRetailerOrderRequestMessage
    {
    }

    public class EnrichedOrderBroadcastRequestMessage : EnrichedRetailerOrderRequestMessage
    {
    }



}
