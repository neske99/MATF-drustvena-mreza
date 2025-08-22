namespace Post.Infrastructure.Contracts;

public interface IRelationService
{
  List<int> GetFriends(int userId);
   List<string> GetRelationShips(int userId, List<int> userIdList);
}