using Chat.Repository.Repositories.Contracts;
using EventBusMessages.Events;
using MassTransit;
using Services.Chat.Chat.Model.Entities;

namespace Chat.API.EventBsConsumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IChatRepository _chatRepository;

        public UserCreatedConsumer(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            //TODO user replication
            try
            {
                var userCreated = context.Message;
                var newUser = new User()
                {
                    Username = userCreated.UserName,
                    CreatedDate = userCreated.CreationDate,
                    Id = userCreated.UserId
                };

                await _chatRepository.ReplicateUser(newUser);
                await context.ConsumeCompleted;
            }
            catch
            {
                throw;
            }



        }
    }
}