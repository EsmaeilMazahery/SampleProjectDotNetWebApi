using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ESkimo.WebApiUser.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub { }
}