using System;
using System.Collections.Generic;
using System.Linq;
using CashOut.Services;
using System.Text;

namespace CashOut.Services
{
    public class MainMenuService
    {
        public void MainMenu()
        {
            var starterInput = string.Empty;

            while (starterInput != "4")
            {

                Console.Clear();

                Console.WriteLine(@"Welcome To CashOut.
Menus:
1.Product List
2.Purchase Out
3.View Purchase Out
4.Exit    

Your Choice:");

                starterInput = Console.ReadLine();

                var numInput = int.TryParse(starterInput, out var isNumInput);

                if (!numInput)
                {
                    Console.WriteLine("Input Must Be Numeric");
                }
                else
                {
                    switch (isNumInput)
                    {
                        case 1:
                            var productListService = new ProductListService();
                            productListService.RunDisplayProductList();
                            DisplayContinueConfirmation();
                            break;
                        case 2:
                            var purchaseproduct = new PurchaseProductService();
                            purchaseproduct.DisplayPurchaseProduct(); //Purchase Out
                            DisplayContinueConfirmation();
                            break;
                        case 3:
                            PurchaseProductService.ShowPurchaseHistory();
                            DisplayContinueConfirmation();
                            break;
                        case 4:
                            Console.WriteLine("Thank for using this application!");//Exit
                            break;
                        default:
                            Console.WriteLine("Input the number from [1...4]");
                            break;
                    }

                }
            }
        }
        void DisplayContinueConfirmation()
        {
            Console.WriteLine("Please press any key to go back.");
            Console.Read();
        }
    }
}