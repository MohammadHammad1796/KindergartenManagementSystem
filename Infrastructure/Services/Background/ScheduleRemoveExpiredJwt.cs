using KindergartenManagementSystem.Core.Services;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace KindergartenManagementSystem.Infrastructure.Services.Background
{
    public class ScheduleRemoveExpiredJwt : BackgroundService
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private static string ScheduleTime => "0 0 2 * * *"; //Runs every day at 2 am
        private readonly IServiceProvider _serviceProvider;

        public ScheduleRemoveExpiredJwt(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _crontabSchedule = CrontabSchedule.Parse(ScheduleTime,
                new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = DateTime.Now;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                    await RemoveExpiredTokensAsync();
                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(0, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task RemoveExpiredTokensAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var signInManager = scope.ServiceProvider.GetService<ISignInManager>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                await signInManager.RemoveExpiredTokensAsync();
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
