Cost and Pack Breakdown
=======================

Overview
--------
Input a quantity and product code, and the program with output the cost and pack breakdown.
Products have packs of multiple sizes. The program chooses the pack sizes so that the order
contains the minimum number of packs required.

Assumptions
-----------
- If the number ordered cannot be packed completely, a message appears indicating
  this and the order is canceled
- If the input is incorrect, such as the quantity is not a number, or the product code
  cannot be found, a message appears indicating this and the order is canceled
- Product codes do not have spaces
- A blank line as input indicates the end of an order. When this occurs, the output
  will be displayed.
- If pack sizes or costs change, packs.txt will need to be edited and the program
  restarted
- The maximum quantity that can be ordered in one input line has been limited to 90.
  Above this, the program may take too much time or space (see Algorithm Remarks
  below).
- Output lines such as "  2 x 5 $4.49" are indented with two spaces

Instructions
------------
Input and output is through the command line. The packs data is stored in packs.txt.

Follow to following steps:
1. Run the program
2. Enter the path to the packs.txt file (e.g. C:/folder/packs.txt)
3. Enter each line of the order (e.g. 28 YT2)
4. When the order is finished, enter a blank line
5. The cost and pack breakdown will be displayed
6. Repeat from step 3

Running the Tests
-----------------
To run the tests, add 'test' as the first command line argument when compiling the program.

In Visual Studio:
1. Click Debug -> CostAndPackBreakdown Properties...
2. Under the Debug tab, in the Command line arguments text box, type: test

Algorithm Remarks
-----------------
The algorithm to determine the minimum number of packs checks all combinations of 1 pack,
then 2 packs, 3, 4, etc. The time and space complexity of this brute force approach is nⁿ
where n is the number of packs required.
