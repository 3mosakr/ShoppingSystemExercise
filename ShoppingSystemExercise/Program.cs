using System;

namespace ShoppingSystemExercise
{
    internal class Program
    {

        static public List<string> cartItems = new List<string>(); // Cart
        static public Dictionary<string, double> itemPrice = new Dictionary<string, double>()
        {
            {"Camera", 1500 },
            {"Laptop", 3000 },
            {"TV", 2500 }
        }; // Stock

        static Stack<string> actions = new Stack<string>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("===============================");
                Console.WriteLine("Welcome to the shopping system");
                Console.WriteLine("===============================");
                Console.WriteLine("1. Add item to cart");
                Console.WriteLine("2. View cart items");
                Console.WriteLine("3. Remove item from cart");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Undo last action");
                Console.WriteLine("6. Exit");

                Console.Write("Enter your choise number: ");

                int intchoise = Convert.ToInt32(Console.ReadLine());

                switch (intchoise)
                {
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        ViewCart();
                        break;
                    case 3:
                        RemoveItem();
                        break;
                    case 4:
                        CheckOut();
                        break;
                    case 5:
                        UndoAction();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid code entered, please try again.");
                        break;
                }
            }
        }

        private static void AddItem()
        {
            // display available items to user
            Console.WriteLine("Available items");
            Console.WriteLine("--------------------------");
            foreach (var item in itemPrice)
            {
                Console.WriteLine($"Item: {item.Key} price: {item.Value}");
            }
            Console.WriteLine("--------------------------");

            // customer will choise the item which need to buy
            Console.Write("Please enter product name: ");
            string cartItem = Console.ReadLine();

            // is the user input valid or not
            // if valid add it to cart
            if (itemPrice.ContainsKey(cartItem))
            {
                cartItems.Add(cartItem);
                // for undo action
                actions.Push($"item {cartItem} added to cart");
                Console.WriteLine($"Item {cartItem} is added to your cart");
            }
            else
            {
                Console.WriteLine("Item is out of stock or not available.");
            }

        }

        private static void ViewCart()
        {

            Console.WriteLine("Your cart items : ");
            Console.WriteLine("--------------------------");
            // valid if cart is not empty
            if (cartItems.Any())
            {
                var itemPriceCollection = GetCartPrices();

                foreach (var item in itemPriceCollection)
                {
                    Console.WriteLine($"Item: {item.Item1}, price: {item.Item2}");
                }
            }
            else
            {
                Console.WriteLine("Cart is empty");
            }
            Console.WriteLine("--------------------------");
        }

        /// <summary>
        /// extract each item price in cart from stock
        /// return IEnumerable because it give me type of abstraction
        /// </summary>
        private static IEnumerable<Tuple<string, double>> GetCartPrices()
        {
            var cartPrices = new List<Tuple<string, double>>();
            // iterate for each item in cart to get its price from stock
            foreach (var item in cartItems)
            {
                double price = 0;
                bool foundItem = itemPrice.TryGetValue(item, out price);

                if(foundItem)
                {
                    var itemPrice = new Tuple<string, double>(item, price);
                    cartPrices.Add(itemPrice);

                }
            }
            return cartPrices;
        }

        private static void RemoveItem()
        {
            // valid if cart has items 
            if (cartItems.Any())
            {
                // show cart items
                ViewCart();
                Console.Write("Please select item to remove: ");
                string itemToRemove = Console.ReadLine();
                if (cartItems.Contains(itemToRemove))
                {
                    cartItems.Remove(itemToRemove);
                    // for undo action
                    actions.Push($"item {itemToRemove} removed from cart");
                    Console.WriteLine($"item: {itemToRemove} is removed");

                }
                else
                {
                    Console.WriteLine("Item does not exist in shopping cart");
                }
            }
            else 
            {
                Console.WriteLine("Cart is empty.");
            }
        }

        private static void CheckOut()
        {
            if (cartItems.Any())
            {
                double totalPrice = 0;
                Console.WriteLine("Your cart items are: ");
                Console.WriteLine("--------------------------");
                var itemsInCart = GetCartPrices();

                foreach (var item in itemsInCart)
                {
                    totalPrice += item.Item2;
                    Console.WriteLine($"{item.Item1} ---- {item.Item2}");
                }
                Console.WriteLine("--------------------------");
                Console.WriteLine($"Total price to pay: {totalPrice}");
                Console.WriteLine("Please proceed to payment, Thank you for shopping with us");
                Console.WriteLine("--------------------------");

                // clear the cart
                cartItems.Clear();
                actions.Push("Checkout");
            }
            else 
            {
                Console.WriteLine("Cart is empty");
            }
        }

        private static void UndoAction()
        {
            // valid if there is actions to undo
            if(actions.Count > 0)
            {
                string lastAction = actions.Pop();
                Console.WriteLine($"your last action is {lastAction}");

                var actionArray = lastAction.Split();

                if (lastAction.Contains("added"))
                {
                    cartItems.Remove(actionArray[1]);
                }
                else if (lastAction.Contains("removed"))
                {
                    cartItems.Add(actionArray[1]);
                }
                else 
                {
                    Console.WriteLine("Check out can not be undo, please ask for refund ");
                }
            }
            else
            {
                Console.WriteLine("there is no action to undo.");
            }
        }


    }
}
