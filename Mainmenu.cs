using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Vecka40MiniProjekt
{
    public class Mainmenu
    {
        List<Newitem> Productlist = new List<Newitem>();
        public string filepath = "logfile.txt";

        public void Runsession()
       {
            

            while (true)
            {
                // Here we ask the customer to input a category, a product and its price.
                // As long as the customer continues to add products the loop continues 
                Console.WriteLine("Enter a category of products - press q for Quit");
                string? NewCategory = Console.ReadLine();
                if (NewCategory == "q") { break; }
                if (NewCategory == "") { Runsession(); }

                Console.WriteLine("Enter a product - press q for Quit");
                string? NewProduct = Console.ReadLine();
                if (NewProduct == "q") { break; }
                if (NewProduct == "") { Runsession(); }

                Console.WriteLine("Enter the price of this product");
                string? thePrice = Console.ReadLine();
                if (thePrice == "q") { break; }
                if (thePrice == "") { Runsession(); }

                
                else
                {
                    
                    
                    Newitem A_new_item = new Newitem();
                    // We create a new object instance of the Newitem class and add the object (product) to the Productlist                    
                    A_new_item.Category = NewCategory;
                    A_new_item.Name = NewProduct;
                    A_new_item.Price = Int32.Parse(thePrice);
                    bool found = Productlist.Exists(item => item.Name == NewProduct);
                    Console.WriteLine(found);
                    //If product is not in the productlist already it is added:
                    if (!found)
                    {
                        Productlist.Add(A_new_item);

                        // A logfile is created were we log the userdata:
                        StreamWriter writer = new StreamWriter(filepath, true);
                        writer.WriteLine(A_new_item.Category + "," + A_new_item.Name + "," + A_new_item.Price);
                        writer.Close();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The product was added to the productlist");
                        Console.ResetColor();
                        Console.WriteLine("------------------------------");
                    } 
                    else 
                    { 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"The product {NewProduct} is already in the list!");
                        Console.ResetColor();
                    }
                }
            }

            // Sort the productlist in order by price, lowest first
            List<Newitem> sortedList = Productlist.OrderBy(item => item.Price).ToList();
            List<Newitem> summaryList = new List<Newitem>();

            // When the customer is finished with adding products to the list the productlist is presented with the price:
            Console.WriteLine("------------------------------");
            Console.WriteLine("Category".PadRight(5) + " " + "Product".PadRight(5) + " " + "Price".PadRight(5));
            Console.WriteLine(">>>>>>>>>>>>>>><<<<<<<<<<<<<<<");
            foreach (Newitem item in sortedList)
            {
                Console.WriteLine($"{item.Category.PadRight(5)} {item.Name.PadRight(5)} {item.Price.ToString().PadRight(5)}");
                Console.WriteLine("------------------------------");
            }
            int Totalprice = sortedList.Sum(item => item.Price);
            Console.WriteLine("The total cost of all your products for this session:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Totalprice);
            Console.ResetColor();

            /*We also collect previous data from previous sessions that the customer had and add also the products from the current session
             + the old products earlier added to a new list which is also presented together with category and price:
            To read data from the txt file we open a Streamreader. */
            string data;
            StreamReader reader = new StreamReader(filepath);
            data = reader.ReadLine();
            try
            {
                Console.WriteLine("These are your products from previous sessions:");
                Console.WriteLine("Category".PadRight(5) + " |" + "Product".PadRight(5) + " |" + "Price".PadRight(5));
                Console.WriteLine(">>>>>>>>>>>>>>><<<<<<<<<<<<<<<");
                while (data != null)
                {
                    // Each line in the txt file is a product (categ, product,price) and we split it in substrings of an array separated by comma
                    // The three properties are separated by a comma in the txt file 
                    // We place the substring into an array. Entries at index pos 0 is the first column on each iteration
                    // AnItem that is assigned the properties of each column (category, product, price) on each line (product) in the txt file
                    string[] entries = data.Split(",");
                    Newitem anItem = new Newitem();
                    anItem.Category = entries[0];
                    anItem.Name = entries[1];
                    anItem.Price = Int32.Parse(entries[2]);
                    summaryList.Add(anItem);
                    data = reader.ReadLine();


                }

                //Presentation of the complete product list:
                List<Newitem> oldAndNewProductsSorted = summaryList.OrderBy(item => item.Price).ToList();

                foreach (Newitem anItem in oldAndNewProductsSorted)
                {
                    Console.WriteLine($"{anItem.Category.PadRight(5)} | {anItem.Name.PadRight(5)} | {anItem.Price.ToString().PadRight(5)}");
                }
                int TotalCostOldAndNewProducts = oldAndNewProductsSorted.Sum(item => item.Price);
                Console.WriteLine("The total cost of all your products (this session and previous sessions:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(TotalCostOldAndNewProducts);
                Console.ResetColor();
            }

            // In case there is an error we catch it here...
            catch (Exception Ex) { Console.WriteLine("Something went wrong"); }

            finally
            {
                // The customer gets a couple of options after being presented with the complete list. Either add more products, search for products
                // Or quit session
                reader.Close();
                Console.WriteLine("To enter more products press p | To search for a product press s | To Quit session press q");
                string? Userchoice = Console.ReadLine();
                if (Userchoice != "")
                {
                    if (Userchoice == "p")
                    {
                        Console.WriteLine("Ok, sending you back to Main Menu...");
                        Console.WriteLine("-------------------------------");
                        Runsession();
                    }

                    if (Userchoice == "q") { Console.WriteLine("Session ends...thanks for using our services"); }

                    if (Userchoice == "s")
                    {
                        Console.WriteLine("Search for a product:");
                        Console.WriteLine("------------------------");
                        string? Search = Console.ReadLine();
                        Console.WriteLine("------------------------");
                        Console.WriteLine("------------------------");
                        Console.WriteLine("Category".PadRight(5) + " Product".PadRight(5) + " " + "Price".PadRight(5));

                        foreach (Newitem item in summaryList)
                        {
                            if (item.Name == Search)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"{item.Category.PadRight(5)} {item.Name.PadRight(5)} {item.Price.ToString().PadRight(5)}");
                                Console.ResetColor();
                            }
                            if (item.Name != Search)
                            {
                                Console.WriteLine($"{item.Category.PadRight(5)} {item.Name.PadRight(5)} {item.Price.ToString().PadRight(5)}");
                            }
                        }
                    }
                }
                else { Console.WriteLine("That is not a valid choice"); }

            }
        }

    }
}
