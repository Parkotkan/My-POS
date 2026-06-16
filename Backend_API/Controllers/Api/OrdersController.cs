using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_API.Data;
using Backend_API.Models;

namespace Backend_API.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Order_items)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Order_items)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (order == null)
                {
                    return BadRequest("Order data is required");
                }

                if (order.UserId == 0)
                {
                    return BadRequest("UserId is required");
                }

                if (order.Order_items == null || !order.Order_items.Any())
                {
                    return BadRequest("Order must contain at least one item");
                }

                // Prepare order data
                order.OrderDate = DateTime.Now;
                order.CreatedDate = DateTime.Now;
                order.Status = "Completed";

                decimal calculatedTotal = 0;
                foreach (var item in order.Order_items)
                {
                    item.CreatedDate = DateTime.Now;
                    item.TotalPrice = item.Quantity * item.UnitPrice;
                    calculatedTotal += item.TotalPrice;
                }

                // Use the calculated total if it differs or is zero
                if (order.TotalAmount == 0)
                {
                    order.TotalAmount = calculatedTotal;
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Load the created order with all relations for the response
                var result = await _context.Orders
                    .Include(o => o.Order_items)
                        .ThenInclude(oi => oi.Product)
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (order == null || id != order.OrderId)
            {
                return BadRequest();
            }

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.UserId = order.UserId;
            existingOrder.Status = order.Status;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
