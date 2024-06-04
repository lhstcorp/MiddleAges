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

                List<War> wars = _context.Wars.Where(w => w.IsEnded == false && w.StartDateTime.AddHours(24) < DateTime.UtcNow).ToList();

                foreach (var war in wars)
                {
                    CalculateWar(war);
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

            double attackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount);
            double defendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount);

            if (!CheckEndOfWar(war, attackersSoldiersCount, defendersSoldiersCount))
            {
                double attackersPerc = attackersSoldiersCount / (attackersSoldiersCount + defendersSoldiersCount);
                double defendersPerc = 1 - attackersPerc;

                const double lossPerc = 0.2;

                Random rnd = new Random();
                double cubeValueA = rnd.Next(-50, 51) / 100;
                double cubeValueD = rnd.Next(-50, 51) / 100;

                double attackersLossProportionPerc = defendersPerc + defendersPerc * cubeValueA;
                double defendersLossProportionPerc = attackersPerc + attackersPerc * cubeValueD;

                double deathCount = Math.Ceiling((attackersSoldiersCount + defendersSoldiersCount) * lossPerc);

                foreach (var army in attackersArmies)
                {
                    CalculateArmy(army, attackersSoldiersCount, deathCount, attackersLossProportionPerc, defendersLossProportionPerc);
                }

                foreach (var army in defendersArmies)
                {
                    CalculateArmy(army, defendersSoldiersCount, deathCount, defendersLossProportionPerc, attackersLossProportionPerc);
                }

                TryEndWar(war);
            }
        }

        private void CalculateArmy(Army army, double sideSoldiersCount, double deathCount, double lossProportionPerc, double killProportionPerc)
        {
            double armyProportionPerc = army.SoldiersCount / sideSoldiersCount;
            double soldiersLost = Math.Round(armyProportionPerc * lossProportionPerc * deathCount);
            double soldiersKilled = Math.Round(armyProportionPerc * killProportionPerc * deathCount);

            if (army.SoldiersCount - soldiersLost <= 0)
            {
                soldiersLost = army.SoldiersCount;
                DeleteArmy(army);                
            }
            else
            {
                army.SoldiersCount -= Convert.ToInt32(soldiersLost);
                _context.Update(army);
            }
                      
            UpdatePlayerStatistics(army.PlayerId, soldiersLost, soldiersKilled);
            UpdatePlayerExp(army.PlayerId, soldiersLost, soldiersKilled);
            UpdatePlayerUnits(army.PlayerId, soldiersLost);
        }

        private void DeleteArmy(Army army)
        {
            _context.Remove(army);
        }

        private void UpdatePlayerStatistics(string playerId, double soldiersLost, double soldiersKilled)
        {
            PlayerStatistics playerStatistics = _context.PlayerStatistics.FirstOrDefault(ps => ps.PlayerId == playerId);
            playerStatistics.SoldiersKilled += Convert.ToInt32(soldiersKilled);
            playerStatistics.SoldiersLost += Convert.ToInt32(soldiersLost);

            _context.Update(playerStatistics);
        }

        private void UpdatePlayerExp(string playerId, double soldiersLost, double soldiersKilled)
        {
            Player player = _context.Players.FirstOrDefault(p => p.Id == playerId);

            player.Exp += Convert.ToInt64(soldiersLost * 1 + soldiersKilled * 2);

            _context.Update(player);
        }

        /// <summary>
        /// Sync units and army in battles.
        /// </summary>
        /// <param name="playerId">string</param>
        /// <param name="soldiersLost">double</param>
        private void UpdatePlayerUnits(string playerId, double soldiersLost)
        {
            Unit unit = _context.Units.FirstOrDefault(u => u.PlayerId == playerId && u.Type == (int)UnitType.Soldier);

            unit.Count -= Convert.ToInt32(soldiersLost);

            _context.Update(unit);
        }

        private bool CheckEndOfWar(War war, double attackersSoldiersCount, double defendersSoldiersCount)
        {
            bool ret = false;

            if (attackersSoldiersCount == 0 
             && defendersSoldiersCount == 0)
            {
                war.IsEnded = true;
                war.WarResult = (int)WarResult.Draw;

                _context.Update(war);

                ret = true;
                
            }
            else if (attackersSoldiersCount == 0)
            {
                EndWar(war, WarResult.Defeat, war.LandIdTo, war.LandIdFrom);

                ret = true;
            }
            else if (defendersSoldiersCount == 0)
            {
                EndWar(war, WarResult.Victory, war.LandIdFrom, war.LandIdTo);

                ret = true;
            }

            return ret;
        }

        private void EndWar(War war, WarResult result, string victoryLandId, string defeatLandId)
        {
            Land victoryLand = _context.Lands.Include(l => l.Country).FirstOrDefault(l => l.LandId == victoryLandId);
            Land defeatLand = _context.Lands.Include(l => l.Country).FirstOrDefault(l => l.LandId == defeatLandId);

            CheckLastCountryLand(defeatLand);
            TransferLandToVictoryCountry(defeatLand, victoryLand.Country);

            war.WarResult = (int)result;
            _context.Update(war);
        }

        private void TransferLandToVictoryCountry(Land defeatLand, Country victoryCountry)
        {
            defeatLand.CountryId = victoryCountry.CountryId;
            _context.Update(defeatLand);
        }

        private void CheckLastCountryLand(Land defeatLand)
        {
            int countryLandsCount = _context.Lands.Count(l => l.CountryId == defeatLand.CountryId);

            if (countryLandsCount == 1)
            {
                Country country = new Country();
                _context.Remove(country);
            }
        }

        private void TryEndWar(War war)
        {
            List<Army> armies = _context.Armies.Include(a => a.Player).Where(a => a.WarId == war.WarId).ToList();
            List<Army> attackersArmies = armies.FindAll(a => a.Side == ArmySide.Attackers);
            List<Army> defendersArmies = armies.FindAll(a => a.Side == ArmySide.Defenders);

            double attackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount);
            double defendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount);

            CheckEndOfWar(war, attackersSoldiersCount, defendersSoldiersCount);
        }
    }
}