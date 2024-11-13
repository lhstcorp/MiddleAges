using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Models;
using MiddleAges.Temporary_Entities;
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

                CalculateLandDevelopmentShares();
                UpdateProductionLimits();
                UpdatePlayerDailyData();
                CalculateRatingPlaces();

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
            List<LandDevelopmentShare> landDevelopmentShares = _context.LandDevelopmentShares.ToList();

            if (lands.Count > 0)
            {
                foreach (var land in lands)
                {
                    LandDevelopmentShare landDevelopmentShare = landDevelopmentShares.FirstOrDefault(ld => ld.LandId == land.LandId);

                    if (landDevelopmentShare != null)
                    {
                        land.ProductionLimit = CommonLogic.BaseGoldLimit * CommonLogic.LandsCount * landDevelopmentShare.InfrastructureShare;
                    }                    

                    _context.Update(land);
                }
            }
        }

        public void UpdatePlayerDailyData()
        {
            List<Player> players = _context.Players.ToList();

            if (players.Count > 0)
            {
                foreach (var player in players)
                {
                    player.MoneyProduced = 0;
                    player.MoneySpent = 0;

                    _context.Update(player);
                }
            }
        }

        private void CalculateRatingPlaces()
        {
            var query = _context.Units
                                .Include(u => u.Player)
                                .Where(u => u.Type == (int)UnitType.Soldier).ToList()
                                .Join(_context.PlayerAttributes,
                                      u => u.PlayerId,
                                      pa => pa.PlayerId,
                                      (u, pa) => new { Unit = u, PlayerAttribute = pa });

            List<Unit> units = query.Select(q => q.Unit).ToList();
            List<PlayerAttribute> playerAttributes = query.Select(q => q.PlayerAttribute).ToList();

            List<RatingCalculatedPoints> ratingCalculatedPointsList = new List<RatingCalculatedPoints>();

            foreach (Unit u in units)
            {
                RatingCalculatedPoints ratingCalculatedPoints = new RatingCalculatedPoints();
                ratingCalculatedPoints.PlayerId = u.PlayerId;
                ratingCalculatedPoints.ExpPoints = u.Player.Exp;
                ratingCalculatedPoints.MoneyPoints = u.Player.Money;
                ratingCalculatedPoints.WarPowerPoints = u.Count * playerAttributes.FirstOrDefault(pa => pa.PlayerId == u.PlayerId).Warfare;

                ratingCalculatedPointsList.Add(ratingCalculatedPoints);
            }

            List<Rating> ratings = _context.Ratings.ToList();

            _context.Ratings.RemoveRange(ratings);

            //_context.SaveChangesAsync();

            ratings = new List<Rating>();

            ratingCalculatedPointsList = ratingCalculatedPointsList.OrderByDescending(r => r.ExpPoints).ToList();

            for (int i = 1; i <= ratingCalculatedPointsList.Count; i++)
            {
                Rating rating = new Rating();
                rating.PlayerId = ratingCalculatedPointsList[i-1].PlayerId;
                rating.ExpPlace = i;

                ratings.Add(rating);
            }

            ratingCalculatedPointsList = ratingCalculatedPointsList.OrderByDescending(r => r.MoneyPoints).ToList();

            for (int i = 1; i <= ratingCalculatedPointsList.Count; i++)
            {
                Rating rating = ratings.FirstOrDefault(r => r.PlayerId == ratingCalculatedPointsList[i-1].PlayerId);
                rating.MoneyPlace = i;
            }

            ratingCalculatedPointsList = ratingCalculatedPointsList.OrderByDescending(r => r.WarPowerPoints).ToList();

            for (int i = 1; i <= ratingCalculatedPointsList.Count; i++)
            {
                Rating rating = ratings.FirstOrDefault(r => r.PlayerId == ratingCalculatedPointsList[i - 1].PlayerId);
                rating.WarPowerPlace = i;
            }

            List<TotalRating> totalRatings = new List<TotalRating>();

            foreach (Rating r in ratings)
            {
                TotalRating totalRating = new TotalRating();

                totalRating.PlayerId = r.PlayerId;
                totalRating.SumOfPlaces = r.ExpPlace + r.MoneyPlace + r.WarPowerPlace;

                totalRatings.Add(totalRating);
            }

            totalRatings = totalRatings.OrderBy(tr => tr.SumOfPlaces).ToList();

            for (int i = 1; i <= totalRatings.Count; i++)
            {
                Rating rating = ratings.FirstOrDefault(r => r.PlayerId == totalRatings[i - 1].PlayerId);
                rating.TotalPlace = i;
            }

            foreach (Rating r in ratings)
            {
                _context.Add(r);
            }
        }

        public void CalculateLandDevelopmentShares()
        {
            var query = _context.Lands
                                .Join(_context.LandBuildings,
                                      l => l.LandId,
                                      lb => lb.LandId,
                                      (l, lb) => new { Land = l, LandBuilding = lb })
                                .Join(_context.LandDevelopmentShares,
                                      combined => combined.Land.LandId,
                                      ld => ld.LandId,
                                      (combined, ld) => new { combined.Land, combined.LandBuilding, LandDevelopmentShare = ld }).ToList();

            List<Land> lands = _context.Lands.ToList();
            List<LandBuilding> landBuildings = _context.LandBuildings.ToList();
            List<LandDevelopmentShare> landDevelopmentShares = _context.LandDevelopmentShares.ToList();

            double totalInfrastructureBuildings = landBuildings.Where(lb => lb.BuildingType == LandBuildingType.Infrastructure).Sum(lb => lb.Lvl);
            double totalMarketBuildings = landBuildings.Where(lb => lb.BuildingType == LandBuildingType.Market).Sum(lb => lb.Lvl);
            double totalFortificationBuildings = landBuildings.Where(lb => lb.BuildingType == LandBuildingType.Fortification).Sum(lb => lb.Lvl);

            if (lands.Count > 0)
            {
                foreach (var land in lands)
                {
                    LandDevelopmentShare landDevelopmentShare = landDevelopmentShares.FirstOrDefault(ld => ld.LandId == land.LandId);

                    if (landDevelopmentShares != null)
                    {
                        double landInfrastructureLvl = landBuildings.FirstOrDefault(lb => lb.BuildingType == LandBuildingType.Infrastructure && lb.LandId == land.LandId).Lvl;
                        double landMarketLvl = landBuildings.FirstOrDefault(lb => lb.BuildingType == LandBuildingType.Infrastructure && lb.LandId == land.LandId).Lvl;
                        double landFortificationLvl = landBuildings.FirstOrDefault(lb => lb.BuildingType == LandBuildingType.Infrastructure && lb.LandId == land.LandId).Lvl;

                        landDevelopmentShare.InfrastructureShare = landInfrastructureLvl / totalInfrastructureBuildings;
                        landDevelopmentShare.MarketShare = landMarketLvl / totalMarketBuildings;
                        landDevelopmentShare.FortificationShare = landFortificationLvl / totalFortificationBuildings;

                        _context.Update(landDevelopmentShare);
                    }
                }
            }
        }

        private class TotalRating
        {
            public string PlayerId { get; set; }
            public int SumOfPlaces { get; set; }
        }
    }
}
