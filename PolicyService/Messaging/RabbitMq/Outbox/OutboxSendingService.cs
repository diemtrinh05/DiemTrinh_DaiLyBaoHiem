using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PolicyService.Messaging.RabbitMq.Outbox;

public class OutboxSendingService : IHostedService
{
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
    private readonly Outbox outbox;
    private readonly ILogger<OutboxSendingService> logger;
    private Timer timer;

    public OutboxSendingService(Outbox outbox, ILogger<OutboxSendingService> logger)
    {
        this.outbox = outbox;
        this.logger = logger;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer
        (
            PushMessages,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1)
        );
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }


    private async void PushMessages(object state)
    {
        if (!await semaphore.WaitAsync(0))
            return;

        try
        {
            outbox.PushPendingMessages().Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Outbox background push failed.");
        }
        finally
        {
            semaphore.Release();
        }
    }
}
