using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Services
{
    public interface IDispatcherService
    {
        Task CallDispatcher(Action action);
    }
}
