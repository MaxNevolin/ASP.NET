using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplom_project.Models
{
    public class ShoppingCart
    {
        public const string CartSessionKey = "idCart";
        string idShoppingCart { get; set; }
        public static List<CartItem> cartList = new List<CartItem>();
        
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.idShoppingCart = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and album instances
            var cartItem = cartList.SingleOrDefault(
c => c.idCart == idShoppingCart
&& c.idProduct == product.idProduct);

            if (cartItem == null)
            {
                int index;
                if(cartList.Count==0)
                    index=1;
                else
                    index=cartList.Max(p => p.idRecord)+1;
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    idRecord = index ,
                    idCart = idShoppingCart,
                    idProduct =product.idProduct,
                    amount = 1,
                    price = (decimal)product.price,
                    nameProduct = product.nameProduct
                };

                cartList.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.amount++;
            }
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = cartList.Single(
c => c.idCart == idShoppingCart
&& c.idRecord == id);
            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.amount > 1)
                {
                    cartItem.amount--;
                    itemCount = cartItem.amount;
                }
                else
                {
                    cartList.Remove(cartItem);
                }
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = cartList.Where(cart => cart.idCart == idShoppingCart);

            foreach (var cartItem in cartItems)
            {
                cartList.Remove(cartItem);
            }
        }

        public List<CartItem> GetCartItems()
        {
            return cartList.Where(cart => cart.idCart == idShoppingCart).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in cartList
                          where cartItems.idCart == idShoppingCart
                          select (int?)cartItems.amount).Sum();

            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal(decimal discount)
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total         

            decimal? total = (from cartItems in cartList
                              where cartItems.idCart == idShoppingCart
                              select (int?)cartItems.amount * cartItems.price).Sum();
            total = total - (total * discount);
            return total ?? decimal.Zero;
        }

      /*  public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);

                storeDB.OrderDetails.Add(orderDetail);

            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            storeDB.SaveChanges();

            // Empty the shopping cart
            EmptyCart();

            // Return the OrderId as the confirmation number
            return order.OrderId;
        }*/

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = cartList.Where(c => c.idCart == idShoppingCart);

            foreach (CartItem item in shoppingCart)
            {
                item.idCart = userName;
            }           
        }

    }
}