﻿using AutoMapper;
using Books;
using Gateway.GraphQL.Types;

namespace Gateway.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<BookModel, BookType>();
        CreateMap<DeleteBookResponse, BookType>();
    }
}