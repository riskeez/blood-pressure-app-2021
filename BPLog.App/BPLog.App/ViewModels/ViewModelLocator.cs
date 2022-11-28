using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BPLog.App.ViewModels
{
    public static class ViewModelLocator
    {
        public static TService Resolve<TService>()
           where TService : class
        {
            return Startup.ServiceProvider.GetRequiredService<TService>();
        }
    }
}
