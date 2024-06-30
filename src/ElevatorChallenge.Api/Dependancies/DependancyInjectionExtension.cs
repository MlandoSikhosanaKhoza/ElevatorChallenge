namespace ElevatorChallenge.Api.Dependancies
{
    public static class DependancyInjectionExtension
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection Services)
        {
            return Services;
        }
    }
}
