using BooksGQL;
using Microsoft.AspNetCore.Components;

namespace WebClient.Pages.Books;

public class BooksBase : ComponentBase
{
    [Inject] private BooksClient _client { get; set; }
    protected IGetBooksResult? fetchedBooks { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await _client.GetBooks.ExecuteAsync();

        fetchedBooks = result.Data;
    }
}