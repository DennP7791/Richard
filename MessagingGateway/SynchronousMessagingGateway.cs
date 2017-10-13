using EasyNetQ;
using Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MessagingGateway
{
    public class SynchronousMessagingGateway
    {
        private int customerID;
        IBus bus = RabbitHutch.CreateBus("host=localhost");
        string replyMessage = null;
        int timeout = 10000;
        

        public SynchronousMessagingGateway(int customerID)
        {
            this.customerID = customerID;
            bus.Subscribe<OrderReplyMessage>("customer" + customerID,
               HandleOrderEvent, x => x.WithTopic(customerID.ToString()));
        }

        public string SendRequest(int productID)
        {
            CustomerOrderRequestMessage request = new CustomerOrderRequestMessage
            {
                CustomerId = customerID,
                ProductId = productID
            };
            bool gotReply;

            bus.Send<CustomerOrderRequestMessage>("retailerQueue", request);

            lock (this)
            {
                gotReply = Monitor.Wait(this, timeout);
            }

            if (gotReply)
                return replyMessage;
            else
                throw new Exception("Timeout. The requested product is out of stock!");
        }

        private void HandleOrderEvent(OrderReplyMessage message)
        {
            StringBuilder reply = new StringBuilder();
            reply.Append("Order reply received by customer:" + customerID + "\n");
            reply.Append("Warehouse Id: " + message.WarehouseId + "\n");
            reply.Append("Order Id: " + message.OrderId + "\n");
            reply.Append("Items in stock: " + message.ItemsInStock + "\n");
            reply.Append("Shipping charge: " + message.ShippingCharge + "\n");
            reply.Append("Days for delivery: " + message.DaysForDelivery + "\n");
            replyMessage = reply.ToString();

            lock (this)
            {
                // Wake up the blocked Customer thread
                Monitor.Pulse(this);
            }
        }
    }
}
