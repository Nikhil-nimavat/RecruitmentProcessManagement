using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;
using UAParser;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.InkML;
using RecruitmentProcessManagement.Data;

namespace RecruitmentProcessManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IEmailService emailService)
        { 
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)  
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SendConfirmationEmail(model.Email, user);
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    return View("RegistrationSuccessful");
                }    
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return View(model);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "Your email address is not confirmed. Please confirm your email before logging in.");
                        model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                        return View(model);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    // Handle two-factor authentication case
                }
                if (result.IsLockedOut)
                {
                    // Handle lockout scenario
                }
                else
                {
                    // Generic failure message for invalid credentials
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);

                TempData["ProfileUpdateSuccess"] = "Your profile has been updated successfully.";
                return RedirectToAction("MyProfile");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        private async Task SendConfirmationEmail(string email, IdentityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _userManager.SetAuthenticationTokenAsync(user, "EmailConfirmation", "EmailConfirmationToken", token);

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { UserId = user.Id, Token = token }, protocol: HttpContext.Request.Scheme);

            var safeLink = HtmlEncoder.Default.Encode(confirmationLink);

            var subject = "Welcome to RP Management! Please Confirm Your Email";

            var messageBody = $@"
                <div style=""font-family:Arial,Helvetica,sans-serif;font-size:16px;line-height:1.6;color:#333;"">
                    <p>Hi,</p>

                    <p>Thank you for creating an account at <strong>RP Management</strong>.
                    To start enjoying all of our features, please confirm your email address by clicking the button below:</p>

                    <p>
                        <a href=""{safeLink}"" 
                        style=""background-color:#007bff;color:#fff;padding:10px 20px;text-decoration:none;
                                font-weight:bold;border-radius:5px;display:inline-block;"">
                            Confirm Email
                        </a>
                    </p>

                    <p>If the button doesn’t work for you, copy and paste the following URL into your browser:
                        <br />
                        <a href=""{safeLink}"" style=""color:#007bff;text-decoration:none;"">{safeLink}</a>
                    </p>

                    <p>If you did not sign up for this account, please ignore this email.</p>

                    <p>Thanks,<br />
                    The RP Management Team</p>
                </div>
            ";
            await _emailService.SendEmailAsync(email, subject, messageBody);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string UserId, string Token)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            {
                ViewBag.ErrorMessage = "The link is invalid or has expired. Please request a new one if needed.";
                return View();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "We could not find a user associated with the given link.";
                return View();
            }

            var result = await _userManager.ConfirmEmailAsync(user, Token);

            await _userManager.RemoveAuthenticationTokenAsync(user, "EmailConfirmation", "EmailConfirmationToken");

            if (result.Succeeded)
            {
                ViewBag.Message = "Thank you for confirming your email address. Your account is now verified!";
                return View();
            }

            ViewBag.ErrorMessage = "We were unable to confirm your email address. Please try again or request a new link.";
            return View();
        }        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResendConfirmationEmail(bool IsResend = true)
        {
            if (IsResend)
            {
                ViewBag.Message = "Resend Confirmation Email";
            }
            else
            {
                ViewBag.Message = "Send Confirmation Email";
            }
            return View();
        }    

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmationEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                return View("ConfirmationEmailSent");
            }

            await SendConfirmationEmail(Email, user);
            return View("ConfirmationEmailSent");
        }

        public IActionResult RegistrationSuccessful()
        {
            return View("RegistrationSuccessful");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        private async Task SendForgotPasswordEmail(string? email, IdentityUser? user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordToken", token);

            var passwordResetLink = Url.Action("ResetPassword", "Account",
                new { Email = email, Token = token }, protocol: HttpContext.Request.Scheme);

            // Encode the link to prevent XSS attacks
            var safeLink = HtmlEncoder.Default.Encode(passwordResetLink);

            var subject = "Reset Your Password";

            var messageBody = $@"
            <div style=""font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #333; line-height: 1.5; padding: 20px;"">
                <h2 style=""color: #007bff; text-align: center;"">Password Reset Request</h2>
                <p style=""margin-bottom: 20px;"">Hi,</p>
                
                <p>We received a request to reset your password for your <strong>RP Management</strong> account. If you made this request, please click the button below to reset your password:</p>
                
                <div style=""text-align: center; margin: 20px 0;"">
                    <a href=""{safeLink}"" 
                    style=""background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; font-weight: bold; border-radius: 5px; display: inline-block;"">
                        Reset Password
                    </a>
                </div>
                
                <p>If the button above doesn’t work, copy and paste the following URL into your browser:</p>
                <p style=""background-color: #f8f9fa; padding: 10px; border: 1px solid #ddd; border-radius: 5px;"">
                    <a href=""{safeLink}"" style=""color: #007bff; text-decoration: none;"">{safeLink}</a>
                </p>
                
                <p>If you did not request to reset your password, please ignore this email or contact support if you have concerns.</p>
                
                <p style=""margin-top: 30px;"">Thank you,<br />The RP Management Team</p>
            </div>";

            await _emailService.SendEmailAsync(email, subject, messageBody);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await SendForgotPasswordEmail(user.Email, user);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }
            
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    await _userManager.RemoveAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordToken");

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description); 
                    }

                    return View(model);
                }

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // Get user location and alert based on it like google
        private async Task<string> GetLocationAsync(HttpContext httpContext)
        {
            try
            {
                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
                {
                    using var client = new HttpClient();
                    ipAddress = await client.GetStringAsync("https://api.ipify.org");
                }

                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync($"http://ip-api.com/json/{ipAddress}");

                var locationData = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (locationData != null && locationData.TryGetValue("city", out var city) &&
                    locationData.TryGetValue("regionName", out var region) &&
                    locationData.TryGetValue("country", out var country))
                {
                    return $"{city}, {region}, {country}";
                }

                return "Unknown Location";
            }
            catch
            {
                return "Unknown Location";
            }
        }

        private async Task SendPasswordChangedNotificationEmail(string email, IdentityUser user, string location, string device)
        {
            var subject = "Your Password Has Been Changed";

            var messageBody = $@"
        <div style=""font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #333; line-height: 1.5; padding: 20px;"">
            <h2 style=""color: #007bff; text-align: center;"">Password Change Notification</h2>
            <p style=""margin-bottom: 20px;"">Hi,</p>
            
            <p>We wanted to let you know that your password for your <strong>RP Management
            </strong> account was successfully changed.</p>
            
            <div style=""margin: 20px 0; padding: 10px; background-color: #f8f9fa; border: 1px solid #ddd; border-radius: 5px;"">
                <p><strong>Date and Time:</strong> {DateTime.UtcNow:dddd, MMMM dd, yyyy HH:mm} UTC</p>
                <p><strong>Location:</strong> {location}</p>
                <p><strong>Device:</strong> {device}</p>
            </div>
            
            <p>If you did not make this change, please reset your password immediately or contact support for assistance:</p>
            
            <p style=""margin-top: 30px;"">Thank you,<br />The RP Management Team</p>
        </div>";

            await _emailService.SendEmailAsync(email, subject, messageBody);
        }

        private string GetDeviceInfo(HttpContext httpContext)
        {
            try
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (string.IsNullOrEmpty(userAgent))
                {
                    return "Unknown Device";
                }
                var parser = Parser.GetDefault();
                var clientInfo = parser.Parse(userAgent);
                var os = clientInfo.OS.ToString(); 
                var browser = clientInfo.UA.ToString(); 

                return $"{browser} on {os}";
            }
            catch
            {
                return "Unknown Device";
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)  
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    var location = await GetLocationAsync(HttpContext);
                    var device = GetDeviceInfo(HttpContext);

                    await SendPasswordChangedNotificationEmail(user.Email, user, location, device);

                    await _signInManager.RefreshSignInAsync(user);

                    return RedirectToAction("ChangePasswordConfirmation", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        
    }
}


  