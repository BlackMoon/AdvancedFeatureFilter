using AdvancedFeatureFilter.Storage;
using Library.Extensions;
using Library.Rules;
using Library.Storage;
using Library.Strategy;
using Microsoft.Extensions.DependencyInjection;

namespace Library
{
    public static class Registration
    {
        /// <summary>
        /// Adds a strategy with specific filter-types
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeArgs"></param>
        /// <returns></returns>
        public static IServiceCollection AddStrategy(this IServiceCollection services, params Type[] typeArgs)
        {
            return AddStrategyWithStorage(services, typeof(MemStorage<>), typeArgs);
        }

        /// <summary>
        /// Adds a strategy with specific storage-type & filter-types
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storageImplType"></param>
        /// <param name="typeArgs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IServiceCollection AddStrategyWithStorage(this IServiceCollection services, Type storageImplType, params Type[] typeArgs)
        {
            if (!typeArgs.AnySafe())
            {
                return services;
            }

            Type ruleType;
            Type strategyType;
            Type strategyImplType;

            switch (typeArgs.Length)
            {
                case 1:
                    ruleType = typeof(Rule1Filters<>).MakeGenericType(typeArgs);
                    strategyType = typeof(IStrategy1<>).MakeGenericType(typeArgs);
                    strategyImplType = typeof(EngineStrategy1<>).MakeGenericType(typeArgs);
                    break;

                case 2:
                    ruleType = typeof(Rule2Filters<,>).MakeGenericType(typeArgs);
                    strategyType = typeof(IStrategy2<,>).MakeGenericType(typeArgs);
                    strategyImplType = typeof(EngineStrategy2<,>).MakeGenericType(typeArgs);
                    break;

                case 3:
                    ruleType = typeof(Rule3Filters<,,>).MakeGenericType(typeArgs);
                    strategyType = typeof(IStrategy3<,,>).MakeGenericType(typeArgs);
                    strategyImplType = typeof(EngineStrategy3<,,>).MakeGenericType(typeArgs);
                    break;

                case 4:
                    ruleType = typeof(Rule4Filters<,,,>).MakeGenericType(typeArgs);
                    strategyType = typeof(IStrategy4<,,,>).MakeGenericType(typeArgs);
                    strategyImplType = typeof(EngineStrategy4<,,,>).MakeGenericType(typeArgs);
                    break;

                default:
                    throw new NotImplementedException();
            }

            Type storageType = typeof(IStorage<>).MakeGenericType(ruleType);

            services.AddSingleton(storageType, storageImplType.MakeGenericType(ruleType));
            services.AddSingleton(strategyType, strategyImplType);

            return services;
        }
    }
}

