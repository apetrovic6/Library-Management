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

    protected List<BreadcrumbItem> _breadcrumbItems;
    
    protected void GoToEditPage()
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

        _breadcrumbItems = new()
        {
            new BreadcrumbItem("Books", href: "/books"),
            new BreadcrumbItem(Book?.Title, href: $"/book/{BookId}"),
            new BreadcrumbItem($"Update {Book?.Title}", href: null, disabled: true)
        };
    }
}