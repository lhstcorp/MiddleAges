using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.HelperClasses;
using MiddleAges.Models;
using MiddleAges.Temporary_Entities;
using MiddleAges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static MiddleAges.HelperClasses.LocalEventHelper;

namespace MiddleAges.Controllers
{
    [Authorize]
    public class LocalEventController : Controller
    {
        private readonly ILogger<LocalEventController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public LocalEventController(ILogger<LocalEventController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetLocalEventById(Guid id)
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            PlayerLocalEvent playerLocalEvent = await _context.PlayerLocalEvents.FirstOrDefaultAsync(le => le.LocalEventId == id);

            if (playerLocalEvent == null)
            {
                return null;
            }

            LocalEvent localEvent = LocalEventHelper.GetLocalEventById(playerLocalEvent.EventId);

            LocalEventViewModel localEventViewModel = new LocalEventViewModel
            {
                LocalEvent = localEvent,
                Option1Element = GenerateOptionElement(localEvent, 1),
                Option2Element = GenerateOptionElement(localEvent, 2)
            };

            return Json(JsonSerializer.Serialize(localEventViewModel));
        }

        public string GenerateOptionElement(LocalEvent localEvent, int optionNum)
        {
            string optionElement = "";
            string greenSpan = "<span style=\"color: limegreen; font-weight: 700\">+{0}</span>";
            string redSpan = "<span style=\"color: red; font-weight: 700\">{0}</span>";
            string chanceSpan = "<span style=\"font-style: italic;\">{0}</span>";
            bool emptyOption = true;

            switch (optionNum)
            {
                case 1:
                    optionElement += localEvent.Option1Text + " (";

                    for (int i = 0; i < localEvent.Option1Chances.Count(); i++)
                    {
                        if (localEvent.Option1Chances[i] > 0)
                        {
                            
                            double optionValue = localEvent.Option1Values[i];

                            if (i == 0 || i == 2) // Money or Exp
                            {
                                optionValue = RecalculateOptionValue(optionValue, i).Result;
                            }

                            if (!emptyOption)
                            {
                                optionElement += " ";
                            }

                            optionElement += optionValue > 0 ?
                                                string.Format(greenSpan, optionValue) :
                                                string.Format(redSpan, optionValue);
                            optionElement += " " + GetOptionValueName(i);

                            if (localEvent.Option1Chances[i] < 100)
                            {
                                optionElement += " [" + string.Format(chanceSpan, localEvent.Option1Chances[i] + "% chance") + "]";
                            }

                            emptyOption = false;
                        }
                    }

                    if (emptyOption)
                    {
                        optionElement += "No effects";
                    }

                    optionElement += ")";

                    break;
                case 2:
                    optionElement += localEvent.Option2Text + " (";

                    for (int i = 0; i < localEvent.Option2Chances.Count(); i++)
                    {
                        if (localEvent.Option2Chances[i] > 0)
                        {
                            
                            double optionValue = localEvent.Option2Values[i];

                            if (i == 0 || i == 2) // Money or Exp
                            {
                                optionValue = RecalculateOptionValue(optionValue, i).Result;
                            }

                            if (!emptyOption)
                            {
                                optionElement += " ";
                            }

                            optionElement += optionValue > 0 ?
                                                string.Format(greenSpan, optionValue) :
                                                string.Format(redSpan, optionValue);
                            optionElement += " " + GetOptionValueName(i);

                            if (localEvent.Option2Chances[i] < 100)
                            {
                                optionElement += " [" + string.Format(chanceSpan, localEvent.Option2Chances[i] + "% chance") + "]";
                            }

                            emptyOption = false;
                        }
                    }

                    if (emptyOption)
                    {
                        optionElement += "No effects";
                    }

                    optionElement += ")";

                    break;
            }

            return optionElement;
        }

        public string GetOptionValueName(int valueNum)
        {
            string optionValueName = "";

            switch (valueNum)
            {
                case 0:
                    optionValueName = "coins";
                    break;
                case 1:
                    optionValueName = "recruits";
                    break;
                case 2:
                    optionValueName = "experience";
                    break;
                case 3:
                    optionValueName = "peasants";
                    break;
                case 4:
                    optionValueName = "soldiers";
                    break;
            }

            return optionValueName;
        }

        public async Task<double> RecalculateOptionValue(double value, int valueType)
        {
            double optionValue = value;

            var player = await _userManager.GetUserAsync(HttpContext.User);

            switch (valueType)
            {
                case 0:                    
                    Unit unit = await _context.Units.FirstOrDefaultAsync(u => u.PlayerId == player.Id
                                                                           && u.Type == (int)UnitType.Peasant);

                    PlayerAttribute playerAttribute = await _context.PlayerAttributes.FirstOrDefaultAsync(pa => pa.PlayerId == player.Id);

                    optionValue *= Math.Round(unit.Count * 0.01 * (1 + Convert.ToDouble(playerAttribute.Management) * 0.02));
                    break;
                case 2:
                    optionValue *= Convert.ToDouble(CommonLogic.GetRequiredExpByLvl(player.Lvl + 1) - CommonLogic.GetRequiredExpByLvl(player.Lvl)) / 100.00;
                    break;
            }

            return optionValue;
        }

        public async Task<IActionResult> SelectLocalEventOption(string localEventId, string optionNum)
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            PlayerLocalEvent playerLocalEvent = await _context.PlayerLocalEvents.FirstOrDefaultAsync(w => w.LocalEventId.ToString() == localEventId);

            if (playerLocalEvent != null
             && player.Id == playerLocalEvent.PlayerId)
            {
                LocalEvent localEvent = LocalEventHelper.GetLocalEventById(playerLocalEvent.EventId);

                double[] optionValues = { 0, 0, 0, 0, 0 };
                double[] optionChances = { 0, 0, 0, 0, 0 };

                switch (optionNum)
                {                    
                    case "1":
                        optionValues = localEvent.Option1Values;
                        optionChances = localEvent.Option1Chances;
                        break;
                    case "2":
                        optionValues = localEvent.Option2Values;
                        optionChances = localEvent.Option2Chances;
                        break;
                }

                // Normalizing money and exp option values -->
                if (optionValues[0] != 0)
                { 
                    optionValues[0] = await RecalculateOptionValue(optionValues[0], 0);
                }

                if (optionValues[2] != 0)
                {
                    optionValues[2] = await RecalculateOptionValue(optionValues[2], 2);
                }
                // <--

                if (ValidateOptionValues(optionValues, player))
                {
                    await ApplyRewardsAndPenalties(optionValues, optionChances, player.Id);
                    result = "Ok";
                    //_context.Remove(playerLocalEvent);
                }
                else
                {
                    result = "ValidationFailed";
                }

                try
                {
                    await _context.SaveChangesAsync();                    
                }
                catch
                {
                    result = "Error";
                }
            }

            return Json(JsonSerializer.Serialize(result));
        }

        private bool ValidateOptionValues(double[] optionValues, Player player)
        {
            bool ret = true;

            if (optionValues[0] < 0)
            {
                ret = ValidatePlayerMoney(optionValues[0], player);
            }

            if (optionValues[1] < 0
             && ret)
            {
                ret = ValidatePlayerRecruits(optionValues[1], player);
            }

            if (optionValues[3] < 0
             && ret)
            {
                ret = ValidatePlayerPeasants(optionValues[3], player);
            }

            if (optionValues[4] < 0
             && ret)
            {
                ret = ValidatePlayerSoldiers(optionValues[4], player);
            }

            return ret;
        }

        private bool ValidatePlayerMoney(double optionValue, Player player)
        {
            bool ret = true;

            if (player.Money < -optionValue)
            {
                ret = false;
            }

            return ret;
        }

        private bool ValidatePlayerRecruits(double optionValue, Player player)
        {
            bool ret = true;

            if (player.RecruitAmount < -optionValue)
            {
                ret = false;
            }

            return ret;
        }

        private bool ValidatePlayerPeasants(double optionValue, Player player)
        {
            bool ret = true;

            Unit peasants = _context.Units.FirstOrDefault(u => u.PlayerId == player.Id
                                                            && u.Type == (int)UnitType.Peasant);

            if (peasants.Count < -optionValue)
            {
                ret = false;
            }

            return ret;
        }

        private bool ValidatePlayerSoldiers(double optionValue, Player player)
        {
            bool ret = true;

            Unit peasants = _context.Units.FirstOrDefault(u => u.PlayerId == player.Id
                                                            && u.Type == (int)UnitType.Soldier);

            if (peasants.Count < -optionValue)
            {
                ret = false;
            }

            return ret;
        }

        private async Task ApplyRewardsAndPenalties(double[] optionValues, double[] optionChances, string playerId)
        {
            Player player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);

            double chanceValue = CommonLogic.GetRandomNumber(0, 100.01);

            if (optionChances[0] > chanceValue)
            {
                player.Money += optionValues[0];
            }

            if (optionChances[1] > chanceValue)
            {
                player.RecruitAmount += Convert.ToInt32(optionValues[1]);
            }

            if (optionChances[2] > chanceValue)
            {
                player.Exp += Convert.ToInt32(optionValues[2]);
            }

            _context.Update(player);

            List<Unit> units = await _context.Units.Where(u => u.PlayerId == player.Id).ToListAsync();

            if (optionChances[3] > chanceValue)
            {
                units[0].Count += Convert.ToInt32(optionValues[3]);
                _context.Update(units[0]);
            }

            if (optionChances[4] > chanceValue)
            {
                units[1].Count += Convert.ToInt32(optionValues[4]);
                _context.Update(units[1]);
            }  
        }

        public JsonResult GetPlayerData()
        {
            Player player = _userManager.GetUserAsync(HttpContext.User).Result;

            List<Unit> units = _context.Units.Where(u => u.PlayerId == player.Id).ToList();

            return Json(JsonSerializer.Serialize(new { Player = player, Peasants = units[0], Soldiers = units[1], ProgressbarExpNow = player.Exp - CommonLogic.GetRequiredExpByLvl(player.Lvl) }));
        }
    }
}
