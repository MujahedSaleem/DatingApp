using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.HelpersAndExtentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/users/{userId}/[controller]")]
    [ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class MessageController : ControllerBase
    {

        private IHubContext<notifyHub, ITypedHubClient> _hubContext;

        public MessageController(IDatingRepository repository, IMapper mapper, IHubContext<notifyHub, ITypedHubClient> hubContext)
        {
            repo = repository;
            map = mapper;
            _hubContext = hubContext;
        }

        public readonly IDatingRepository repo;

        public readonly IMapper map;

        [HttpGet("{id}", Name = "getMessage")]
        public async Task<IActionResult> getMessage(string userId, int id)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var message = await repo.GetMessage(id);
            if (message is null)
                return NotFound();
            return Ok(new { Message = message });

        }
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> messageThread(string userId, string recipientId)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var messages = await repo.GetMessageThread(userId, recipientId);

            var messagetoreturn = map.Map<IEnumerable<MessageForReturnDto>>(messages);
            return Ok(messagetoreturn);
        }
        private void MarkAsRead(IEnumerable<Message> messages, string userId)
        {
            foreach (var x in messages)
            {
                if (x.senderId != userId)
                    x.isRead = true;
            }
        }

        [HttpPost("signalR")]
        public async Task<IActionResult> Post(string userId, MessageForCreationDto MessageForCreationDto)
        {
            string retMessage = string.Empty;

            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            MessageForCreationDto.senderId = userId;
            var recipient = await repo.GetUser(MessageForCreationDto.recipientId);
            if (recipient is null)
            { return BadRequest("Can't Find User"); }
            var x = await repo.GetMainPhoto(userId);
            MessageForCreationDto.senderPhotoUrl = x.Url;
            var Message = map.Map<Message>(MessageForCreationDto);
            Message.Recipient = recipient;
            Message.Sender = await repo.GetUser(userId);


            repo.Add(Message);
            var MessagetoBeSent = map.Map<MessageForCreationDto>(Message);
            if (await repo.SaveAll())
            {
                await _hubContext.Clients.All.BroadcastMessage(MessagetoBeSent);
                return Ok(MessagetoBeSent);
            }
            throw new Exception("Creating The Message Faild");

        }

        [HttpPost]
        public async Task<IActionResult> createMessage(string userId, MessageForCreationDto MessageForCreationDto)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            MessageForCreationDto.senderId = userId;
            var recipient = await repo.GetUser(MessageForCreationDto.recipientId);
            if (recipient is null)
            { return BadRequest("Can't Find User"); }
            var x = await repo.GetMainPhoto(userId);
            MessageForCreationDto.senderPhotoUrl = x.Url;
            var Message = map.Map<Message>(MessageForCreationDto);
            repo.Add(Message);
            var MessagetoBeSent = map.Map<MessageForCreationDto>(Message);
            if (await repo.SaveAll())
            {
                return CreatedAtRoute("getMessage", new { id = Message.Id }, MessagetoBeSent);
            }
            throw new Exception("Creating The Message Faild");
        }

        [HttpGet]
        public async Task<IActionResult> getMessages(string userId, [FromQuery]MessageParams messageParams)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }

            messageParams.UserId = userId;

            var Messages = await repo.GetMessages(messageParams);
            var messageToReturn = map.Map<IEnumerable<MessageForReturnDto>>(Messages);
            Response.AddPagination(Messages.CurrentPage, Messages.PageSize, Messages.TotalCount, Messages.TotalPages);
            return Ok(messageToReturn);
        }

        [HttpPost("signalR/delete/{id}")]
        public async Task<IActionResult> deleteMessage(int id, string userId)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var message = await repo.GetMessage(id);
            if (message.senderId == userId)
            {
                message.senderDeleted = true;
            }
            if (message.recipientId == userId)
            {
                message.recipientDeleted = true;
            }
            if (message.senderDeleted && message.recipientDeleted)
            {
                repo.Delete<Message>(message); if (await repo.SaveAll())
                {
                    return NoContent();
                }
            }



            if (message.senderDeleted || message.recipientDeleted)
            {
                await _hubContext.Clients.All.DeleteMessage(id);
                return NoContent();

            }

            throw new Exception("Can't Delete Message");
        }
        [HttpPost("signalR/read/{recipientId}")]
        public async Task<IActionResult> readMessage(string recipientId, string userId)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }

            var messages = await repo.GetMessageThread(userId, recipientId);
            if (userId == messages.FirstOrDefault().senderId)
                return NoContent();
            MarkAsRead(messages, userId);
            if (messages is null)
            {
                return NoContent();
            }
            List<int> ids = new List<int>();
            foreach (var item in messages)
            {
                ids.Add(item.Id);
            }
            await _hubContext.Clients.All.MarkMessages(ids);
            return NoContent();
        }

    }
}