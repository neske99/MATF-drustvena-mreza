using Relations.GRPC;
namespace Post.API.GrpcServices;

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

    public List<string> GetRelationShips(int userId,List<int>userIdList)
    {
        GetRelationsWithRequest request = new GetRelationsWithRequest();
        var user = new User();
        user.Id = userId;
        request.User = user;

        request.TargetUsers.AddRange(userIdList.Select(id => new User { Id = id }));

        var response = _grpcService.GetRelationsWith(request);
        return response.Relations.Select(r=>r.ToString()).ToList();
    }


}
