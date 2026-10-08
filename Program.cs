using System;

class Program
{
    // Catalog: two parallel arrays (same index = same product)
    static readonly string[] Names = { "Keyboard", "Mouse", "Monitor", "Headset", "Webcam" };
    static readonly decimal[] Prices = { 350m, 150m, 1200m, 400m, 250m };

    static void Main()
    {
        string customer = FormatName();
        PrintCatalog();
        int[] cart = RunShoppingMenu();
        decimal subtotal = CalculateSubtotal(cart);
        decimal rate = GetDiscountRate(subtotal);
        PrintReceipt(customer, cart, subtotal, rate);
    }

    // Step 1: ask for the name, clean it, capitalize the first letter
    static string FormatName()
    {
        Console.Write("Enter your name: ");
        string name = (Console.ReadLine() ?? "").Trim();   // Trim() returns a NEW string, so save it

        if (name.Length == 0)
            return "Guest";

        return $"{char.ToUpper(name[0])}{name.Substring(1).ToLower()}";
    }

    // Step 2: print the catalog with a for loop
    static void PrintCatalog()
    {
        Console.WriteLine("\n--- Catalog ---");
        for (int i = 0; i < Names.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Names[i]} - {Prices[i]:F2}");
        }
    }

    // Step 3 (menu): while(true) loop, returns the cart (quantity of every product)
    static int[] RunShoppingMenu()
    {
        int[] cart = new int[Names.Length];

        while (true)
        {
            Console.WriteLine("\n1 = Add item, 2 = Checkout");
            int choice = ReadValidInt("Choose: ", 1, 2);

            if (choice == 2)
                break;

            int product = ReadValidInt("Product number: ", 1, Names.Length);
            int quantity = ReadValidInt("Quantity: ", 1, 1000);
            cart[product - 1] += quantity;
        }

        return cart;
    }

    // Step 3 (helper): keep asking until the user types a valid int in [min, max]
    static int ReadValidInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Invalid input. Enter a whole number from {min} to {max}.");
        }
    }

    // Step 4: subtotal = sum of price x quantity
    static decimal CalculateSubtotal(int[] cart)
    {
        decimal subtotal = 0m;
        for (int i = 0; i < cart.Length; i++)
        {
            subtotal += Prices[i] * cart[i];
        }
        return subtotal;
    }

    // Step 4: discount rate with a switch expression
    static decimal GetDiscountRate(decimal subtotal)
    {
        return subtotal switch
        {
            >= 500m => 0.10m,
            >= 200m => 0.05m,
            _ => 0m
        };
    }

    // Step 5: print the receipt
    static void PrintReceipt(string customer, int[] cart, decimal subtotal, decimal rate)
    {
        decimal discount = Math.Round(subtotal * rate, 2);
        decimal total = subtotal - discount;
        string discountLabel = $"Discount ({rate * 100:F0}%):";

        Console.WriteLine($"\nCustomer: {customer}");

        int i = 0;
        foreach (int quantity in cart)
        {
            if (quantity > 0)
                Console.WriteLine($"{Names[i],-8} x{quantity} = {Prices[i] * quantity:F2}");
            i++;
        }

        Console.WriteLine($"{"Subtotal:",-17}{subtotal,10:F2}");
        Console.WriteLine($"{discountLabel,-17}{-discount,10:F2}");
        Console.WriteLine($"{"Total:",-17}{total,10:F2}");
        Console.WriteLine(total >= 500m ? "FREE SHIPPING" : "Shipping: 50.00");
    }
}
