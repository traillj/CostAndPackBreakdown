using System;
using System.Collections.Generic;
using System.IO;

namespace CostAndPackBreakdown
{
    class Program
    {
        /// <summary>
        /// Inputs the packs data file path and order.
        /// Outputs the cost and pack breakdown.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "test")
                {
                    Test.RunAllTests();

                    Console.WriteLine();
                    Console.WriteLine("Press Enter to exit.");
                    Console.ReadLine();
                    return;
                }
            }
            catch
            {
                // No command line arguments
            }

            // Read the packs data file
            Dictionary<string, List<Pack>> packCodeDict =
                new Dictionary<string, List<Pack>>();
            try
            {
                packCodeDict = GetPackData();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading packs file: " + e.Message);
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Enter a blank line to finish the order.");
            Console.WriteLine("Order:");
            string output = "";

            // Read the order and output the breakdown
            while (1 == 1)
            {
                string input = Console.ReadLine();

                // Blank line to finish the order
                if (input == "")
                {
                    FinishOrder(output);
                    output = "";
                    continue;
                }

                string message = ValidateOrderInput(input, packCodeDict);
                if (message != "")
                {
                    FinishOrder("Error: " + message);
                    output = "";
                    continue;
                }

                string[] inputParts = input.Split(' ');
                int qty = Int32.Parse(inputParts[0]);
                string code = inputParts[1];

                Packs requiredPacks =
                    GetMinRequiredPacks(qty, packCodeDict[code]);

                if (requiredPacks == null)
                {
                    FinishOrder("Error: number ordered cannot " +
                        "be packed completely.");
                    output = "";
                    continue;
                }
                else
                {
                    output += GetCostAndPackBreakdown(requiredPacks, code);
                    output += Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// Writes to the console the message then
        /// starts a new order.
        /// </summary>
        static void FinishOrder(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Order:");
        }

        /// <summary>
        /// Returns a message of any errors.
        /// Returns an empty string if there are no errors.
        /// </summary>
        /// <param name="input">String from console</param>
        /// <param name="packCodeDict">
        /// Dictionary with each product code as a key
        /// </param>
        static string ValidateOrderInput(string input,
            Dictionary<string, List<Pack>> packCodeDict)
        {
            string message = "";

            string[] inputParts = input.Split(' ');
            string qtyPart = "";
            string codePart = "";

            // Validate format
            try
            {
                qtyPart = inputParts[0];
                codePart = inputParts[1];
            }
            catch (Exception e)
            {
                message += Environment.NewLine;
                message += "Invalid input: " + e.Message;
                return message;
            }

            // Validate quantity
            try
            {
                int qty = Int32.Parse(qtyPart);
                if (qty < 1)
                {
                    message += Environment.NewLine;
                    message += "Invalid quantity:" + qtyPart;
                }
            }
            catch
            {
                message += Environment.NewLine;
                message += "Invalid quantity:" + qtyPart;
            }

            // Validate product code
            if (!packCodeDict.ContainsKey(codePart))
            {
                message += Environment.NewLine;
                message += "Product code not found:" + codePart;
            }

            return message;
        }

        /// <summary>
        /// Gets the pack data from a file path read from the console.
        /// </summary>
        /// <returns>
        /// Returns a dictionary with:
        /// Key   - The product codes.
        /// Value - The list of packs for the product code.
        /// </returns>
        static Dictionary<string, List<Pack>> GetPackData()
        {
            // Get the file path to the pack data as input
            //Console.WriteLine("Enter the path to the Packs file:");
            //string packsPath = Console.ReadLine();
            StreamReader reader = new StreamReader(@"C:\a\a\packs.txt");

            Dictionary<string, List<Pack>> packCodeDict =
                new Dictionary<string, List<Pack>>();
            List<Pack> packList = new List<Pack>();

            // Input the pack data
            try
            {
                if (reader.Peek() == -1)
                {
                    throw new Exception("File is empty.");
                }

                while (reader.Peek() != -1)
                {
                    string input = reader.ReadLine();
                    string[] values = input.Split('|');
                    string productCode = values[1];

                    if (packCodeDict.ContainsKey(productCode))
                    {
                        packList = packCodeDict[productCode];
                    }
                    else
                    {
                        packList = new List<Pack>();
                    }

                    packList.Add(new Pack(values[0], values[1],
                        values[2], values[3]));
                    packCodeDict[productCode] = packList;
                }
            }
            finally
            {
                reader.Close();
            }

            return packCodeDict;
        }

        /// <summary>
        /// Gets the minimum number of packs required
        /// for the specified quantity.
        /// </summary>
        /// <param name="requiredQty">Quantity required</param>
        /// <param name="packsWithCode">
        /// Packs available of the required product code
        /// </param>
        public static Packs GetMinRequiredPacks(int requiredQty,
            List<Pack> packsWithCode)
        {
            List<Packs> tryPacksList = new List<Packs>();
            Packs tryPacks = new Packs();

            foreach (Pack pack in packsWithCode)
            {
                tryPacks.TotalSize = pack.Size;

                List<Pack> newPackList = new List<Pack>();
                newPackList.Add(pack);
                tryPacks.PackList = newPackList;

                tryPacksList.Add(tryPacks);
                tryPacks = new Packs();
            }
            
            tryPacks = GetCostAndPackCalc(tryPacksList,
                packsWithCode, requiredQty);

            return tryPacks;
        }

        /// <summary>
        /// Main calculation part of the GetMinRequiredPacks method.
        /// Do not call directly.
        /// Tries all combinations of 1 pack, then 2, 3, etc.
        /// </summary>
        /// <param name="tryPacksList">
        /// Separate copy of packsWithCode
        /// </param>
        /// <param name="packsWithCode">
        /// Packs available of the required product code
        /// </param>
        /// <param name="requiredQty">Quantity required</param>
        static Packs GetCostAndPackCalc(List<Packs> tryPacksList,
            List<Pack> packsWithCode, int requiredQty)
        {
            while (tryPacksList.Count > 0)
            {
                List<Packs> toRemove = new List<Packs>();

                // Try all packs in current list
                foreach (Packs tryPacks in tryPacksList)
                {
                    if (tryPacks.TotalSize == requiredQty)
                    {
                        return tryPacks;
                    }
                    else if (tryPacks.TotalSize > requiredQty)
                    {
                        toRemove.Add(tryPacks);
                    }
                }

                // Remove impossible pack combinations from list
                foreach (Packs tryPacks in toRemove)
                {
                    tryPacksList.Remove(tryPacks);
                }

                if (tryPacksList.Count < 1)
                {
                    break;
                }

                // Add each type of pack to each pack list to be tried
                List<Packs> newTryPacksList = new List<Packs>();
                foreach (Pack pack in packsWithCode)
                {
                    foreach (Packs tryPacks in tryPacksList)
                    {
                        Packs newTryPacks = new Packs();
                        newTryPacks.TotalSize = tryPacks.TotalSize + pack.Size;

                        List<Pack> newPackList = new List<Pack>();
                        foreach (Pack oldPack in tryPacks.PackList)
                        {
                            newPackList.Add(oldPack);
                        }
                        newPackList.Add(pack);
                        newTryPacks.PackList = newPackList;

                        newTryPacksList.Add(newTryPacks);
                    }
                }
                tryPacksList = newTryPacksList;
            }
            return null;
        }

        /// <summary>
        /// Gets a breakdown of one line of the order.
        /// Includes the total cost and amounts of each pack type.
        /// </summary>
        public static string GetCostAndPackBreakdown(Packs requiredPacks,
            string productCode)
        {
            decimal totalCost = GetTotalCost(requiredPacks);
            string output = requiredPacks.TotalSize + " " +
                productCode + " $" + totalCost;

            // Sort size descending
            requiredPacks.PackList.Sort(
                (x, y) => -1 * x.Size.CompareTo(y.Size));

            // Generate line of pack type and quantity
            int prevSize = 0;
            int packQty = 0;
            decimal prevCost = 0;
            foreach (Pack pack in requiredPacks.PackList)
            {
                if (prevSize != pack.Size && prevSize != 0)
                {
                    output += Environment.NewLine + "  ";
                    output += packQty + " x " + prevSize + " $" + prevCost;
                    packQty = 1; // Current pack is different size
                }
                else
                {
                    packQty += 1;
                }
                prevSize = pack.Size;
                prevCost = pack.Cost;
            }
            output += Environment.NewLine + "  ";
            output += packQty + " x " + prevSize + " $" + prevCost;

            return output;
        }

        /// <summary>
        /// Returns the total cost of the pack list.
        /// </summary>
        static decimal GetTotalCost(Packs requiredPacks)
        {
            decimal totalCost = 0;

            foreach (Pack pack in requiredPacks.PackList)
            {
                totalCost += pack.Cost;
            }

            return totalCost;
        }
    }
}
