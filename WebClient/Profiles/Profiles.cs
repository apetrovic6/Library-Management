using AutoMapper;
using BooksGQL;
using WebClient.DTO;

namespace WebClient.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<CreateBookDto, CreateBookInput>();
        CreateMap<UpdateBookDto, UpdateBookInput>();
        CreateMap<UpdateBookInput, UpdateBookDto>();
        CreateMap<Book, UpdateBookDto>()
            .ForMember(dest => dest.Description, opt => opt.NullSubstitute(""));
        CreateMap<IGetBooks_Books, Book>();
        CreateMap<IGetBookById_BookById_Book, Book>();
        CreateMap<ICreateBook_CreateBook_Book, Book>();
        CreateMap<IUpdateBook_UpdateBook_Book, Book>();
        CreateMap<IGetBooks_Books_Data, Book>();
    }
}