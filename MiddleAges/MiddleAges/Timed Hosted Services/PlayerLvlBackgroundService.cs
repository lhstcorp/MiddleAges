using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiddleAges.Timed_Hosted_Services
{
    public class PlayerLvlBackgroundService : IHostedService, IDisposable
    {
        ApplicationDbContext _context;

        private readonly IServiceScopeFactory _scopeFactory;

        private Timer? _timer = null;

        public PlayerLvlBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5),
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                List<Player> players = _context.Players.ToList();

                if (players.Count > 0)
                {
                    foreach (var player in players)
                    {
                        CalculateLvl(player);
                    }

                    _context.SaveChanges();
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void CalculateLvl(Player player)
        {
            long playerExp = player.Exp;

            if (player.Exp > 2)
            {
                int newLvl = Convert.ToInt32(Math.Floor(Math.Log(playerExp, 1.4)));

                if (player.Lvl != newLvl)
                {
                    player.Lvl = newLvl;

                    _context.Update(player);
                }                
            }
        }
    }
}
