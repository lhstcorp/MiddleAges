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

        public async Task<IActionResult> GetLocalEventById(string id)
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            LocalEvent localEvent = LocalEventHelper.GetLocalEventById(1);

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
    }
}
