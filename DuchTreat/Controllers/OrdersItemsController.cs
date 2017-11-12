using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DuchTreat.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DutchTreat.Data.Entities;
using DuchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DuchTreat.Controllers
{
    [Route("api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersItemsController : Controller
    {
        private readonly ILogger<OrdersItemsController> _logger;
        private readonly IDutchRepository _repository;
        private readonly IMapper _mapper;

        public OrdersItemsController(IDutchRepository repository, 
            ILogger<OrdersItemsController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public IActionResult Get(int orderId)
        {
            var order = _repository.GetOrderById(User.Identity.Name, orderId);
            if (order != null) return Ok(_mapper
                .Map<IEnumerable<OrderItem>,
                IEnumerable<OrderItemViewModel>>
                (order.Items));
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = _repository.GetOrderById(User.Identity.Name, orderId);
            if (order != null) {
                var item = order
                    .Items
                    .Where(i => i.Id == id)
                    .FirstOrDefault();
                if(item != null)
                {
                    return Ok
                        (_mapper.Map<OrderItem, 
                        OrderItemViewModel>
                        (item));
                }

               
            }
            return NotFound();

        }
    }
}