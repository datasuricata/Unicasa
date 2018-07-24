using System;

namespace Unicasa.Domain.Entities.Base
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
