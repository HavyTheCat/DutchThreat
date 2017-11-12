using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DuchTreat.Data;
using Microsoft.Extensions.Logging;
using DutchTreat.Data.Entities;
using DuchTreat.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using DuchTreat.Data.Entities;
using System.Threading.Tasks;

namespace DuchTreat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDutchRepository __repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository, 
            ILogger<OrdersController> logger, 
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _mapper = mapper;
            __repository = repository;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var user = User.Identity.Name;

                var results = __repository.GetAllOrdersByUser(user, includeItems);

                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get Orders:{ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = __repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Orders:{ex}");
                return BadRequest("Failed to get orders");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
              {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);
                    if(newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;

                    __repository.AddOrder(newOrder);
               if(__repository.SaveAll())
                {
                   
                    return Created($"api/orders/{newOrder.Id}",
                        _mapper.
                        Map<Order,
                        OrderViewModel>
                        (newOrder));
                }

               }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex) 
            {

                _logger.LogError($"Failed to save a new order:{ex}");
            }
            return BadRequest("Failed to save new order");
        }
    }
}