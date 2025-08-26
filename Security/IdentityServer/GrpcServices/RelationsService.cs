using Relations.GRPC;

namespace IdentityServer.GrpcServices;

public class RelationsService
{
    //privete readonly Greeter.GreeterClient _greeterService;
    private readonly RelationsProtoService.RelationsProtoServiceClient _grpcService;

    public RelationsService(RelationsProtoService.RelationsProtoServiceClient greeterService)
    {

        _grpcService = greeterService ?? throw new ArgumentNullException(nameof(greeterService));
    }
    public List<int> GetFriends(int userId)
    {
        GetFriendsRequest request = new GetFriendsRequest();
        var usr = new User();
        usr.Id = userId;
        request.User = usr;

        var response = _grpcService.GetFriends(request);
        return response.Friends.Select(u => u.Id).ToList();

    }

    public List<string> GetRelationShips(int userId, List<int> userIdList)
    {
        try
        {
            GetRelationsWithRequest request = new GetRelationsWithRequest();
            var user = new User();
            user.Id = userId;
            request.User = user;

            request.TargetUsers.AddRange(userIdList.Select(id => new User { Id = id }));

            var response = _grpcService.GetRelationsWith(request);
            return response.Relations.Select(r => r.Relation_).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetRelationShips: {ex.Message}");
            // Return default relationships for all users to prevent crashes
            return userIdList.Select(_ => "NONE").ToList();
        }
    }

    public List<int> GetFriendRequestUserIdList(int userId)
    {

        return _grpcService.GetFriendRequests(new GetFriendRequestsRequests { User = new User { Id = userId } })
            .Users.Select(u => u.Id).ToList();
    }

}
