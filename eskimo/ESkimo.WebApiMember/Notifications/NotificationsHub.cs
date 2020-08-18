using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ESkimo.WebApiMember.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub { }
}