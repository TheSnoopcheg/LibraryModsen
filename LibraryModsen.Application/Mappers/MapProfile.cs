using AutoMapper;
using LibraryModsen.Application.Contracts.Auth;
using LibraryModsen.Application.Contracts.Author;
using LibraryModsen.Application.Contracts.Book;
using LibraryModsen.Application.Contracts.BookState;
using LibraryModsen.Application.Contracts.User;
using LibraryModsen.Domain.Models;

namespace LibraryModsen.Application.Mappers;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Book, BookCreationRequest>().ReverseMap();
        CreateMap<Book, BookEditRequest>().ReverseMap();
        CreateMap<Book, BookFullResponse>().ReverseMap();
        CreateMap<Book, BookResponse>().ReverseMap();

        CreateMap<Author, AuthorCreationRequest>().ReverseMap();
        CreateMap<Author, AuthorEditRequest>().ReverseMap();
        CreateMap<Author, AuthorFullResponse>().ReverseMap();
        CreateMap<Author, AuthorResponse>().ReverseMap();

        CreateMap<User, RegisterRequest>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();

        CreateMap<BookState, BookStateResponse>().ReverseMap();
    }
}
