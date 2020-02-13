using AutoMapper;
using Forum.Models.CommentsManagement;
using System;

namespace Forum.Services.CommentsManagement
{
    public class CommentManagementMappingProfile : Profile
    {
        public CommentManagementMappingProfile()
        {
            CreateMap<CommentRequest, Comment>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src =>
                        (src.Id.Length == 36 ? new Guid(src.Id) : new Guid())))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => new Guid(src.ArticleId)));
        }
    }
}
