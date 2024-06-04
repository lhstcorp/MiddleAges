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
    public class WarBackgroundService : IHostedService, IDisposable
    {
        ApplicationDbContext _context;

        private readonly IServiceScopeFactory _scopeFactory;

        private Timer? _timer = null;

        public WarBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                List<War> wars = _context.Wars.Where(w => w.IsEnded == false && w.StartDateTime.AddHours(24) > DateTime.UtcNow).ToList();

                foreach (var war in wars)
                {
                    CalculateWar(war);
                    _context.Update(war);
                }

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

        private void CalculateWar(War war)
        {
            List<Army> armies = _context.Armies.Include(a => a.Player).Where(a => a.WarId == war.WarId).ToList();
            List<Army> attackersArmies = armies.FindAll(a => a.Side == ArmySide.Attackers);
            List<Army> defendersArmies = armies.FindAll(a => a.Side == ArmySide.Defenders);

            int attackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount);
            int defendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount);

            double attackersPerc = attackersSoldiersCount / (attackersSoldiersCount + defendersSoldiersCount);
            double defendersPerc = 1 - attackersPerc;

            const double lossPerc = 0.02;

            Random rnd = new Random();
            double cubeValueA = rnd.Next(-50, 51) / 100;
            double cubeValueD = rnd.Next(-50, 51) / 100;

            double attackersLossProportionPerc = defendersPerc + defendersPerc * cubeValueA;
            double defendersLossProportionPerc = attackersPerc + attackersPerc * cubeValueD;
        }
    }
}