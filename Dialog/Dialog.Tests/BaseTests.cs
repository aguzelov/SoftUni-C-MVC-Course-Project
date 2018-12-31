using Dialog.Common.Mapping;
using Dialog.ViewModels.Base;
using System.Reflection;
using NUnit.Framework;

namespace Dialog.Tests
{
    public class BaseTests
    {
        public BaseTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }
    }
}