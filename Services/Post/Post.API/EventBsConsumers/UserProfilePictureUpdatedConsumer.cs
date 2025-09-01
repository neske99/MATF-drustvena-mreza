using EventBusMessages.Events;
using MassTransit;
using Post.Application.Contracts;

namespace Post.API.EventBsConsumers
{
    public class UserProfilePictureUpdatedConsumer : IConsumer<UserProfilePictureUpdatedEvent>
    {
        private readonly IPostRepository _postRepository;

        public UserProfilePictureUpdatedConsumer(IPostRepository postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        public async Task Consume(ConsumeContext<UserProfilePictureUpdatedEvent> context)
        {
            try
            {
                var profileUpdateEvent = context.Message;
                
                // Update user's profile picture in Post service database
                await _postRepository.UpdateUserProfilePicture(profileUpdateEvent.UserId, profileUpdateEvent.ProfilePictureUrl);
                
                await context.ConsumeCompleted;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}