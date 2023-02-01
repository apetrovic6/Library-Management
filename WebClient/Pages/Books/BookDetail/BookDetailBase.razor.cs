using BooksGQL;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebClient.Components;
using WebClient.DTO;
using WebClient.Services.Interfaces;

namespace WebClient.Pages.Books.BookDetail;

public class BookDetailBase : ComponentBase
{
    [Inject] private IGenericService<Book> _bookService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService _dialogService { get; set; }
    [Parameter] public int BookId { get; set; }

    protected List<BreadcrumbItem> _breadcrumbItems;
    protected Book book { get; set; }

    protected void GoToEditPage()
    {
        Navigation.NavigateTo($"/book/{book?.Id}/edit");
    }

    protected void ShowDeleteDialog()
    {
     
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Do you really want to delete these records? This process cannot be undone.");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);
            parameters.Add("OnClickFunction", DeleteBook);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            _dialogService.Show<DeleteConfirmDialog>("Delete", parameters, options);
    }

    private async void DeleteBook()
    {
        var (deleted, errors) = await _bookService.Delete(book.Id);
        if (deleted)
        {
            Snackbar.Add($"Book {book.Title} deleted successfully", Severity.Success);
            Navigation.NavigateTo("/books");
        }
        else
        {
            foreach (var err in errors)
            {
                Snackbar.Add($"Error: {err.Message}", Severity.Error);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var res = await _bookService.GetById(BookId);

        book = res;
        
        _breadcrumbItems = new()
        {
            new BreadcrumbItem("Books", href: "/books"),
            new BreadcrumbItem(book?.Title, href: null, disabled: true),
        };
    }
}