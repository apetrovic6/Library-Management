using AutoMapper;
using BooksGQL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using StrawberryShake;
using WebClient.DTO;

namespace WebClient.Pages.Books.BookDetail;

public class EditBookBase : ComponentBase
{
    [Inject] private IMapper _mapper { get; set; }
    [Inject] private BooksClient client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Parameter] public int BookId { get; set; }

    protected IGetBookById_BookById_Book? Book { get; set; }

    protected void GoToEditPage(IGetBookById_BookById_Book? book)
    {
        Navigation.NavigateTo($"/book/{book?.Id}/edit");
    }

    protected UpdateBookDto model { get; set; } = new();

    protected async void OnValidSubmit(EditContext context)
    {
        var res = await client.UpdateBook.ExecuteAsync(BookId, _mapper.Map<UpdateBookInput>(model));
        if (res.IsSuccessResult())
        {
            Snackbar.Add($"Book {res?.Data?.UpdateBook?.Book?.Title} Updated Successfully", Severity.Success);
            Navigation.NavigateTo($"/book/{res?.Data?.UpdateBook?.Book?.Id}");
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
        model = new()
        {
            Title = Book.Title,
            Author = Book.Author,
            Year = Book.Year,
            Country = Book.Country,
            Language = Book.Language,
            Stock = Book.Stock,
            ImageLink = Book.Imagelink
        };
    }
}