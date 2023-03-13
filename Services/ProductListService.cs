using CashOut.Models;

namespace CashOut.Services
{
    public class ProductListService
    {
        private static List<ProductListModel> productList = new List<ProductListModel>
        {

            new ProductListModel
            {
                ProductListId = new Guid("34cb21ba-ff13-4948-aa38-03fb7d7804cb"),
                ProductCode="NS",
                Name="Nintendo Switch",
                Price=3500000,
                Stock=10
            },
            new ProductListModel
            {
                ProductListId = new Guid("18a51419-89fa-4c63-8ec1-7af6c34ffab6"),
                ProductCode="PS4",
                Name="PS4 Black",
                Price=4000000,
                Stock=5
            }
            // tambahkan data lainnya di sini
        };

        public void DisplayProductList()
        {
            Console.WriteLine("| Product Code | Name | Price | Stock |");
            foreach (var product in productList)
            {
                Console.WriteLine($"| {product.ProductCode} | {product.Name} | {product.Price} | {product.Stock} |");
            }
        }

        public void RunDisplayProductList()
        {
            var ProductListInput = string.Empty;
            while (ProductListInput != "4")
            {
                Console.Clear();
                Console.WriteLine("Product List:");
                DisplayProductList();
                Console.WriteLine("Product List menus:");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Back");
                Console.WriteLine("Please input your choice: ");

                string ProductInput = Console.ReadLine();
                int.TryParse(ProductInput, out int choice);

                switch (choice)
                {
                    case 1:
                        AddProduct();
                        DisplayContinueConfirmation();
                        break;
                    case 2:
                        UpdateProduct();
                        DisplayContinueConfirmation();
                        break;
                    case 3:
                        DeleteProduct();
                        DisplayContinueConfirmation();
                        break;
                    case 4:
                        var mainMenu = new MainMenuService();
                        mainMenu.MainMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again");
                        break;
                }
            }
        }

        public void AddProduct()
        {
            string productCode = string.Empty;
            string productName = string.Empty;
            int productStock = 0;
            decimal productPrice = 0;

            // Input validation
            while (true)
            {
                Console.WriteLine("Please enter your Product Code: ");
                productCode = Console.ReadLine().Trim();

                if (productCode.Length < 2)
                {
                    Console.WriteLine("Product code must be at least 2 characters.");
                }
                else if (productList.Any(p => p.ProductCode == productCode))
                {
                    Console.WriteLine("The same Product Code was found. Please use another Product Code.");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Please enter your Product Name: ");
                productName = Console.ReadLine().Trim();

                if (productName.Length < 2)
                {
                    Console.WriteLine("Product name must be at least 2 characters.");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Please enter your Product Stock: ");
                var productStockInput = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(productStockInput))
                {
                    Console.WriteLine("Product stock must not be empty.");
                }
                else if (!int.TryParse(productStockInput, out productStock))
                {
                    Console.WriteLine("Product stock must be a valid integer.");
                }
                else if (productStock < 0)
                {
                    Console.WriteLine("Product stock cannot be less than 0.");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Please enter your Product Price: ");
                var productPriceInput = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(productPriceInput))
                {
                    Console.WriteLine("Product price must not be empty.");
                }
                else if (!decimal.TryParse(productPriceInput, out productPrice))
                {
                    Console.WriteLine("Product price must be a valid decimal.");
                }
                else if (productPrice < 100)
                {
                    Console.WriteLine("Product price cannot be less than 100.");
                }
                else
                {
                    break;
                }
            }

            // Add new product
            var newProduct = new ProductListModel
            {
                ProductListId = Guid.NewGuid(),
                ProductCode = productCode,
                Name = productName,
                Stock = productStock,
                Price = Convert.ToInt32(productPrice)
            };

            productList.Add(newProduct);
            Console.WriteLine("New product has been added.");
        }

        public void UpdateProduct()
        {
            Console.WriteLine("Please enter your Product Code you want Update (type exit to return):");
            string productCode = Console.ReadLine();

            if (productCode == "exit")
            {
                RunDisplayProductList();
            }

            var productToUpdate = productList.FirstOrDefault(p => p.ProductCode == productCode);

            if (productToUpdate == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            Console.WriteLine($"Please type your new Product Code to Update ({productToUpdate.ProductCode}): ");
            string newProductCode = Console.ReadLine();
            Console.WriteLine($"Please type your new Product Name to Update  ({productToUpdate.Name}): ");
            string newName = Console.ReadLine();
            Console.WriteLine($"Please type your new Product Price to Update  ({productToUpdate.Price}): ");
            int newPrice;
            bool priceParsed = int.TryParse(Console.ReadLine(), out newPrice);
            if (!priceParsed)
            {
                Console.WriteLine("Invalid input for price. Stock will not be updated.");
                return;
            }
            Console.WriteLine($"Please type your new Product Stock to Update  ({productToUpdate.Stock}): ");
            int newStock;
            bool stockParsed = int.TryParse(Console.ReadLine(), out newStock);
            if (!stockParsed)
            {
                Console.WriteLine("Invalid input for stock. Stock will not be updated.");
                return;
            }



            productToUpdate.ProductCode = newProductCode;
            productToUpdate.Name = newName;
            productToUpdate.Price = newPrice;
            productToUpdate.Stock = newStock;


            Console.WriteLine("Product updated successfully.");
        }

        public void UpdatedCheckOut(ProductListModel productToUpdate)
        {
            var productIndex = productList.FindIndex(p => p.ProductCode == productToUpdate.ProductCode);
            if (productIndex >= 0)
            {
                productList[productIndex] = productToUpdate;
                Console.WriteLine("Product updated.");
            }
            else
            {
                Console.WriteLine($"Product with code {productToUpdate.ProductCode} not found.");
            }
        }

        void DeleteProduct()
        {
            Console.WriteLine("Enter the product code you want to delete (type exit to return): ");
            string productCode = Console.ReadLine();

            if (productCode == "exit")
            {
                RunDisplayProductList();
            }
            else
            {
                var productToDelete = productList.FirstOrDefault(p => p.ProductCode == productCode);
                if (productToDelete == null)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                productList.Remove(productToDelete);

                Console.WriteLine("Product deleted successfully.");
            }

        }

        public ProductListModel GetProductByCode(string productCode)
        {
            return productList.FirstOrDefault(p => p.ProductCode == productCode);
        }


        void DisplayContinueConfirmation()
        {
            Console.WriteLine("Please press any key to go back.");
            Console.Read();
        }

    }
}
