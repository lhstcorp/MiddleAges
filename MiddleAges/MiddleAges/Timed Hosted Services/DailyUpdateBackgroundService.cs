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
    public class DailyUpdateBackgroundService : IHostedService, IDisposable
    {
        ApplicationDbContext _context;

        private readonly IServiceScopeFactory _scopeFactory;

        private Timer? _timer = null;

        public DailyUpdateBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                UpdateProductionLimits();
                UpdatePlayerMoneyProduced();

                _context.SaveChanges();
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

        public void UpdateProductionLimits()
        {
            List<Land> lands = _context.Lands.ToList();

            if (lands.Count > 0)
            {
                foreach (var land in lands)
                {
                    land.ProductionLimit = 1000;

                    _context.Update(land);
                }
            }
        }

        public void UpdatePlayerMoneyProduced()
        {
            List<Player> players = _context.Players.ToList();

            if (players.Count > 0)
            {
                foreach (var player in players)
                {
                    player.MoneyProduced = 0;

                    _context.Update(player);
                }
            }
        }
    }
}
