using ElevatorChallenge.BusinessLogic;

namespace ElevatorChallenge.Api.Dependancies
{
    public static class DependancyInjectionExtension
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection Services)
        {
            Services.AddSingleton<IBuildingStorageProvider, BuildingStorageProvider>();
            
            Services.AddScoped<IBuildingLogic, BuildingLogic>();
            
            return Services;
        }
    }
}
