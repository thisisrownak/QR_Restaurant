using Microsoft.Extensions.Localization;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Helper
{
    public class LocService
    {
        private readonly IStringLocalizer _localizer;

        public LocService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString GetLocalizedValue(string key)
        {
            return _localizer[key];
        }
    }

    public class SharedResource
    {
    }

    public class ValidationLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public ValidationLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("ValidationResource", assemblyName.Name);
        }

        public LocalizedString GetLocalizedValue(string key)
        {
            return _localizer[key];
        }
    }
}
