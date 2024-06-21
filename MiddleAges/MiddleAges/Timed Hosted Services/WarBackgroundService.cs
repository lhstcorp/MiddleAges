using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Models;
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
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(30),
                TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                List<War> wars = _context.Wars.Where(w => w.IsEnded == false && w.StartDateTime < DateTime.UtcNow).ToList();

                if (wars.Count > 0)
                {
                    foreach (var war in wars)
                    {
                        CalculateWar(war);
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

        private void CalculateWar(War war)
        {
            var query = _context.PlayerAttributes
                                    .Join(_context.Players,
                                        pa => pa.PlayerId,
                                        p => p.Id,
                                        (pa, p) => new { PlayerAttribute = pa, Player = p })
                                    .Join(_context.Armies,
                                        combined => combined.Player.Id,
                                        a => a.PlayerId,
                                        (combined, a) => new { combined.PlayerAttribute, combined.Player, Army = a })
                                    .Where(combined => combined.Army.WarId == war.WarId)
                                    .Select(combined => new { PlayerAttribute = combined.PlayerAttribute, Player = combined.Player, Army = combined.Army }).ToList();

            List<Army> armies = query.Select(q => q.Army).ToList();
            List<Army> attackersArmies = armies.FindAll(a => a.Side == ArmySide.Attackers);
            List<Army> defendersArmies = armies.FindAll(a => a.Side == ArmySide.Defenders);
            List<PlayerAttribute> playerAttributes = query.Select(q => q.PlayerAttribute).ToList();
            List<PlayerAttribute> attackersPlayerAttributes = new List<PlayerAttribute>();
            List<PlayerAttribute> defendersPlayerAttributes = new List<PlayerAttribute>();

            foreach (Army army in attackersArmies)
            {
                attackersPlayerAttributes.Add(playerAttributes.FirstOrDefault(pa => pa.PlayerId == army.PlayerId));
            }

            foreach (Army army in defendersArmies)
            {
                defendersPlayerAttributes.Add(playerAttributes.FirstOrDefault(pa => pa.PlayerId == army.PlayerId));
            }

            double attackersArmyStrength = 100 + Math.Round(CommonLogic.GetAverageArmyWarfare(attackersArmies, attackersPlayerAttributes) * 2, 2);
            double defendersArmyStrength = 100 + Math.Round(CommonLogic.GetAverageArmyWarfare(defendersArmies, defendersPlayerAttributes) * 2, 2);

            double attackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount);
            double defendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount);

            if (!CheckEndOfWar(war, attackersSoldiersCount, defendersSoldiersCount))
            {
                double attackersPower = attackersSoldiersCount * attackersArmyStrength;
                double defendersPower = defendersSoldiersCount * defendersArmyStrength;

                double attackersPerc = attackersPower / (attackersPower + defendersPower);
                double defendersPerc = 1 - attackersPerc;

                const double lossPerc = 0.2;

                Random rnd = new Random();
                double cubeValueA = Convert.ToDouble(rnd.Next(-50, 51)) / 100.00;
                double cubeValueD = Convert.ToDouble(rnd.Next(-50, 51)) / 100.00;

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
            }

            army.SoldiersCount -= Convert.ToInt32(soldiersLost);
            army.SoldiersLost += Convert.ToInt32(soldiersLost);
            army.SoldiersKilled += Convert.ToInt32(soldiersKilled);
            _context.Update(army);

            UpdatePlayerStatistics(army.PlayerId, soldiersLost, soldiersKilled);
            UpdatePlayerExp(army.PlayerId, soldiersLost, soldiersKilled);
            UpdatePlayerUnits(army.PlayerId, soldiersLost);
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

            if (unit.Count < 0)
            {
                unit.Count = 0;
            }

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

            if (war.IsRevolt)
            {
                CheckLastCountryLand(defeatLand);

                if (result == WarResult.Victory) //if the rebels got success
                {
                    Country rebelCountry = CreateNewRebelCountry(war, defeatLand);
                    TransferLandToVictoryCountry(defeatLand, rebelCountry);
                }
            }
            else
            {
                CheckLastCountryLand(defeatLand);
                TransferLandToVictoryCountry(defeatLand, victoryLand.Country);
            }

            war.IsEnded = true;
            war.WarResult = (int)result;
            _context.Update(war);

            DisbandWarArmies(war);
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
                country = defeatLand.Country;
                _context.Remove(country);

                Country independentCountry = _context.Countries.FirstOrDefault(c => c.Name == "Independent lands");
                defeatLand.CountryId = independentCountry.CountryId;
                _context.Update(defeatLand);
            }
            else
            {
                if (CheckDefeatLandIsCapital(defeatLand))
                {
                    ChangeCountryCapital(defeatLand);
                }
            }
        }

        private bool CheckDefeatLandIsCapital(Land defeatLand)
        {
            bool ret = false;

            Country country = _context.Countries.FirstOrDefault(c => c.CapitalId == defeatLand.LandId);

            if (country != null)
            {
                ret = true;
            }

            return ret;
        }

        private void ChangeCountryCapital(Land defeatLand)
        {
            Land newCapital = _context.Lands.FirstOrDefault(l => l.CountryId == defeatLand.CountryId 
                                                              && l.LandId != defeatLand.LandId);

            defeatLand.Country.CapitalId = newCapital.LandId;
            _context.Update(defeatLand.Country);
        }

        private void TryEndWar(War war)
        {
            List<Army> armies = _context.Armies.Where(a => a.WarId == war.WarId).ToList();
            List<Army> attackersArmies = armies.FindAll(a => a.Side == ArmySide.Attackers);
            List<Army> defendersArmies = armies.FindAll(a => a.Side == ArmySide.Defenders);

            double attackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount);
            double defendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount);

            CheckEndOfWar(war, attackersSoldiersCount, defendersSoldiersCount);
        }

        private void DisbandWarArmies(War war)
        {
            List<Army> armies = _context.Armies.Where(a => a.WarId == war.WarId).ToList();

            foreach (var army in armies)
            {
                _context.Remove(army);
            }
        }

        private Country CreateNewRebelCountry(War war, Land defeatLand)
        {
            Country country = new Country();

            country.Name = "Rebel state";
            country.CapitalId = defeatLand.LandId;
            country.Color = "#000000";
            country.RulerId = war.RebelId;

            _context.Add(country);

            return country;
        }
    }
}