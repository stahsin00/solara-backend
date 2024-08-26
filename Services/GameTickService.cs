using Microsoft.EntityFrameworkCore;
using Solara.Data;
using Solara.Models;

namespace Solara.Services
{
    public class GameTickService : IHostedService, IDisposable
    {
        private readonly ILogger<GameTickService> _logger;
        private readonly IServiceScopeFactory _scopeFactory; // TODO: look into scoped services
        private readonly UserSocketManager _userSocketManager;
        private readonly RedisCacheService _redis;

        private Timer? _timer = null;

        public GameTickService(ILogger<GameTickService> logger, IServiceScopeFactory scopeFactory, UserSocketManager userSocketManager, RedisCacheService redis)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _userSocketManager = userSocketManager;
            _redis = redis;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        // TODO: prevent overlapping timers, do something about async void
        private async void DoWork(object? state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    var runningGames = await _redis.GetAllHashAsync<Game>();

                    foreach (var game in runningGames)
                    {
                        if (game == null) continue;  // TODO
                        game.LastUpdate = DateTime.UtcNow;
                        game.EnemyCurHealth -= game.DPS;

                        if (game.EnemyCurHealth <= 0)
                        {
                            await _userSocketManager.SendMessageAsync(game.User.Id, "Enemy defeated.");  // TODO send JSON info instead?
                            game.EnemyCurHealth = game.EnemyMaxHealth;
                            game.RewardBalance += 100;  // TODO: centralize hardcoded values
                            game.RewardExp += 1;
                        }

                        game.RemainingTime = game.RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                        if (game.RemainingTime <= TimeSpan.Zero)
                        {
                            // TODO: any other game ending updates
                            var dbGame = await context.Games.FindAsync(game.Id);
                            if (dbGame != null)
                            {
                                dbGame.Running = false;
                                dbGame.LastUpdate = game.LastUpdate;
                                dbGame.RewardBalance = game.RewardBalance;
                                dbGame.RewardExp = game.RewardExp;
                                dbGame.RemainingTime = game.RemainingTime;
                                dbGame.EnemyCurHealth = game.EnemyCurHealth;

                                context.Games.Update(dbGame);
                            }

                            await _redis.RemoveHashAsync<Game>(game.Id.ToString());
                            await _userSocketManager.CloseSocket(game.User.Id, "Time's up.");
                        } else {
                            await _redis.SetHashAsync(game.Id.ToString(), game);  // TODO: batch updates (?)
                        }
                    }
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