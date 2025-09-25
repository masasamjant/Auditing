using Masasamjant.Auditing.Abstractions;
using Masasamjant.Auditing.Demo.Models;
using System.Collections.Concurrent;

namespace Masasamjant.Auditing.Demo.Services
{
    public class PeopleService : AuditedService
    {
        private static readonly ConcurrentDictionary<Guid, PersonViewModel> persons = new ConcurrentDictionary<Guid, PersonViewModel>();
        private readonly IAuditor auditor;
        private static readonly string applicationName = "Masasamjant Auditing Demo";

        public PeopleService(IAuditor auditor)
        {
            this.auditor = auditor;
            this.auditor.Append(this);
        }

        public async Task<IEnumerable<PersonViewModel>> GetPersonsAsync()
        {
            var result = persons.Values.ToList();
            
            if (result.Count > 0)
            {
                var auditingEvent = AuditingEvent.CreateForMany(result, applicationName, AuditingActionType.View, AuditingActionResult.Succeeded, AuditingUser.AnonymousUser());
                await OnAuditAsync(auditingEvent);
            }

            return result;
        }

        public async Task<PersonViewModel?> GetPersonAsync(Guid identifier)
        {
            if (!persons.TryGetValue(identifier, out var person))
                return null;
            var auditingEvent = AuditingEvent.CreateForOne(person, applicationName, AuditingActionType.View, AuditingActionResult.Succeeded, AuditingUser.AnonymousUser());
            await OnAuditAsync(auditingEvent);
            return person;
        }

        public async Task<PersonViewModel?> AddAsync(PersonViewModel person)
        {
            if (persons.TryAdd(person.Identifier, person))
            { 
                var auditingEvent = AuditingEvent.CreateForOne(person, applicationName, AuditingActionType.Add, AuditingActionResult.Succeeded, AuditingUser.AnonymousUser());
                await OnAuditAsync(auditingEvent);
                return person;
            }

            return null;
        }

        public async Task<PersonViewModel?> UpdateAsync(PersonViewModel person)
        {
            if (!persons.TryRemove(person.Identifier, out var current))
                return null;

            if (persons.TryAdd(person.Identifier, person))
            {
                var auditingEvent = AuditingEvent.CreateForOne(person, applicationName, AuditingActionType.Update, AuditingActionResult.Succeeded, AuditingUser.AnonymousUser());
                await OnAuditAsync(auditingEvent);
                return person;
            }

            return null;
        }

        public async Task RemoveAsync(Guid identifier)
        {
            if (persons.TryRemove(identifier, out var person))
            {
                var auditingEvent = AuditingEvent.CreateForOne(person, applicationName, AuditingActionType.Remove, AuditingActionResult.Succeeded, AuditingUser.AnonymousUser());
                await OnAuditAsync(auditingEvent);
            }
        }
    }
}
