using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotificationService _notificationService;

        public BaseController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Implement this method to get the logged-in user ID.
            //int userId = GetLoggedInUserId(); 
            //ViewBag.Notifications = _notificationService.GetUserNotifications(userId).Result;
            //base.OnActionExecuting(context);
        }
    }
}
