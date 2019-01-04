using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleCosmosCore2App.Core;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPNETCosmosDBCore.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private Persistence _persistence;

        public HomeController(Persistence persistence)
        {
            _persistence = persistence;
        }

        [HttpGet()]
        public async Task<IActionResult> IndexAsync()
        {
            var contacts = await _persistence.GetSamplesAsync();
            return View("Index", contacts);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _persistence.DeleteItemAsync(id);
            var contacts = _persistence.GetSamplesAsync();
            return RedirectToAction("IndexAsync", contacts);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var contact = new Contact() { };
            return View("Create", contact);
        }

        [HttpPut("Edit")]
        public IActionResult Edit(string id)
        {
            var contact = _persistence.GetSampleAsync(id);
            return View("Edit", id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var contact = await _persistence.GetSampleAsync(id);
            return View("Create", contact);
        }

        [HttpPost()]
        public async Task<IActionResult> PostAsync([FromForm] Contact contact)
        {
            await _persistence.SaveSampleAsync(contact);
            return RedirectToAction("IndexAsync");
        }
    }
}
