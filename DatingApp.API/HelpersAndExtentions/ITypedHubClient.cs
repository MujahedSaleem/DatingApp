using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.HelpersAndExtentions
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(MessageForCreationDto message);
        Task NewMessage(string username, string message);
        Task DeleteMessage(int id);
        Task MarkMessages(List<int>  enumerable);
    }
}