using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Services
{
    public class DispatcherService : IDispatcherService
    {
        public async Task CallDispatcher(Action action)
        {
            App.Dispatcher.BeginInvoke(() =>
            {
                action();
            });
        }
    }
}
