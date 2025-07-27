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

    public List<string> GetRelationShips(int userId)
    {
        GetRelationsWithRequest request = new GetRelationsWithRequest();
        request.User.Id = userId;
        var response = _grpcService.GetRelationsWith(request);
        return response.Relations.Select(r => r.Relation_).ToList();
    }


}
