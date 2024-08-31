using Microsoft.EntityFrameworkCore;
using Solara.Data;
using Solara.Models;

namespace Solara.Services
{
    public class GameManager : IDisposable
    {
        private readonly ILogger<GameTickService> _logger;
        private readonly IServiceScopeFactory _scopeFactory; // TODO: look into scoped services
        private readonly UserSocketManager _userSocketManager;
        private readonly RedisCacheService _redis;

        public GameManager(ILogger<GameTickService> logger, IServiceScopeFactory scopeFactory, UserSocketManager userSocketManager, RedisCacheService redis)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _userSocketManager = userSocketManager;
            _redis = redis;

            GameTickService.OnTick += Update;
        }

        public async Task StartGame(Game game) { 
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    context.Attach(game);  // TODO: assumes this method is called on a valid entity in the database
                    game.Running = true;

                    context.Entry(game).State = EntityState.Modified;

                    await context.SaveChangesAsync();

                    await _redis.SetHashAsync(game.Id.ToString(), game);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while starting game.");
            }
        }

        // TODO: prevent overlapping updates, do something about async void
        private async void Update()
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
                            var dbGame = await context.Games.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == game.Id);
                            if (dbGame != null)
                            {
                                dbGame.Running = false;
                                dbGame.LastUpdate = game.LastUpdate;
                                dbGame.RewardBalance = game.RewardBalance;
                                dbGame.RewardExp = game.RewardExp;
                                dbGame.RemainingTime = game.RemainingTime;
                                dbGame.EnemyCurHealth = game.EnemyCurHealth;

                                dbGame.User.Balance += game.RewardBalance;
                                dbGame.User.Exp += game.RewardExp;

                                await context.SaveChangesAsync();
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
                _logger.LogError(e, "An error occurred during the game update.");
            }
        }

        public async Task StopGame(int gameId) {
            try {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    var cachedGame = await _redis.GetHashAsync<Game>(gameId.ToString());
                    if (cachedGame == null) {
                        _logger.LogError("Attempted to stop a game that is not running.");
                        return;
                    }
                    await _redis.RemoveHashAsync<Game>(gameId.ToString());

                    var game = await context.Games.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == gameId);
                    if (game != null)
                    {
                        game.Running = false;
                        game.LastUpdate = DateTime.UtcNow;
                        game.RewardBalance += cachedGame.RewardBalance;
                        game.RewardExp += cachedGame.RewardExp;

                        await context.SaveChangesAsync();
                    } else {
                        _logger.LogError("Trying to stop a nonexistant game.");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while stopping game.");
            }
        }

        public void Dispose()
        {
            // TODO: consider case where GameTickService is disposed before GameManager
            GameTickService.OnTick -= Update;
        }
    }
}