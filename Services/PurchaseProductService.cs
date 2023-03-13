using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashOut.Models;

namespace CashOut.Services
{
    public class PurchaseProductService
    {
        private static List<PurchaseProductModel> purchaseProduct = new List<PurchaseProductModel>();
        private static List<PurchaseModel> purchaseHistory = new List<PurchaseModel>();

        public void DisplayPurchaseProduct()
        {
            var purchaseProductList = string.Empty;
            while (purchaseProductList != "5")
            {
                Console.Clear();

                Console.WriteLine("Purchase Out");
                Console.WriteLine("Product List:");
                var productListService = new ProductListService();
                productListService.DisplayProductList();

                Console.WriteLine("\n| Product Code | Name | Price | Quantity |");
                foreach (var product in purchaseProduct)
                {
                    Console.WriteLine($"| {product.PurchaseProductCode} | {product.ProductName} | {product.ProductPrice} | {product.Quantity} |");
                }


                Console.WriteLine("Menus:");
                Console.WriteLine("1. Add Product to cart");
                Console.WriteLine("2. Remove Product from cart");
                Console.WriteLine("3. Update Product from cart");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Back");
                Console.WriteLine("Please input your choice: ");
                string purchase = Console.ReadLine();
                int.TryParse(purchase, out int purchaseInput);

                switch (purchaseInput)
                {
                    case 1:
                        AddCart();
                        DisplayContinueConfirmation();
                        break;
                    case 2:
                        RemoveCart();
                        DisplayContinueConfirmation();
                        break;
                    case 3:
                        UpdateCart();
                        DisplayContinueConfirmation();
                        break;
                    case 4:
                        CheckOut();
                        DisplayContinueConfirmation();
                        break;
                    case 5:
                        var mainMenu = new MainMenuService();
                        mainMenu.MainMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again");
                        break;
                }

            }

        }

        void AddCart()
        {
            Console.WriteLine("Please enter your Product Code (type exit to return): ");
            string productCode = Console.ReadLine();
            var productListService = new ProductListService();
            var product = productListService.GetProductByCode(productCode);

            if (productCode == "exit")
            {
                return;
            }


            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }
            else
            {
                Console.WriteLine("Enter Quantity: ");
                string quantityString = Console.ReadLine();
                int.TryParse(quantityString, out int quantity);

                if (quantity <= 0)
                {
                    Console.WriteLine("Quantity must be greater than zero!");
                }
                else if (product.Stock < quantity)
                {
                    Console.WriteLine($"Only {product.Stock} unit(s) left in stock!");
                }
                else
                {
                    var existingPurchaseProduct = purchaseProduct.FirstOrDefault(p => p.PurchaseProductCode == productCode);
                    if (existingPurchaseProduct != null)
                    {
                        existingPurchaseProduct.Quantity += quantity;
                    }
                    else
                    {
                        var newPurchaseProduct = new PurchaseProductModel
                        {
                            PurchaseProductCode = product.ProductCode,
                            ProductName = product.Name,
                            ProductPrice = product.Price,
                            Quantity = quantity
                        };
                        purchaseProduct.Add(newPurchaseProduct);
                    }

                    Console.WriteLine("Product has been added to cart!");
                }
            }
        }
        void RemoveCart()
        {
            Console.WriteLine("Please enter your Product Code (type exit to return): ");
            string productCode = Console.ReadLine();
            var existingPurchaseProduct = purchaseProduct.FirstOrDefault(p => p.PurchaseProductCode == productCode);

            if (productCode == "exit")
            {
                return;
            }

            if (existingPurchaseProduct == null)
            {
                Console.WriteLine("Product not found in cart!");
            }
            else
            {
                purchaseProduct.Remove(existingPurchaseProduct);
                Console.WriteLine("Product has been removed from cart!");
            }
        }

        void UpdateCart()
        {
            Console.WriteLine("Update Product from cart");
            Console.WriteLine("Please enter your Product Code (type exit to return): ");
            string productCode = Console.ReadLine();

            // Check if the product is already in the purchaseProduct list
            var selectedProduct = purchaseProduct.FirstOrDefault(p => p.PurchaseProductCode == productCode);

            if (productCode == "exit")
            {
                return;
            }

            if (selectedProduct == null)
            {
                Console.WriteLine("Product is not found in cart.");
                return;
            }

            Console.WriteLine($"Product Name: {selectedProduct.ProductName}");
            Console.WriteLine($"Current Quantity: {selectedProduct.Quantity}");
            Console.WriteLine("Please input new quantity: ");
            string quantityInput = Console.ReadLine();
            int.TryParse(quantityInput, out int newQuantity);

            if (newQuantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than 0.");
                return;
            }

            // Check if the selected product is still available in the product list
            var productListService = new ProductListService();
            var product = productListService.GetProductByCode(productCode);
            if (product == null)
            {
                Console.WriteLine("Product is not found in product list.");
                return;
            }

            if (newQuantity > product.Stock)
            {
                Console.WriteLine("Insufficient product stock.");
                return;
            }

            selectedProduct.Quantity = newQuantity;
            Console.WriteLine($"Product quantity is updated to {newQuantity}.");
        }

        private static void CheckOut()
        {
            Console.WriteLine("\nUser Cart:");
            Console.WriteLine("| Product Code | Name | Price | Quantity |");
            foreach (var product in purchaseProduct)
            {
                Console.WriteLine($"| {product.PurchaseProductCode} | {product.ProductName} | {product.ProductPrice} | {product.Quantity} |");
            }

            int totalPrice = purchaseProduct.Sum(product => product.ProductPrice * product.Quantity);
            Console.WriteLine($"Total Price: {totalPrice}");

            Console.WriteLine("Proceed? (Type OK to proceed, otherwise type anything beside it to back)");
            string input = Console.ReadLine();
            if (input == "OK")
            {
                foreach (var product in purchaseProduct)
                {
                    var productListService = new ProductListService();
                    var productToUpdate = productListService.GetProductByCode(product.PurchaseProductCode);
                    productToUpdate.Stock -= product.Quantity;
                    productListService.UpdatedCheckOut(productToUpdate);

                    // Add purchase to purchase history
                    var purchase = new PurchaseModel
                    {
                        ProductCode = product.PurchaseProductCode,
                        Name = product.ProductName,
                        Price = product.ProductPrice,
                        Quantity = product.Quantity
                    };
                    purchaseHistory.Add(purchase);
                }
                purchaseProduct.Clear();
                Console.WriteLine("Checkout success!");
            }
            else
            {
                Console.WriteLine("Checkout canceled.");
            }
        }

        public static void ShowPurchaseHistory()
        {
            Console.WriteLine("\nView Purchase History:");
            Console.WriteLine("| Product Code | Name | Price | Quantity | Sub Total |");
            decimal total = 0;
            foreach (var purchase in purchaseHistory)
            {
                Console.WriteLine($"| {purchase.ProductCode} | {purchase.Name} | {purchase.Price} | {purchase.Quantity} | {purchase.SubTotal} |");
                total += purchase.SubTotal;
            }
            Console.WriteLine($"Total: {total}");

            Console.WriteLine("Menu:");
            Console.WriteLine("1. Back");
            Console.WriteLine("Please input your choice: ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                var mainMenu = new MainMenuService();
                mainMenu.MainMenu();
            }
        }


        void DisplayContinueConfirmation()
        {
            Console.WriteLine("Please press any key to go back.");
            Console.Read();
        }

    }

}
