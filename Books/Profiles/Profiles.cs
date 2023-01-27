﻿using AutoMapper;
using Books.Models;

namespace Books.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<Book, BookModel>();
        CreateMap<List<Book>, GetBooksResponse>()
            .ForMember(dest => dest.Books,
                src => src.MapFrom(
                    x => x
                ));
        CreateMap<CreateBookRequest, Book>();
        CreateMap<Book, CreateBookResponse>()
            .ForMember(dest => dest.Book,
                src => src.MapFrom(
                    x => x
                ));

        CreateMap<Book, UpdateBookResponse>()
            .ForMember(dest => dest.Book,
                src => src.MapFrom(
                    b => b
                ));
    }
}