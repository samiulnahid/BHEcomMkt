using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Services.Interfaces;
using BHEcom.Services.Implementations;
using System.ComponentModel.DataAnnotations;


namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ILogger<OrderRepository> _logger;
        public OrdersController(IOrderService orderService, ICartService cartService, IOrderDetailService orderDetailService, IProductService productService, ILogger<OrderRepository> logger)
        {
            _orderService = orderService;
            _cartService = cartService;
            _orderDetailService = orderDetailService;
            _productService = productService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Order order)
        {
            try
            {
                await _orderService.AddOrderAsync(order);
                return Ok(new { id = order.OrderID, Success = true, Message = "Successfully create order & order details." });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }


        [HttpPost("CreateOrderXXXX")]
        public async Task<ActionResult> CreateOrder2([FromBody] Order model)
        {
            try
            {
                if (model == null || model.CartID == Guid.Empty)
                    return BadRequest(new { Success = false, Message = "Invalid data." });

                List<OrderDetail> DetailsList = new List<OrderDetail>();
                var cartManager = await _cartService.GetCartManagerByCartIdAsync(model.CartID);

                if (cartManager == null || !cartManager.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Invalid CartId or Cart is empty!"
                    });
                }

                var coupon = await _orderService.GetCouponById(model.CouponId);
                var allAmount = cartManager.Sum(item =>item.TotalPrice ?? 0);
                model.TotalAmount = allAmount;

                var unavailableProductList = new List<object>(); 

                foreach (var item in cartManager)
                {
                    if (item.ProductID == Guid.Empty)
                        return NotFound(new
                        {
                            Success = false,
                            Message = $"{item.ProductName} not exist!"
                        });

                    var product = await _productService.GetProductByIdAsync(item.ProductID);

                    if (product.Stock < item.Quantity)
                    {
                        var unavailableProduct = new
                        {
                            ProductId = product.ProductID,
                            CartItemId = item.CartItemID,
                            Stock = product.Stock
                        };

                        unavailableProductList.Add(unavailableProduct);
                        continue; // Skip adding this product to the order
                    }

                    OrderDetail detail = new OrderDetail()
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price ?? 0,
                    };

                    DetailsList.Add(detail);
                }

                // Check if there are any unavailable products
                if (unavailableProductList.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Some products are unavailable.",
                        UnavailableProducts = unavailableProductList
                    });
                }

                 // Check if there are any unavailable products
                if (!DetailsList.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Cart is Empty! "
                    });
                }

                model.OrderDate = DateTime.Now; 
                
                var (orderId, orderNumber) = await _orderService.AddOrderAsync(model);
                if(orderId == Guid.Empty)
                    return StatusCode(500, new { message = "Order Creation Failed!", success = false });

                foreach (var item in DetailsList)
                {
                    item.OrderID = orderId;
                    Guid detailId = await _orderDetailService.AddOrderDetailAsync(item);
                    
                    if (detailId == Guid.Empty)
                    {
                        var data = new
                        {
                            Success = false,
                            Message = "Order Details Creation Failed!"
                        };
                        return StatusCode(500, data);
                    }
                       
                }

                bool isDeleted = await _cartService.DeleteFullCartAsync(model.CartID);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Cart deletion failed!", success = false });

                return Ok(new { id = orderId, Success = true, Message = "Successfully create order & order details." });


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult> CreateOrder([FromBody] Order model)
        {
            try
            {
                if (model == null || model.CartID == Guid.Empty)
                    return BadRequest(new { Success = false, Message = "Invalid data." });

                List<OrderDetail> DetailsList = new List<OrderDetail>();
                var cartManager = await _cartService.GetCartManagerByCartIdAsync(model.CartID);

                if (cartManager == null || !cartManager.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Invalid CartId or Cart is empty!"
                    });
                }

                // Calculate total cart amount
                var allAmount = cartManager.Sum(item => item.TotalPrice ?? 0);
                model.TotalAmount = allAmount;

                var unavailableProductList = new List<object>();
                Coupon coupon = new Coupon();   

                decimal applicableStoreAmount = 0; // Total amount for applicable store products


                if (model.CouponId != null && model.CouponId != Guid.Empty)
                {
                    coupon = await _orderService.GetCouponById(model.CouponId);
                }

                foreach (var item in cartManager)
                {
                    if (item.ProductID == Guid.Empty)
                        return NotFound(new
                        {
                            Success = false,
                            Message = $"{item.ProductName} not exist!"
                        });

                    var product = await _productService.GetProductByIdAsync(item.ProductID);

                    if (product.Stock < item.Quantity)
                    {
                        var unavailableProduct = new
                        {
                            ProductId = product.ProductID,
                            CartItemId = item.CartItemID,
                            Stock = product.Stock
                        };

                        unavailableProductList.Add(unavailableProduct);
                        continue; // Skip adding this product to the order
                    }

                    // Check if the product belongs to the coupon's StoreId
                    if (coupon != null && coupon.StoreId != null && coupon.StoreId != Guid.Empty )
                    {
                        if(product.StoreID == coupon.StoreId)
                            applicableStoreAmount += item.TotalPrice ?? 0;
                    }
                    else if(coupon != null && (coupon.StoreId == null || coupon.StoreId == Guid.Empty))
                    {
                        applicableStoreAmount += item.TotalPrice ?? 0;
                    }

                    OrderDetail detail = new OrderDetail()
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price ?? 0,
                    };

                    DetailsList.Add(detail);
                }

                // Check if there are any unavailable products
                if (unavailableProductList.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Some products are unavailable.",
                        UnavailableProducts = unavailableProductList
                    });
                }

                if (!DetailsList.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Cart is Empty!"
                    });
                }

                // Apply coupon if provided
                if (model.CouponId != null && model.CouponId != Guid.Empty)
                {
                    if (coupon != null)
                    {
                        if (coupon.IsActive == null || coupon.IsActive == false || coupon.ExpirationDate <= DateTime.Now)
                        {
                            return Ok(new
                            {
                                Success = false,
                                Message = "The coupon is either inactive or expired."
                            });
                        }

                        if (model.TotalAmount < coupon.MinimumOrderAmount)
                        {
                            return Ok(new
                            {
                                Success = false,
                                Message = $"The minimum order amount for this coupon is {coupon.MinimumOrderAmount}."
                            });
                        }

                        var discountAmount = coupon.DiscountPercentage.HasValue && coupon.DiscountPercentage > 0
    ? (applicableStoreAmount * coupon.DiscountPercentage.Value) / 100
    : (coupon.DiscountAmount.HasValue ? Math.Min(coupon.DiscountAmount.Value, applicableStoreAmount) : 0);


                        model.DiscountAmount = Math.Min(discountAmount, model.TotalAmount); // Ensure discount doesn't exceed total
                        model.TotalAfterDiscount = model.TotalAmount - model.DiscountAmount;
                    }
                    else
                    {
                        return Ok(new { Success = false, Message = "Invalid coupon." });
                    }
                }
                else
                {
                    model.DiscountAmount = 0;
                    model.TotalAfterDiscount = model.TotalAmount;
                }

                model.OrderDate = DateTime.Now;

                // add Total Delivery Amount

                model.TotalAmount = model.TotalAmount + model.TotalDeliveryFee;    
                model.TotalAfterDiscount = model.TotalAfterDiscount + model.TotalDeliveryFee;    

                var (orderId, orderNumber) = await _orderService.AddOrderAsync(model);

                if (orderId == Guid.Empty)
                    return StatusCode(500, new { message = "Order Creation Failed!", success = false });

                foreach (var item in DetailsList)
                {
                    item.OrderID = orderId;
                    Guid detailId = await _orderDetailService.AddOrderDetailAsync(item);

                    if (detailId == Guid.Empty)
                    {
                        return StatusCode(500, new { Success = false, Message = "Order Details Creation Failed!" });
                    }
                }

                bool isDeleted = await _cartService.DeleteFullCartAsync(model.CartID);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Cart deletion failed!", success = false });

                var data = new { 
                    OrderID = orderId,
                    OrderNumber = orderNumber,
                };
                return Ok(new { data, Success = true, Message = "Successfully created order & order details." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return Ok(new { data = order,Message="Not Found!", Success = true });
                }
                return Ok(new { data = order, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(new { data = orders, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }
        

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] string status)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }

                bool isUpdated = await _orderService.UpdateOrderAsync(id, status);
                if (!isUpdated)
                    return Ok(new { Success = false, Message = "Order status update unsuccessful." });

                return Ok(new { Success = true, Message = "Successfully updated order status." });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                // Delete all order details by OrderId
                await _orderDetailService.DeleteDetailsByOrderIdAsync(id);

                // Delete the order itself
                await _orderService.DeleteOrderAsync(id);

                return Ok(new { Success = true, Message = "Order and its details deleted successfully." });
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
           
        }

        [HttpPost("CreateAndUpdateCoupon")]
        public async Task<ActionResult> CreateAndUpdateCoupon([FromBody] Coupon coupon)
        {
            try
            {
                if (coupon.CouponId == Guid.Empty)
                {
                    coupon.CreatedAt = DateTime.UtcNow;
                    Guid id = await _orderService.AddCouponAsync(coupon);
                    return Ok(new { id = coupon.CouponId, Success = true, Message = "Successfully Create Coupon." });
                }
                else
                {
                    bool isUpdate = await _orderService.UpdateCouponAsync(coupon);
                    if (!isUpdate)
                        return Ok(new { id = coupon.CouponId, Success = true, Message = "Invalid Data! Update Unsuccessful!" });
                    return Ok(new { id = coupon.CouponId, Success = true, Message = "Successfully Update Coupon." });
                }

                
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding & Updating a coupon.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpGet("CouponValidation")]
        public async Task<ActionResult<IEnumerable<Order>>> CouponValidation(string code)
        {
            try
            {
                var (coupon, result) = await _orderService.ValidateCouponAsync(code);
                return Ok(new { data = coupon, result, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }


        [HttpGet("GetAllByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<OrderManager>>> GetAllByUserId(Guid id)
        {
            try
            {
                var orders = await _orderService.GetAllByUserIdAsync(id);
                return Ok(new { data = orders, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all order.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

    }

}
