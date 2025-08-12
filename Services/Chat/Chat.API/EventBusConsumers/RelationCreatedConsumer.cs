using Chat.Chat.API;
using Chat.Repository.Repositories.Contracts;
using EventBusMessages.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Services.Chat.Chat.Model.Entities;

namespace Chat.API.EventBsConsumers
{
    public class RelationCreatedConsumer : IConsumer<RelationCreatedEvent>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHub> _chatHubContext;

    public RelationCreatedConsumer(IChatRepository chatRepository, IHubContext<ChatHub> chatHubContext)
    {
      _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
      _chatHubContext = chatHubContext ?? throw new ArgumentNullException(nameof(chatHubContext));
    }

    public async Task Consume(ConsumeContext<RelationCreatedEvent> context)
        {
            //TODO user replication
            try
            {
                var relationCreated = context.Message;


                var chatGroup=await _chatRepository.CreateChatGroupAsync(relationCreated.UserAId, relationCreated.UserBId);
                //TODO: Add users to chat group

                var userA = chatGroup.ChatUsers[0];
                var userB = chatGroup.ChatUsers[1];

                await _chatHubContext.Clients.User(userA.User.Id.ToString())
                    .SendAsync("ReceiveMessage", chatGroup.Id,userB.Id,userB.User.Username);

                await _chatHubContext.Clients.User(userB.User.Id.ToString())
                    .SendAsync("ReceiveMessage", chatGroup.Id,userA.Id,userA.User.Username);


                //await _chatHubContext.Groups.AddToGroupAsync(_chatHubContext.Clients.User("").,chatGroup.Id.ToString());

                //await _chatHubContext.Clients.User(relationCreated.UserAId.ToString()).SendAsync("ReceiveMessage", relationCreated.UserBId,relationCreated.);
                //_chatHubContext.Groups.AddToGroupAsync( relationCreated.UserBId.ToString(),);
                await context.ConsumeCompleted;
            }
            catch
            {
                throw;
            }



        }
    }
}