using System;
using System.Collections.Generic;

namespace CostAndPackBreakdown
{
    class Test
    {
        public static void RunAllTests()
        {
            Console.WriteLine("Test pack breakdown...");

            int numSuccess = 0;
            int numTests = 0;

            List<Pack> packListYT2 = new List<Pack>();
            packListYT2.Add(new Pack("Yoghurt", "YT2", "4", "4.95"));
            packListYT2.Add(new Pack("Yoghurt", "YT2", "10", "9.95"));
            packListYT2.Add(new Pack("Yoghurt", "YT2", "15", "13.95"));

            numSuccess += RunPackTest(30, packListYT2, "30 YT2 (2 packs)",
                new List<int> { 15, 15 });
            numTests += 1;

            numSuccess += RunPackTest(28, packListYT2, "28 YT2 (4 packs)",
                new List<int> { 10, 10, 4, 4 });
            numTests += 1;

            numSuccess += RunPackTest(17, packListYT2, "17 YT2 (0 packs)",
                null);
            numTests += 1;

            numSuccess += RunPackTest(86, packListYT2, "86 YT2 (9 packs)",
                new List<int> { 15, 15, 15, 15, 10, 4, 4, 4, 4 });
            numTests += 1;

            Console.WriteLine(numSuccess + "/" + numTests + " successful");
        }

        static int RunPackTest(int qty, List<Pack> packList,
            string testName, List<int> expectedSizes)
        {
            try
            {
                GetPackResult(qty, packList, expectedSizes);

                Console.WriteLine(testName + ": Test Success");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(testName + ": Test Fail - " + e.Message);
                return 0;
            }
        }

        static void GetPackResult(int requiredQty, List<Pack> packList,
            List<int> expectedSizes)
        {
            Packs requiredPacks =
                    Program.GetMinRequiredPacks(requiredQty, packList);

            if (expectedSizes == null)
            {
                if (requiredPacks != null)
                {
                    throw new Exception("Result is not null.");
                }
            }
            else
            {
                foreach (Pack pack in requiredPacks.PackList)
                {
                    if (!expectedSizes.Remove(pack.Size))
                    {
                        throw new Exception("Unexpected size returned.");
                    }
                }

                if (expectedSizes.Count > 0)
                {
                    throw new Exception("Incorrect number of packs.");
                }
            }
        }
    }
}
