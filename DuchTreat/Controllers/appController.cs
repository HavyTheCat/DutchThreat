using DuchTreat.Data;
using DuchTreat.Services;
using DuchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DuchTreat.Controllers
{
    public class appController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly IMailService _mailService;
  

        public appController(IMailService mailService, IDutchRepository repository)
        {
            _repository = repository;
            _mailService = mailService;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }
        [HttpGet("contact")]
        public IActionResult Contact()
        {
           

    
            return View();
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
             _mailService.SendMessage("test@test", 
                 model.Subject, 
                 $"From:{model.Name} - {model.Email}" +
                 $", Message:" +
                 $" {model.Message}");

                ViewBag.UserMessage = "Норм";
                ModelState.Clear();
            }
            return View();
        }
        
        public IActionResult Shop()
        {
          return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }
    }
}
