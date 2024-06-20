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
    public class ProductionBackgroundService : IHostedService, IDisposable
    {
        ApplicationDbContext _context;
        const double _peasantHourSalary = 0.01;
        const double _soldierHourExpense = 0.02;

        private readonly IServiceScopeFactory _scopeFactory;

        private Timer? _timer = null;

        public ProductionBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5),
                TimeSpan.FromHours(1));

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
                        if (player.EndDateTimeProduction > DateTime.UtcNow)
                        {
                            CalculateProductionIncome(player);
                        }

                        CalculatePlayerExpenses(player);
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

        private void CalculateProductionIncome(Player player)
        {
            Unit peasants = _context.Units.FirstOrDefault(u => u.PlayerId == player.Id && u.Type == (int)UnitType.Peasant);
            Land land = _context.Lands.Include(l => l.Country).FirstOrDefault(l => l.LandId == player.ResidenceLand);
            PlayerAttribute playerAttribute = _context.PlayerAttributes.FirstOrDefault(pa => pa.PlayerId == player.Id);

            if (land.ProductionLimit > 0)
            {
                double hourIncome = peasants.Count * _peasantHourSalary * (1 + 0.02 * playerAttribute.Management); // 100% + 2% * Management attribute
                player.Money += hourIncome * (1 - (land.Taxes / 100.00));
                player.MoneyProduced += hourIncome * (1 - (land.Taxes / 100.00));

                player.Exp += Convert.ToInt64(Math.Floor(hourIncome));

                _context.Update(player);

                land.ProductionLimit -= hourIncome;

                if (land.ProductionLimit < 0)
                {
                    land.ProductionLimit = 0;
                }

                _context.Update(land);

                Country country = land.Country;
                country.Money += hourIncome * land.Taxes / 100.00;

                _context.Update(country);
            }
        }

        private void CalculatePlayerExpenses(Player player)
        {
            Unit soldiers = _context.Units.FirstOrDefault(u => u.PlayerId == player.Id && u.Type == (int)UnitType.Soldier);

            if (soldiers.Count > 0)
            {
                double hourExpense = soldiers.Count * _soldierHourExpense;
                player.Money -= hourExpense;                
                player.MoneySpent += hourExpense;

                _context.Update(player);                                
            }
        }
    }
}
