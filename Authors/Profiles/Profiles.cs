using Authors.DTO;
using Authors.Models;
using AutoMapper;

namespace Authors.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<List<AuthorEntity>, GetAuthorsResponse>()
            .ForMember(dest => dest.Authors,
                src => src.MapFrom(
                    x => x
                )
            );
        CreateMap<AuthorEntity, AuthorDTO>();
        CreateMap<AuthorDTO, AuthorEntity>();
        CreateMap<AuthorEntity, GetAuthorByIdResponse>()
            .ForMember(dest => dest.Author,
                src => src.MapFrom(
                    x => x
                ));
        CreateMap<CreateAuthorRequest, AuthorEntity>();
        CreateMap<AuthorEntity, CreateAuthorResponse>();
        CreateMap<AuthorEntity, UpdateAuthorResponse>()
            .ForMember(dest => dest.Author,
                src => src.MapFrom(
                    x => x));
        CreateMap<AuthorDTO, AuthorPublishedDto>();
    }
}