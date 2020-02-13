using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Forum.Models.ArticlesManagement;

namespace Forum.Services.ArticlesManagement
{
    public class ArticleManagementMappingProfile : Profile
    {
        public ArticleManagementMappingProfile()
        {
            Categories result;
            CreateMap<ArticleRequest, Article>()
               .ForMember(dest => dest.Id,
                   opt => opt.MapFrom(src =>
                       (src.Id.Length == 36 ? new Guid(src.Id) : new Guid())))
               .ForMember(dest => dest.Content,
                   opt => opt.MapFrom(src => src.Content))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.Category, opt => opt.MapFrom(src =>
                   Enum.TryParse(src.Category, true, out  result) ? result : Categories.None))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                   src.UserName));
        }
    }
}
