using System;

namespace CostAndPackBreakdown
{
    class Pack
    {
        public string ProductName { get; }
        public string ProductCode { get; }
        public int Size { get; }
        public decimal Cost { get; }

        /// <summary>
        /// Performs validation and creates a new Pack object.
        /// </summary>
        public Pack(string productName, string productCode,
                    string size, string cost)
        {
            string message = PerformValidation(size, cost);
            if (message != "")
            {
                throw new Exception(message);
            }

            ProductName = productName;
            ProductCode = productCode;
            Size = Int32.Parse(size);
            Cost = Decimal.Parse(cost);
        }

        /// <summary>
        /// Returns a message of any errors.
        /// Returns an empty string if there are no errors.
        /// </summary>
        string PerformValidation(string size, string cost)
        {
            string message = "";

            // Validate size
            try
            {
                int value = Int32.Parse(size);
                if (value < 1)
                {
                    message += Environment.NewLine;
                    message += "Invalid pack size:" + size;
                }
            }
            catch
            {
                message += Environment.NewLine;
                message += "Invalid pack size:" + size;
            }

            // Validate cost
            try
            {
                Decimal.Parse(cost);
            }
            catch
            {
                message += Environment.NewLine;
                message += "Invalid pack cost:" + cost;
            }

            return message;
        }
    }
}
