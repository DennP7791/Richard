using System;
using MessagingGateway;

namespace Customer
{
    public class Customer
    {   
        private int customerID;
        private int productID;
        private SynchronousMessagingGateway gateway;

        public Customer(int customerID, int productID)
        {
            this.customerID = customerID;
            this.productID = productID;
            gateway = new SynchronousMessagingGateway(customerID);
        }

        public void Start()
        {
            SynchronizedWriteLine("Customer running. Waiting for a reply.\n");

            try
            {
                SynchronizedWriteLine(gateway.SendRequest(productID));
            }
            catch (Exception ex)
            {
                SynchronizedWriteLine(ex.Message);
            }
        }

        private void SynchronizedWriteLine(string s)
        {
            lock (this)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(s);
                Console.ResetColor();
            }
        }

    }
}
