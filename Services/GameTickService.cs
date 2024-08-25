using Microsoft.EntityFrameworkCore;
using Solara.Data;

namespace Solara.Services
{
    public class GameTickService : IHostedService, IDisposable
    {
        private readonly ILogger<GameTickService> _logger;
        private readonly IServiceScopeFactory _scopeFactory; // TODO: look into scoped services
        private readonly UserSocketManager _userSocketManager;

        private Timer? _timer = null;

        public GameTickService(ILogger<GameTickService> logger, IServiceScopeFactory scopeFactory, UserSocketManager userSocketManager)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _userSocketManager = userSocketManager;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    var runningGames = context.Games.Where(g => g.Running).Include(g => g.User).ToList();  // TODO: cache?

                    foreach (var game in runningGames)
                    {
                        game.EnemyCurHealth -= game.DPS;

                        if (game.EnemyCurHealth <= 0)
                        {
                            await _userSocketManager.SendMessageAsync(game.User.Id, "Enemy defeated.");
                            game.EnemyCurHealth = game.EnemyMaxHealth;
                            game.RewardBalance += 100;  // TODO: hardcoded values
                            game.RewardExp += 1;
                        }

                        game.RemainingTime = game.RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                        if (game.RemainingTime <= TimeSpan.Zero)
                        {
                            game.Running = false;
                            // TODO: other database updates
                            await _userSocketManager.CloseSocket(game.User.Id, "Time's up.");
                        }

                        game.LastUpdate = DateTime.UtcNow;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during the game tick.");
            }
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