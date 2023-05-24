using Gses.Services.Coingecko.ServiceLayer;
using Gses.Services.Mail.ServiceLayer;
using Gses.Services.Rate.ServiceLayer;
using Gses.Services.Subscription.DAL;
using Gses.Services.Subscription.ServiceLayer;

namespace Gses.Infrastructure
{
    public static class ServiceDependenciesExtensions
	{
		public static void AddDependencies(this IServiceCollection services)
		{
			services.AddScoped<IMailService, MailService>();

			services.AddScoped<ICoingeckoHttpClient, CoingeckoHttpClient>();
			services.AddScoped<IRateService, RateService>();

			services.AddScoped<ISubscriptionEmailFileRepository, SubscriptionEmailFileRepository>();
			services.AddScoped<ISubscriptionService, SubscriptionService>();
		}
	}
}
