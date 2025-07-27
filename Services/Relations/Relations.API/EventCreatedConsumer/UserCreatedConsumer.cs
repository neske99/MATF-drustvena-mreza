using EventBusMessages.Events;
using MassTransit;
using Relations.Common.Entities;
using Relations.Common.Repositories;

namespace Relations.API.EventBsConsumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IRelationsRepository _relationsRepository;

        public UserCreatedConsumer(IRelationsRepository relationRepository)
        {
            _relationsRepository = relationRepository ?? throw new ArgumentNullException(nameof(relationRepository));
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            //TODO user replication
            try
            {
                var userCreated = context.Message;
                var newUser = new User()
                {
                    Id = userCreated.UserId
                };

                await _relationsRepository.CreateUser(newUser);
                await context.ConsumeCompleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
