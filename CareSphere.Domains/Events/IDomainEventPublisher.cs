using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Events
{
    public interface IDomainEventPublisher
    {
        void Publish(IDomainEvent domainEvent);
        void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent;
    }
}
