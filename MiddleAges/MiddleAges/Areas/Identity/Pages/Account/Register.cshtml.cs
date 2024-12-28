using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Models;

namespace MiddleAges.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Player> _signInManager;
        private readonly UserManager<Player> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Player> userManager,
            SignInManager<Player> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "UserName must be at least 3 and at max 30 characters long.", MinimumLength = 3)]
            [Display(Name = "User name")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                string randomLand = CommonLogic.getRandomMapLandId();
                Player player = new Player { UserName = Input.UserName, Email = Input.Email, Exp = 0, Lvl = 1, Money = 100, ImageURL = new Random().Next(1, 49).ToString() + ".webp", CurrentLand = randomLand, ResidenceLand = randomLand, RecruitAmount = 10, RegistrationDateTime = DateTime.UtcNow, LastActivityDateTime = DateTime.UtcNow };
                var result = await _userManager.CreateAsync(player, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(player);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = player.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await InitPlayerData(player.Id, player.CurrentLand);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(player, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task InitPlayerData(string userId, string currentLandId)
        {
            Building building;

            building = new Building { PlayerId = userId, Type = (int)BuildingType.Estate, Lvl = 1, LandId = currentLandId };
            _context.Buildings.Add(building);
            building = new Building { PlayerId = userId, Type = (int)BuildingType.Barracks, Lvl = 1, LandId = currentLandId };
            _context.Buildings.Add(building);

            Unit unit;

            unit = new Unit { PlayerId = userId, Type = (int)UnitType.Peasant, Lvl = 1, Count = 100, LandId = currentLandId };
            _context.Units.Add(unit);
            unit = new Unit { PlayerId = userId, Type = (int)UnitType.Soldier, Lvl = 1, Count = 0, LandId = currentLandId };
            _context.Units.Add(unit);

            PlayerStatistics playerStatistics = new PlayerStatistics { PlayerId = userId, SoldiersKilled = 0, SoldiersLost = 0 };
            _context.PlayerStatistics.Add(playerStatistics);

            PlayerAttribute playerAttribute = new PlayerAttribute { PlayerId = userId };
            _context.PlayerAttributes.Add(playerAttribute);

            PlayerInformation playerInformation = new PlayerInformation { PlayerId = userId };
            _context.PlayerInformations.Add(playerInformation);

            await _context.SaveChangesAsync();
        }
    }
}
