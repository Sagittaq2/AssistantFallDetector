using System;
using System.Threading.Tasks;

namespace AssistantFallDetector.Services
{
    public interface IDispatcherService
    {
        Task CallDispatcher(Action action);
    }
}
