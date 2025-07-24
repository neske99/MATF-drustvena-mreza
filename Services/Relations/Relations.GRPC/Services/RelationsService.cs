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
        response.Friends.AddRange((IEnumerable<User>)friends);
        response.NumberOfFriends = response.Friends.Count;
        
        return response;
    }

    public override async Task<GetRelationsWithResponse> GetRelationsWith(GetRelationsWithRequest request, ServerCallContext context)
    {
        var user = request.User;
        var targetUsers = request.TargetUsers;

        var relations = new List<string>();
        foreach (var targetUser in targetUsers)
        {
            relations.Add(await _relationsRepository.GetRelation(user.Id, targetUser.Id));
        }

        var response = new GetRelationsWithResponse();
        response.Relations.AddRange((IEnumerable<Relation>)relations);

        return response;
    }
}
