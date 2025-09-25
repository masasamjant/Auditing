using Masasamjant.Auditing.Abstractions;
using Masasamjant.Auditing.Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Masasamjant.Auditing.Demo.Controllers
{
    public class AuditingController : Controller
    {
        private readonly IAuditingEventSource eventSource;

        public AuditingController(IAuditingEventSource eventSource)
        {
            this.eventSource = eventSource;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AuditingEventSearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromForm] AuditingEventSearchViewModel form)
        {
            foreach (var item in form.AuditingActionSelections.Where(x => x.Selected))
                form.Request.Actions.Add(Enum.Parse<AuditingActionType>(item.Value));

            foreach (var item in form.AuditingActionResultSelections.Where(x => x.Selected))
                form.Request.Results.Add(Enum.Parse<AuditingActionResult>(item.Value));

            var events = await eventSource.SearchEventsAsync(form.Request);
            
            if (events.Any())
                form.Result.AddRange(events);

            return View(form);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(Guid identifier)
        {
            var auditingEvent = await eventSource.FindEventAsync(identifier);

            if (auditingEvent == null)
                return RedirectToAction("Index");

            return View(auditingEvent);
        }
    }
}
