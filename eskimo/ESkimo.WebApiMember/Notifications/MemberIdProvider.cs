using Microsoft.AspNetCore.SignalR;

namespace ESkimo.WebApiMember.Notifications
{
  public class MemberIdProvider : IUserIdProvider
    {
    public string GetUserId(HubConnectionContext connection)
    {
      return connection.User.Identity.Name;
    }
  }
}