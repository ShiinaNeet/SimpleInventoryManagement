

using SimpleInventoryManagement.DTO;
using SimpleInventoryManagement.Interface;
using SimpleInventoryManagement.Model;
using SimpleInventoryManagement.Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;



public class Program
{
    public static void Main()
    {
        // Inventory Manager
        IInventoryManager inventoryManager = new InventoryManager();

        // Dictionary to hold services offered
        Dictionary<string, int> ServiceOffered = new Dictionary<string, int>
        {
            { "LIST", 1 },
            { "CREATE", 2 },
            { "UPDATE", 3 },
            { "DELETE", 4 },
            { "TOTAL", 5 }
        };

        // Main Banner
        Console.WriteLine(new string('=', Console.WindowWidth));
        Console.WriteLine(new string('=', Console.WindowWidth));
        Console.SetCursorPosition((Console.WindowWidth - "Welcome to my Simple Inventory Console app".Length) / 2, Console.CursorTop);
        Console.WriteLine("Welcome to my Simple Inventory Console app");
        Console.WriteLine(new string('=', Console.WindowWidth));

        // Main Menu Loop. Breaks when user types exit
        while (true)
        {
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine("Choose inventory feature to use:");
            Console.WriteLine("1. List Products");
            Console.WriteLine("2. Create Product");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Inventory Total value");
            Console.Write("You: ");

            string? userInput = Console.ReadLine();
            //Check which service the user has chosen in Main Menu based on the services offered which can be seen in the 'ServiceOffered' Dictionary.
            switch (userInput)
            {
                case var _ when userInput == ServiceOffered["LIST"].ToString():

                    ShowProductListUI(inventoryManager.ListProducts());
                    break;

                case var _ when userInput == ServiceOffered["CREATE"].ToString():
                    Console.WriteLine("All Products in Inventory: ");
                    ShowProductListUI(inventoryManager.ListProducts());
                    // Loop to ask user to create more products. Breaks when user says no
                    while (true)
                    {
                        // Create Product UI
                        var productToSave = CreateProductUI();
                        if(productToSave == null)
                        {
                            // Exit if user types exit. Go to main menu
                            break;
                        }
                        inventoryManager.AddProduct(productToSave);

                        Console.Write("Do you want to add another product? (yes/no): ");
                        string choice = Console.ReadLine()?.ToLower() ?? string.Empty;
                        if (choice != "yes") break;
                    }
                    break;

                case var _ when userInput == ServiceOffered["UPDATE"].ToString():
                    Console.WriteLine("All Products in Inventory: ");
                    ShowProductListUI(inventoryManager.ListProducts());
                    // Loop to ask user to create more products. Breaks when user says no
                    while (true)
                    {
                        var productToUpdate = UpdateProductUI();
                        if (productToUpdate == null)
                        {
                            // Exit if user types exit. Go to main menu
                            break;
                        }
                        inventoryManager.UpdateProduct(productToUpdate);
                        Console.Write("Do you want to update another product? (yes/no): ");
                        string choice = Console.ReadLine()?.ToLower() ?? string.Empty;
                        if (choice != "yes") break;
                    }
                    break;

                case var _ when userInput == ServiceOffered["DELETE"].ToString():
                    Console.WriteLine("All Products in Inventory: ");
                    ShowProductListUI(inventoryManager.ListProducts());
                    while (true)
                    {
                        var productToDelete = DeleteProductUI();
                        if (productToDelete == 0)
                        {
                            break;
                        }
                        inventoryManager.RemoveProduct(productToDelete);
                        Console.Write("Do you want to delete another product? (yes/no): ");
                        string choice = Console.ReadLine()?.ToLower() ?? string.Empty;
                        if (choice != "yes") break;
                    }
                    break;

                case var _ when userInput == ServiceOffered["TOTAL"].ToString():
                    Console.WriteLine(new string('=', Console.WindowWidth));
                    Console.WriteLine($"Total inventory value: {inventoryManager.GetTotalValue()}");
                    break;

                case var _ when userInput!.ToUpper() == "EXIT":
                    Console.WriteLine("Application Exiting...");
                    return;

                default:
                    Console.Clear();
                    break;
            }
        }

    }

    // User Interface Methods

    //Show Product List UI
    public static void ShowProductListUI(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.Clear();
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine("No Products Available");
            return;
        }
        foreach (var product in products)
        {
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine($"Product Id: {product.ProductId}");
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Product Quantity: {product.QuantityInStock}");
            Console.WriteLine($"Product Price: {product.Price}");
            Console.WriteLine(new string('=', Console.WindowWidth));
        }
    }
    //Show Product Create UI
    public static Product CreateProductUI() {
        string productName;
        int productQuantity;
        double productPrice;

        Console.WriteLine("\nEnter Product Details:");

        
        //Each Input are input to check for Validation
       
        do
        {
            Console.Write("Name: ");
            productName = Console.ReadLine()?.Trim() ?? string.Empty;
        } while (string.IsNullOrEmpty(productName));

        while (true)
        {
            Console.Write("Price: ");
            if (double.TryParse(Console.ReadLine(), out productPrice) && productPrice >= 0)
                break;
            Console.WriteLine("Invalid input. Price must be a non-negative number.");
        }

        while (true)
        {
            Console.Write("Quantity: ");
            if (int.TryParse(Console.ReadLine(), out productQuantity) && productQuantity >= 0)
                break;
            Console.WriteLine("Invalid input. Quantity must be a non-negative integer.");
        }

        Product product = new Product();
        product.Name = productName;
        product.Price = productPrice;
        product.QuantityInStock = productQuantity;
        return product;

       
    }
    //Show Product Update UI
    public static UpdateProductRequestDTO UpdateProductUI()
    {
        int productQuantity;
        int productId;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Type 'exit' to return");
        Console.ResetColor();
        Console.WriteLine("Update Product Details:");
        Console.WriteLine(new string('=', Console.WindowWidth));

        //Each Input are input to check for Validation
        while (true)
        {
            Console.Write("ID number: ");
            string input = Console.ReadLine();
            if (input?.ToLower() == "exit") return null;
            if (int.TryParse(input, out productId))
                break;
            Console.WriteLine("Invalid input. Product Id must be a number.");
        }

        while (true)
        {
            Console.Write("Quantity: ");
            string input = Console.ReadLine();
            if (input?.ToLower() == "exit") return null;
            if (int.TryParse(input, out productQuantity) && productQuantity >= 0)
                break;
            Console.WriteLine("Invalid input. Quantity must be a non-negative integer.");
        }

        UpdateProductRequestDTO product = new UpdateProductRequestDTO
        {
            ProductId = productId,
            newQuantity = productQuantity 
        };
        return product;
    }
    //Show Product Delete UI
    public static int DeleteProductUI()
    {
        int productId;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Type 'exit' to return");
        Console.ResetColor();
        Console.WriteLine("Delete Product:");
        Console.WriteLine(new string('=', Console.WindowWidth));

        while (true)
        {
            Console.Write("Enter Product Id : ");
            string input = Console.ReadLine();
            if (input?.ToLower() == "exit") return 0;
            if (int.TryParse(input, out productId))
                break;
            Console.WriteLine("Invalid input. Product Id must be a number.");
        }

       
        return productId;
    }

}

