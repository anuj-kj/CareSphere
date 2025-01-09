using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Events
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly ConcurrentDictionary<Type, List<Action<IDomainEvent>>> _handlers = new ConcurrentDictionary<Type, List<Action<IDomainEvent>>>();

        public void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
        {
            var eventType = typeof(TEvent);
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new List<Action<IDomainEvent>>();
            }
            _handlers[eventType].Add(e => handler((TEvent)e));
        }

        public void Publish(IDomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();
            if (_handlers.ContainsKey(eventType))
            {
                foreach (var handler in _handlers[eventType])
                {
                    handler(domainEvent);
                }
            }
        }
    }
}
