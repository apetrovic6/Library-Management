using BooksGQL;
using Microsoft.AspNetCore.Components;

namespace WebClient.Pages.Books;

public class BooksBase : ComponentBase
{
    [Inject] private BooksClient _client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    protected IGetBooksResult? FetchedBooks { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await _client.GetBooks.ExecuteAsync();

        FetchedBooks = result.Data;
    }

    protected void NavigateToBook(int Id)
    {
        Navigation.NavigateTo($"book/{Id}");
    }
}