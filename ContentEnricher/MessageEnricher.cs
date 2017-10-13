using System;
using System.Collections.Generic;
using System.Text;
using Messages;

namespace ContentEnricher
{
    public static class MessageEnricher
    {
        public static EnrichedOrderRequestMessageToLocalWarehouse EnrichMessage(OrderRequestMessageToLocalWarehouse message)
        {
            EnrichedOrderRequestMessageToLocalWarehouse enrichedMessage = new EnrichedOrderRequestMessageToLocalWarehouse
            {
                ProductId = message.ProductId,
                CustomerId = message.CustomerId,
                Country = GetCustomer(message.CustomerId).country,
                OrderId = message.OrderId,
                ReplyTo = message.ReplyTo
            };

            return enrichedMessage;
        }

        //Simulates an external source where we store customer information
        private static Customer GetCustomer(int Id)
        {
            Customer customer1 = new Customer(1, "DK");
            Customer customer2 = new Customer(2, "SE");
            Customer customer3 = new Customer(3, "DK");
            List<Customer> customers = new List<Customer> { customer1, customer2, customer3 };

            foreach (Customer cus in customers)
            {
                if (cus.customerID == Id)
                    return cus;
            }
            return null;
        }

        private class Customer
        {
            public int customerID { get; set; }
            public string country { get; set; }

            public Customer(int customerID, string country)
            {
                this.customerID = customerID;
                this.country = country;
            }
        }
    }
}
