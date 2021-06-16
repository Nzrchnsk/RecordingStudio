using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingStudio.Dto;
using RecordingStudio.Initializers;
using RecordingStudio.Interfaces;
using RecordingStudio.Models;
using RecordingStudio.Static;

namespace RecordingStudio.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservationController : ControllerBase
    {
        private readonly RecordingStudioDbContext _context;
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public ReservationController(RecordingStudioDbContext context, IMapper mapper,
            IAsyncRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Order
        [AllowAnonymous]
        [HttpGet("reservations")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = _context.Orders.Include(o => o.User).Include(o => o.Studio);

            return User.IsInRole(Rolse.Admin)
                ? Ok(await orders.ToListAsync())
                : Ok(await orders.Where(o => o.User.Email == User.Identity.Name).ToListAsync());
        }

        // GET: api/Order/5
        [HttpGet("reservations/{id:int}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Test/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("reservations/{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody]bool isPayment)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                return NotFound();
            }

            try
            {
                order.PaymentStatus = isPayment;
                await _orderRepository.UpdateAsync(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Test
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("reservation")]
        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDto request)
        {
            if (request.FromTime > request.ToTime)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Время начала не может быть больше времени окончания"
                });
            }
            if (!(request.FromTime >= 10 && request.FromTime < 17 && request.ToTime > 11 && request.ToTime <= 18))
                    return new JsonResult(new
                {
                    success = false,
                    message = "Время работы студии с 10:00 до 18:00"
                });
            var localTime = request.Date.ToLocalTime();
            var timeFrom = localTime.StartDay(request.FromTime);
            var timeTo = localTime.StartDay(request.ToTime);
            if (_context.Orders.Any(o => o.StudioId == request.StudioId))
            {
                var exist = _context.Orders.Any(o =>
                    o.StudioId == request.StudioId && o.FromDateTime > timeFrom.StartDay() &&
                    o.FromDateTime < timeFrom.StartDay().AddHours(24) &&
                    !(o.ToDateTime >= timeTo && o.FromDateTime >= timeTo ||
                      o.ToDateTime <= timeFrom && o.FromDateTime <= timeFrom));
                if (exist)
                    return new JsonResult(new
                {
                    success = false,
                    message = "Данное время уже занято"
                });
            }

            var user = _context.Users.First(u => u.Email == User.Identity.Name);
            var order = new Order()
            {
                FromDateTime = timeFrom,
                ToDateTime = timeTo,
                PaymentStatus = false,
                StudioId = request.StudioId,
                UserId = user.Id
            };
            var result = await _orderRepository.AddAsync(order);
            return new JsonResult(new
            {
                success = false,
                message = $"Вы успешно зарезервировали время, ваш номер {result.Id}"
            });
        }

        // DELETE: api/Test/5
        [HttpDelete("reservations/{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteAsync(order);

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}