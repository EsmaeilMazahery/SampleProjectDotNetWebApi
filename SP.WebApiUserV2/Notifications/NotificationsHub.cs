using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SP.WebApiUser.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub { }
}