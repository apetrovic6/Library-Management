using AutoMapper;
using BooksGQL;
using WebClient.DTO;

namespace WebClient.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<AddBookDto, CreateBookInput>();
        CreateMap<UpdateBookDto, UpdateBookInput>();
    }
}