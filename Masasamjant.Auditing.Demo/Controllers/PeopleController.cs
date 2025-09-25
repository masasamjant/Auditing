using Masasamjant.Auditing.Abstractions;
using Masasamjant.Auditing.Demo.Models;
using Masasamjant.Auditing.Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Masasamjant.Auditing.Demo.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PeopleService service;

        public PeopleController(IAuditor auditor)
        {
            service = new PeopleService(auditor);
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var people = await service.GetPersonsAsync();
            return View(people);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new PersonViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] PersonViewModel form)
        {
            var person = await service.AddAsync(form);
            if (person == null)
                return RedirectToAction("Index");
            return RedirectToAction("Edit", new { identifier = person.Identifier });
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid identifier)
        {
            var person = await service.GetPersonAsync(identifier);
            if (person == null)
                return RedirectToAction("Index");
            return View(person);

        }

        [HttpPost]
        public async Task<IActionResult> EditAsync([FromForm] PersonViewModel form)
        {
            var person = await service.UpdateAsync(form);
            if (person == null)
                return RedirectToAction("Index");
            return RedirectToAction("Edit", new { identifier = person.Identifier });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAsync([FromForm] Guid identifier)
        {
            await service.RemoveAsync(identifier);
            return RedirectToAction("Index");
        }
    }
}
