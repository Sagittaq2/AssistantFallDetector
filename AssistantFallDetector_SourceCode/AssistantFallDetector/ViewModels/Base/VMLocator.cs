using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AssistantFallDetector.Services;

namespace AssistantFallDetector.ViewModels.Base
{
    public class VMLocator
    {
        IContainer container;

        public VMLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<AccelerometerService>().As<IAccelerometerService>();
            builder.RegisterType<GpsService>().As<IGpsService>();
            builder.RegisterType<SmsService>().As<ISmsService>();
            builder.RegisterType<DispatcherService>().As<IDispatcherService>();
            builder.RegisterType<VMMainPage>();
            builder.RegisterType<VMContactDetailsPage>();

            container = builder.Build();
        }

        public VMMainPage MainViewModel
        {
            get { return container.Resolve<VMMainPage>(); }
        }

    }
}
