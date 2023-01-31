using BooksGQL;
using Microsoft.AspNetCore.Components;
using WebClient.DTO;
using WebClient.Services.Interfaces;

namespace WebClient.Pages.Books;

public class BooksBase : ComponentBase
{
    [Inject] private IGenericService<Book> _bookService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    protected List<Book> FetchedBooks { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await _bookService.GetAll();
        FetchedBooks = result;
    }

    protected void NavigateToBook(int Id)
    {
        Navigation.NavigateTo($"book/{Id}");
    }
}