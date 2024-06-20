using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleAges.Data;
using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiddleAges.Timed_Hosted_Services
{
    public class RecruitBackgroundService : IHostedService, IDisposable
    {
        ApplicationDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        private Timer? _timer = null;

        public RecruitBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {          
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(10),
                TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                List<Player> players = _context.Players.ToList();

                foreach (var player in players)
                {
                    AddRecruits(player);                    
                }

                _context.SaveChanges();
            }
        }

        private void AddRecruits(Player player)
        {
            PlayerAttribute playerAttribute = _context.PlayerAttributes.FirstOrDefault(pa => pa.PlayerId == player.Id);

            double recruitMaxCount = 1.00 + 0.02 * Convert.ToDouble(playerAttribute.Leadership);

            Random random = new Random();
            double recruitCount = random.NextDouble() * (recruitMaxCount - 0.01) + 0.01;

            player.RecruitAmount += Convert.ToInt32(Math.Ceiling(recruitCount));
            _context.Update(player);
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
    }
}
