using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceBackgroundTasks
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue,
            ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
        {
            TaskQueue = taskQueue;
            _logger = loggerFactory.CreateLogger<QueuedHostedService>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        protected async override Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(cancellationToken);

                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        await workItem(cancellationToken, scope.ServiceProvider);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       $"Error occurred executing {nameof(workItem)}.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
}
