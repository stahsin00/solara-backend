namespace Solara.Services
{
    public class GameTickService : IHostedService, IDisposable
    {
        private readonly ILogger<GameTickService> _logger;

        public delegate void onTick();
        public static onTick? OnTick;

        private Timer? _timer = null;

        public GameTickService(ILogger<GameTickService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            OnTick?.Invoke();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}