using EventBusMessages.Events;
using MassTransit;
using Post.Application.Contracts;
using Post.Domain.Entities;

namespace Post.API.EventBsConsumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IPostRepository _postRepository;

        public UserCreatedConsumer(IPostRepository postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
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

                await _postRepository.ReplicateUser(newUser);
                await context.ConsumeCompleted;
            }
            catch (Exception ex)
            {
                throw;
            }



        }
    }
}
