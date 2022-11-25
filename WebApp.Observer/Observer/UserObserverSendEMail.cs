using BaseProject.Models;

namespace WebApp.Observer.Observer
{
    public class UserObserverSendEMail : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObserverSendEMail(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void CreateUser(AppUser appUser)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObserverSendEMail>>();

            // send email logic

            logger.LogInformation("Email sent");
        }
    }
}
