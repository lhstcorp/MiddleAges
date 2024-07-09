using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.HelperClasses;
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
            string greenSpan = "<span style=\"color: red; font - weight: 700\">{0}</span>";
            string redSpan = "<span style=\"color: red; font - weight: 700\">{0}</span>";

            switch (optionNum)
            {
                case 1:
                    optionElement += localEvent.Option1Text + "(";

                    for (int i = 1; i <= localEvent.Option1Chances.Count(); i++)
                    {
                        if (localEvent.Option1Chances[i] > 0)
                        {
                            optionElement += " " + (localEvent.Option1Values[i] > 0 ?
                                                    string.Format(greenSpan, localEvent.Option1Values[i]) :
                                                    string.Format(redSpan, localEvent.Option1Values[i]));
                            optionElement += " " + GetOptionValueName(i);
                        }
                    }

                    optionElement + ")";

                    break;
                case 2:
                    optionElement += localEvent.Option2Text + "(";

                    for (int i = 1; i <= localEvent.Option2Chances.Count(); i++)
                    {
                        if (localEvent.Option2Chances[i] > 0)
                        {
                            optionElement += " " + (localEvent.Option2Values[i] > 0 ?
                                                    string.Format(greenSpan, localEvent.Option2Values[i]) :
                                                    string.Format(redSpan, localEvent.Option2Values[i]));
                            optionElement += " " + GetOptionValueName(i);
                        }
                    }

                    optionElement + ")";

                    break;
            }

            return optionElement;
        }

        public string GetOptionValueName(int valueNum)
        {
            string optionValueName = "";

            switch (valueNum)
            {
                case 1:
                    optionValueName = "coins";
                    break;
                case 2:
                    optionValueName = "recruits";
                    break;
                case 3:
                    optionValueName = "experience";
                    break;
                case 4:
                    optionValueName = "peasants";
                    break;
                case 5:
                    optionValueName = "soldiers";
                    break;
            }

            return optionValueName;
        }
    }
}
