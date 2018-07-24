using System;
using System.Collections.Generic;

namespace Unicasa.Domain.Interfaces.Services.Base
{
    public interface IBaseService : IDisposable
    {
        List<string> Notifications();
    }
}
