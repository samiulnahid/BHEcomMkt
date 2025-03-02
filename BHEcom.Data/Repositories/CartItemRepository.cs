using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartItemRepository> _logger;

        public CartItemRepository(ApplicationDbContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(CartItem cartItem)
        {
            try
            {
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                return cartItem.CartItemID;
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a cartItem.");
                return Guid.Empty;  
            }
        }

        public async Task<CartItem> GetByIdAsync(Guid id)
        {
            return await _context.CartItems.FindAsync(id);
        }
        // public async Task<CartItem> GetByProductIdAsync(Guid id)
        //{
        //    return await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductID == id);
        //}
        public async Task<CartItem> GetByCartandProductIdAsync(Guid cartId, Guid productId)
        {
            var cartItem =  await _context.CartItems
                                          .FirstOrDefaultAsync(ci => ci.CartID == cartId && ci.ProductID == productId);

            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<bool> UpdateAsync(CartItem cartItem)
        {
            try
            {
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a cartItem.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;   
        }

        public async Task<bool> UpdateModifiedFieldsAsync(CartItem cartItem)
        {
            try
            {
                // Fetch the existing CartItem from the database
                var existingCartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemID == cartItem.CartItemID);

                if (existingCartItem == null)
                {
                    _logger.LogWarning($"CartItem with ID {cartItem.CartItemID} not found.");
                    return false; // Return false if the CartItem does not exist
                }

                // Update only the fields that have changed and are not null
                bool isUpdated = false;

                if (cartItem.CartID != Guid.Empty && cartItem.CartID != existingCartItem.CartID)
                {
                    existingCartItem.CartID = cartItem.CartID;
                    isUpdated = true;
                }

                if (cartItem.ProductID != Guid.Empty && cartItem.ProductID != existingCartItem.ProductID)
                {
                    existingCartItem.ProductID = cartItem.ProductID;
                    isUpdated = true;
                }

                if (cartItem.Quantity != existingCartItem.Quantity)
                {
                    existingCartItem.Quantity = cartItem.Quantity;
                    isUpdated = true;
                }

                // Save changes only if any field was updated
                if (isUpdated)
                {
                    _context.CartItems.Update(existingCartItem);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a CartItem.");
                return false;
            }
        }

        //public async Task<bool> UpdateCartItemQuantityAsync(Guid cartItemId, string operation)
        //{
        //    try
        //    {
        //        // Fetch the cart item and associated product
        //        var cartItem = await _context.Set<CartItem>().FirstOrDefaultAsync(ci => ci.CartItemID == cartItemId);
        //        if (cartItem == null)
        //        {
        //            _logger.LogWarning($"CartItem with ID {cartItemId} not found.");
        //            return false;
        //        }

        //        var product = await _context.Set<Product>().FirstOrDefaultAsync(p => p.ProductID == cartItem.ProductID);
        //        if (product == null)
        //        {
        //            _logger.LogWarning($"Product with ID {cartItem.ProductID} not found.");
        //            return false;
        //        }

        //        // Determine the new quantity based on the operation
        //        int newQuantity = operation.ToLower() switch
        //        {
        //            "increase" => cartItem.Quantity + 1,
        //            "decrease" => cartItem.Quantity - 1,
        //            _ => throw new ArgumentException("Invalid operation. Must be 'increase' or 'decrease'.")
        //        };

        //        // Check if the new quantity exceeds stock or becomes invalid
        //        if (newQuantity > product.Stock || newQuantity < 1)
        //        {
        //            _logger.LogWarning($"Invalid operation: Stock {product.Stock}, Requested Quantity {newQuantity}");
        //            return false;
        //        }

        //        // Update the cart item's quantity
        //        cartItem.Quantity = newQuantity;
        //        _context.Set<CartItem>().Update(cartItem);
        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating the CartItem quantity.");
        //        return false;
        //    }
        //}


        public async Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(Guid cartItemId, string operation)
        {
            try
            {
                // Fetch the cart item along with the related product in one query
                var cartItemWithProduct = await _context.Set<CartItem>()
                    .Join(
                        _context.Set<Product>(),
                        cartItem => cartItem.ProductID,
                        product => product.ProductID,
                        (cartItem, product) => new { CartItem = cartItem, Product = product }
                    )
                    .FirstOrDefaultAsync(cp => cp.CartItem.CartItemID == cartItemId);

                if (cartItemWithProduct == null)
                {
                    return (false, "CartItem or associated Product not found.");
                }

                var cartItem = cartItemWithProduct.CartItem;
                var product = cartItemWithProduct.Product;

                // Determine the new quantity based on the operation
                int newQuantity = operation.ToLower() switch
                {
                    "increase" => cartItem.Quantity + 1,
                    "decrease" => cartItem.Quantity - 1,
                    _ => throw new ArgumentException("Invalid operation. Must be 'increase' or 'decrease'.")
                };

                // Validate the operation
                if (newQuantity > product.Stock)
                {
                    return (false, $"Cannot increase quantity. Only {product.Stock} items available in stock.");
                }
                if (newQuantity < 1)
                {
                    return (false, "Cannot decrease quantity below 1.");
                }

                // Update the cart item quantity
                cartItem.Quantity = newQuantity;
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();

                return (true, "CartItem quantity updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the CartItem quantity.");
                return (false, "An unexpected error occurred. Please try again.");
            }
        }

    }

}
