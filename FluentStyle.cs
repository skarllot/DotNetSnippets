using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Reference: https://www.simple-talk.com/dotnet/net-framework/fluent-code-in-c/

namespace FluentStyle
{
    class FluentStyle
    {
        public IEnumerable<Order> OrderList { get; private set; }

        private void LinqExample()
        {
            var recentBigOrders = OrderList
                .Where(o => o.Amount > 1000 && o.Date >= DateTime.Now.AddDays(-5))
                .OrderBy(o => o.Amount)
                .Take(10)
                .Select(o => o.Customer);
        }

        private void NonFluentExample()
        {
            // Create the order
            var orderLines = new List<IOrderLineItem>
            {
                new OrderLineItem { ProductId = 123, Quantity = 2, UnitPrice = 2.99 },
                new OrderLineItem { ProductId = 234, Quantity = 1, UnitPrice = 9.99 }
            };
            var order = new Order
            {
                CustomerId = 98765,
                OrderLineItems = orderLines
            };

            // Apply taxes
            var taxCalculator = new TaxCalculator();
            if (taxCalculator.Apply(order))
                taxCalculator.Calculate(order);

            // Validate and process the order
            if (order != null && order.OrderLineItems != null)
            {
                var orderProcessor = new OrderProcessor();
                orderProcessor.Process(order);
            }
        }

        private void FluentExample()
        {
            var orderEngine = OrderEngine
                .Initialize()
                .Customer(98765)
                .AddLineItem(
                    new OrderLineItem { ProductId = 123, Quantity = 2, UnitPrice = 2.99 })
                .AddLineItem(
                    new OrderLineItem { ProductId = 234, Quantity = 1, UnitPrice = 9.99 })
                .Process();
        }

        private void FluentExampleV2()
        {
            var orderEngineTaxing = OrderEngine
                .Initialize()
                .Customer(56789)
                .Using(new TaxCalculatorV2())
                .AddValidateFunction(o => o.Tax > 0)
                .AddLineItem(
                    new OrderLineItem { ProductId = 123, Quantity = 2, UnitPrice = 2.99 })
                .AddLineItem(
                    new OrderLineItem { ProductId = 234, Quantity = 1, UnitPrice = 9.99 })
                .Process();

            var order = orderEngineTaxing.Order;
            Console.WriteLine("Order Id = " + order.OrderId);
        }
    }

    class TaxCalculatorV2 : ITaxCalculator
    {
        public bool Apply(IOrder order) { throw new NotImplementedException(); }
        public void Calculate(IOrder order) { throw new NotImplementedException(); }
    }

    interface IOrder
    {
        List<IOrderLineItem> OrderLineItems { get; set; }
        int OrderId { get; set; }
        int CustomerId { get; set; }
        bool Valid { get; set; }
        DateTime? DateProcessed { get; set; }
        int Tax { get; set; }
    }

    interface IOrderProcessor
    {
        void Process(IOrder order);
    }

    interface ITaxCalculator
    {
        bool Apply(IOrder order);
        void Calculate(IOrder order);
    }

    interface IOrderLineItem { }

    class OrderProcessor : IOrderProcessor
    {
        public void Process(IOrder order) { throw new NotImplementedException(); }
    }

    class TaxCalculator : ITaxCalculator
    {
        public bool Apply(IOrder order) { throw new NotImplementedException(); }
        public void Calculate(IOrder order) { throw new NotImplementedException(); }
    }

    class Order : IOrder
    {
        public int OrderId { get; set; }
        public int Amount { get; internal set; }
        public DateTime Date { get; internal set; }
        public object Customer { get; internal set; }
        public int CustomerId { get; set; }
        public List<IOrderLineItem> OrderLineItems { get; set; }
        public bool Valid { get; set; }
        public DateTime? DateProcessed { get; set; }
        public int Tax { get; set; }
    }

    class OrderLineItem : IOrderLineItem
    {
        public int ProductId { get; internal set; }
        public int Quantity { get; internal set; }
        public double UnitPrice { get; internal set; }
    }

    class OrderEngine
    {
        public ITaxCalculator TaxCalculator { get; internal set; }
        public IOrderProcessor OrderProcessor { get; internal set; }
        public IOrder Order { get; internal set; }
        public List<Func<IOrder, bool>> ValidateFunctions { get; internal set; }

        public static OrderEngine Initialize()
        {
            // Instantiate dependencies
            var orderEngine = new OrderEngine
            {
                Order = new Order(),
                TaxCalculator = new TaxCalculator(),
                OrderProcessor = new OrderProcessor(),
                ValidateFunctions = new List<Func<IOrder, bool>>()
            };
            orderEngine.Order.OrderLineItems = new List<IOrderLineItem>();

            // Set the order Id
            orderEngine.Order.OrderId = (new Random()).Next();

            // Add a simplistic null check
            orderEngine.ValidateFunctions.Add(o => o != null && o.OrderLineItems != null);

            return orderEngine;
        }
    }

    static class OrderEngineExtensions
    {
        public static OrderEngine Customer(this OrderEngine orderEngine, int customerId)
        {
            orderEngine.Order.CustomerId = customerId;
            return orderEngine;
        }

        public static OrderEngine AddLineItem(
            this OrderEngine orderEngine,
            IOrderLineItem orderLineItem)
        {
            if (orderEngine.Order == null)
                orderEngine.Order = new Order();

            if (orderEngine.Order.OrderLineItems == null)
                orderEngine.Order.OrderLineItems = new List<IOrderLineItem>();

            orderEngine.Order.OrderLineItems.Add(orderLineItem);
            return orderEngine;
        }

        public static OrderEngine Process(this OrderEngine orderEngine)
        {
            // Can't instantiate an order for processing; need an order with details
            if (orderEngine == null || orderEngine.Order == null)
                throw new InvalidOperationException("Processing not provided an Order");

            if (orderEngine.TaxCalculator != null
                && orderEngine.TaxCalculator.Apply(orderEngine.Order))
                orderEngine.TaxCalculator.Calculate(orderEngine.Order);

            // Run thru any validation checks
            orderEngine.Order.Valid = true;

            if (orderEngine.ValidateFunctions != null)
            {
                foreach (var validateFunction in orderEngine.ValidateFunctions)
                {
                    orderEngine.Order.Valid =
                        orderEngine.Order.Valid
                        && validateFunction(orderEngine.Order);
                }
            }

            // Process the order
            orderEngine.Order.DateProcessed = null;

            if (orderEngine.Order.Valid)
                orderEngine.OrderProcessor.Process(orderEngine.Order);

            return orderEngine;
        }

        public static OrderEngine Using(
            this OrderEngine orderEngine,
            ITaxCalculator taxCalculator)
        {
            orderEngine.TaxCalculator = taxCalculator;
            return orderEngine;
        }

        public static OrderEngine AddValidateFunction(
            this OrderEngine orderEngine,
            Func<IOrder, bool> validateFunction)
        {
            if (validateFunction == null)
                throw new ArgumentNullException(nameof(validateFunction));

            if (orderEngine.ValidateFunctions == null)
                orderEngine.ValidateFunctions = new List<Func<IOrder, bool>>();

            orderEngine.ValidateFunctions.Add(validateFunction);
            return orderEngine;
        }
    }
}
