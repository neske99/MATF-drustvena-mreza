using Grpc.Core;
using Relations.Common.Repositories;


namespace Relations.GRPC.Services;

public class RelationsService : RelationsProtoService.RelationsProtoServiceBase
{
    private readonly IRelationsRepository _relationsRepository;

    public RelationsService(IRelationsRepository relationsRepository)
    {
        _relationsRepository = relationsRepository ?? throw new ArgumentNullException(nameof(relationsRepository));
    }

    public override async Task<GetFriendsResponse> GetFriends(GetFriendsRequest request, ServerCallContext context)
    {
        var friends = await _relationsRepository.GetUserFriends(request.User.Id);
        var response = new GetFriendsResponse();
        response.Friends.AddRange(friends.Where(x => x != null).Select(x => new Relations.GRPC.User() { Id = x.Id }));
        response.NumberOfFriends = response.Friends.Count;

        return response;
    }

    public override async Task<GetRelationsWithResponse> GetRelationsWith(GetRelationsWithRequest request, ServerCallContext context)
    {
        var user = request.User;
        var targetUsers = request.TargetUsers;

        var relations = new List<string>();
        var response = new GetRelationsWithResponse();
        foreach (var targetUser in targetUsers)
        {
            relations.Add(await _relationsRepository.GetRelation(user.Id, targetUser.Id));
            response.Relations.Add(new Relation { Relation_ = relations.Last() });
        }
        response.Relations.Add(new Relation { Relation_ = "FRIENDS_WITH" });

        //response.Relations.AddRange((IEnumerable<Relation>)relations);

        return response;
    }

    public override async Task<GetFriendRequestsResponse> GetFriendRequests(GetFriendRequestsRequests request, ServerCallContext context)
    {
        GetFriendRequestsResponse response = new GetFriendRequestsResponse();
        var userList = await _relationsRepository.GetReceivedFriendRequests(request.User.Id);
        foreach (var user in userList)
        {
            response.Users.Add(new Relations.GRPC.User { Id = user.Id});
        }

        return response;
  }

}
