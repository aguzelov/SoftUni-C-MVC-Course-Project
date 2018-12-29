using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Dialog.Web.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}