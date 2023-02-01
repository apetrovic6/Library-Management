using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using StrawberryShake;
using WebClient.DTO;
using WebClient.Services.Interfaces;

namespace WebClient.Pages.Books.BookDetail;

public class EditBookBase : ComponentBase
{
    [Inject] private IMapper _mapper { get; set; }
    [Inject] private IGenericService<Book> _bookService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Parameter] public int BookId { get; set; }

    protected Book? Book { get; set; }

    protected List<BreadcrumbItem> _breadcrumbItems;
    
    protected void GoToEditPage()
    {
        Navigation.NavigateTo($"/book/{book?.Id}/edit");
    }

    protected UpdateBookDto model { get; set; } = new();

    protected async void OnValidSubmit(EditContext context)
    {
        var (book, errors, isSuccess) = await _bookService.Update(Book.Id, model);
        if (isSuccess)
        {
            Snackbar.Add($"Book {res?.Data?.UpdateBook?.Book?.Title} Updated Successfully", Severity.Success);
            Navigation.NavigateTo($"/book/{res?.Data?.UpdateBook?.Book?.Id}");
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
        Book = res;
        model =  _mapper.Map<Book, UpdateBookDto>(Book);

        _breadcrumbItems = new()
        {
            new BreadcrumbItem("Books", href: "/books"),
            new BreadcrumbItem(Book?.Title, href: $"/book/{BookId}"),
            new BreadcrumbItem($"Update {Book?.Title}", href: null, disabled: true)
        };
    }
}