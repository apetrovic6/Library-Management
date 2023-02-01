using BooksGQL;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using StrawberryShake;

namespace WebClient.Pages.Books;

public class BookDetailBase : ComponentBase
{
    [Inject] private BooksClient client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Parameter] public int BookId { get; set; }

    protected List<BreadcrumbItem> _breadcrumbItems;
    protected IGetBookById_BookById_Book? Book { get; set; }

    protected void GoToEditPage()
    {
        Navigation.NavigateTo($"/book/{Book?.Id}/edit");
    }

    protected async void DeleteBook()
    {
        var res = await client.DeleteBook.ExecuteAsync(Book.Id);
        if (res.IsSuccessResult())
        {
            Snackbar.Add($"Book {Book.Title} deleted successfully", Severity.Success);
            Navigation.NavigateTo("/books");
        }
        else
        {
            foreach (var err in res.Errors)
            {
                Snackbar.Add($"Error: {err.Message}", Severity.Error);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var res = await client.GetBookById.ExecuteAsync(BookId);

        Book = res.Data?.BookById.Book;
        
        _breadcrumbItems = new()
        {
            new BreadcrumbItem("Books", href: "/books"),
            new BreadcrumbItem(Book?.Title, href: null, disabled: true),
        };
    }
}