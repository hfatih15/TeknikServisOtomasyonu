using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Enums;

namespace TeknikServis.BLL.Services.Senders
{
    public interface IMessageService
    {
        MessageStates MessageState { get; }

        Task SendAsync(IdentityMessage message, params string[] contacts);
        void Send(IdentityMessage message, params string[] contacts);
    }
}
