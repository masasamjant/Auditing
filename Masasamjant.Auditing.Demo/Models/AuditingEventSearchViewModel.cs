using Microsoft.AspNetCore.Mvc.Rendering;

namespace Masasamjant.Auditing.Demo.Models
{
    public class AuditingEventSearchViewModel
    {
        public AuditingEventSearchViewModel()
        {
            Request = new AuditingEventSearchRequest();
            Result = new List<AuditingEvent>();
            AuditingActionResultSelections = Enum.GetValues<AuditingActionResult>()
                .Select(x => new SelectListItem(x.ToString(), x.ToString()))
                .ToList();
            AuditingActionSelections = Enum.GetValues<AuditingActionType>()
                .Select(x => new SelectListItem(x.ToString(), x.ToString()))
                .ToList();
        }

        public AuditingEventSearchRequest Request { get; set; }

        public List<AuditingEvent> Result { get; internal set; }

        public List<SelectListItem> AuditingActionResultSelections { get; set; }

        public List<SelectListItem> AuditingActionSelections { get; set; }
    }
}
