using AutoMapper;
using Post.Application.DTOs;
using Post.Domain.Entities;

namespace Post.Application.Mapper
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            //Get DTOs
            CreateMap<User, GetUserDTO>().ReverseMap();

            CreateMap<Comment, GetCommentDTO>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)).ReverseMap();

            CreateMap<Like, GetLikeDTO>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)).ReverseMap();

            CreateMap<Post.Domain.Entities.Post, GetPostDTO>().
            ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)).
            ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments)).
            ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes)).ReverseMap();

            //Set DTOs
            CreateMap<User, CreateUserDTO>().ReverseMap();

            CreateMap<Comment, CreateCommentDTO>().ReverseMap();

            CreateMap<Like, CreateLikeDTO>().ReverseMap();

            CreateMap<Post.Domain.Entities.Post, CreatePostDTO>().
            ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments)).ReverseMap();


        }
    }
}