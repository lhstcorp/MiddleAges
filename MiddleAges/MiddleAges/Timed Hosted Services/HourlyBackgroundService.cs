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
    public class HourlyBackgroundService : IHostedService, IDisposable
    {
        ApplicationDbContext _context;        

        private readonly IServiceScopeFactory _scopeFactory;

        private Timer? _timer = null;

        public HourlyBackgroundService(IServiceScopeFactory scopeFactory)
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

                DoProductionBackgroundLogic();
                DoRecruitBackgroundLogic();
                DoWarBackgroundLogic();
                DoLocalEventBackgroundLogic();
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

        private void DoProductionBackgroundLogic()
        {
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

        private void CalculateProductionIncome(Player player)
        {
            Unit peasants = _context.Units.FirstOrDefault(u => u.PlayerId == player.Id && u.Type == (int)UnitType.Peasant);
            Land land = _context.Lands.Include(l => l.Country).FirstOrDefault(l => l.LandId == player.ResidenceLand);
            LandDevelopmentShare landDevelopmentShare = _context.LandDevelopmentShares.FirstOrDefault(ld => ld.LandId == player.ResidenceLand);
            PlayerAttribute playerAttribute = _context.PlayerAttributes.FirstOrDefault(pa => pa.PlayerId == player.Id);

            double peasantHourIncome = CommonLogic.BasePeasantIncome * CommonLogic.LandsCount * landDevelopmentShare.MarketShare;

            if (land.ProductionLimit > 0)
            {
                double hourIncome = peasants.Count * peasantHourIncome * (1 + 0.02 * playerAttribute.Management); // 100% + 2% * Management attribute
                player.Money += hourIncome * (1 - (land.LandTax / 100.00));
                player.MoneyProduced += hourIncome * (1 - (land.LandTax / 100.00));

                player.Exp += Convert.ToInt64(Math.Floor(hourIncome));

                _context.Update(player);

                land.ProductionLimit -= hourIncome;

                if (land.ProductionLimit < 0)
                {
                    land.ProductionLimit = 0;
                }

                land.Money += hourIncome * land.LandTax * (1 - (land.CountryTax / 100.00)) / 100.00;

                _context.Update(land);

                Country country = land.Country;
                country.Money += hourIncome * land.LandTax / 100.00 * land.CountryTax / 100.00;

                _context.Update(country);
            }
        }

        private void CalculatePlayerExpenses(Player player)
        {
            const double _soldierHourExpense = 0.02;

            Unit soldiers = _context.Units.FirstOrDefault(u => u.PlayerId == player.Id && u.Type == (int)UnitType.Soldier);

            if (soldiers.Count > 0)
            {
                double hourExpense = soldiers.Count * _soldierHourExpense;
                player.Money -= hourExpense;
                player.MoneySpent += hourExpense;

                _context.Update(player);
            }
        }

        private void DoRecruitBackgroundLogic()
        {            
            List<Player> players = _context.Players.ToList();

            foreach (var player in players)
            {
                AddRecruits(player);
            }

            _context.SaveChanges();
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

        private void DoWarBackgroundLogic()
        {
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

            // Fortification strengths -->
            var attackersFortificationStrengthQuery = _context.LandDevelopmentShares
                                                .Join(_context.Wars,
                                                    ld => ld.LandId,
                                                    w => w.LandIdFrom,
                                                    (ld, w) => new { LandDevelopmentShare = ld, War = w })
                                                .FirstOrDefault(q => q.War.WarId == war.WarId);

            LandDevelopmentShare attackersLandDevelopmentShare = attackersFortificationStrengthQuery.LandDevelopmentShare;

            var defendersFortificationStrengthQuery = _context.LandDevelopmentShares
                                                .Join(_context.Wars,
                                                    ld => ld.LandId,
                                                    w => w.LandIdFrom,
                                                    (ld, w) => new { LandDevelopmentShare = ld, War = w })
                                                .FirstOrDefault(q => q.War.WarId == war.WarId);

            LandDevelopmentShare defendersLandDevelopmentShare = defendersFortificationStrengthQuery.LandDevelopmentShare;

            LandDevelopmentShare maxFortificationLandDevelopmentShare = _context.LandDevelopmentShares.OrderByDescending(lds => lds.FortificationShare).FirstOrDefault();

            double attackersFortificationStrength = Math.Round(100 * CommonLogic.GetFortificationValue(attackersLandDevelopmentShare.FortificationShare, maxFortificationLandDevelopmentShare.FortificationShare));
            double defendersFortificationStrength = Math.Round(100 * CommonLogic.GetFortificationValue(defendersLandDevelopmentShare.FortificationShare, maxFortificationLandDevelopmentShare.FortificationShare));
            // <--

            double attackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount);
            double defendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount);

            if (!CheckEndOfWar(war, attackersSoldiersCount, defendersSoldiersCount))
            {
                double attackersPower = attackersSoldiersCount * attackersArmyStrength * attackersFortificationStrength;
                double defendersPower = defendersSoldiersCount * defendersArmyStrength * defendersFortificationStrength;

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
                if (result == WarResult.Victory) //if the rebels got success
                {
                    CheckLastCountryLand(defeatLand);
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

            DestroyBuildingsAfterWar(defeatLand);
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
        private void DoLocalEventBackgroundLogic()
        {
            DeleteExpiredLocalEvents();
            AddLocalEventsToFreeSlots();

            _context.SaveChanges();
        }

        private void DeleteExpiredLocalEvents()
        {
            List<PlayerLocalEvent> localEvents = _context.PlayerLocalEvents.Where(le => le.AssignedDateTime.AddHours(6) < DateTime.UtcNow).ToList();

            foreach (var localEvent in localEvents)
            {
                _context.Remove(localEvent);
            }
        }

        private void AddLocalEventsToFreeSlots()
        {
            List<Player> players = _context.Players.ToList();

            List<PlayerLocalEvent> activeLocalEvents = _context.PlayerLocalEvents.Where(le => le.AssignedDateTime.AddHours(6) > DateTime.UtcNow).ToList();

            foreach (var player in players)
            {
                int playerlocalEventsCount = activeLocalEvents.FindAll(le => le.PlayerId == player.Id).Count;

                int randomEventId = new Random().Next(1, 54); //MaxID - 1

                for (int i = 0; i < 2 - playerlocalEventsCount; i++) // if playerlocalEventsCount < 2 but + additional check because we can have 0 events
                { 
                    AddRandomLocalEvent(player.Id, randomEventId);
                    randomEventId++;
                }
            }
        }

        private void AddRandomLocalEvent(string playerId, int eventId)
        {
            PlayerLocalEvent localEvent = new PlayerLocalEvent
            {
                PlayerId = playerId,
                EventId = eventId,
                AssignedDateTime = DateTime.UtcNow
            };

            _context.Add(localEvent);
        }

        private void DestroyBuildingsAfterWar(Land defeatLand)
        {
            List<LandBuilding> landBuildings  = _context.LandBuildings.Where(lb => lb.LandId == defeatLand.LandId).ToList();

            for (int i = 0; i <= landBuildings.Count; i++)
            {
                landBuildings[i].Lvl = (int)Math.Ceiling(Convert.ToDouble(landBuildings[i].Lvl) * CommonLogic.LandBuildingDestructionPercentage);

                _context.Update(landBuildings);
            }
        }
    }
}
