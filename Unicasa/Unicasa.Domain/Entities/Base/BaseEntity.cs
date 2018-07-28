using System;

namespace Unicasa.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString().ToUpper();
        }

        public string GerarId()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        public string Id { get; set; }
    }

}
