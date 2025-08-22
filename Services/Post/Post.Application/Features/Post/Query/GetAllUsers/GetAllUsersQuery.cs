using MediatR;
using Post.Application.DTOs;
using Post.Domain.Entities;

namespace Post.Applictaion.Features.Posts.Query;

public class GetAllUsersQuery: IRequest<IEnumerable<GetUserDTO>>
{

}