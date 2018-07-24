using System;

namespace Unicasa.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity(string id)
        {
            Id = Guid.NewGuid().ToString().ToUpper();
        }

        public string Id { get; private set; }
    }
}
