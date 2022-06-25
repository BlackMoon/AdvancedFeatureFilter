using System;
using AdvancedFeatureFilter.Storage;
using Library.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Library
{
    public static class Registration
    {
        public static IServiceCollection AddStrategy(this IServiceCollection services, params Type[] argTypes)
        {
            Type ruleType = typeof(Rule)
            Type storageType = typeof(IStorage<>).MakeGenericType(ruleType);
            Type storageImplType = typeof(MemStorage<>).MakeGenericType(ruleType);
            services.AddSingleton(storageType, storageImplType);
        }
    }
}

