using BooksGQL;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebClient.DTO;
using WebClient.Services.Interfaces;

namespace WebClient.Pages.Books;

public class BooksBase : ComponentBase
{
    [Inject] private IGenericService<Book> _bookService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    protected List<Book> FetchedBooks { get; set; } = new();
    protected PageInfo PageInfo { get; set; } = new() { Page = 1, PageSize = 10};
    protected  int PageCount { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await GetData();
    }

    private async Task GetData()
    {
        var input = new PagingInput() { Page = PageInfo.Page, PageSize = PageInfo.PageSize };
        var (result, errors, isSuccess) = await _bookService.GetAll(input);
        FetchedBooks = result.Data;
        PageInfo.Total = result.PageInfo.Total;
        PageCount  = (int)Math.Ceiling((double)PageInfo.Total / (double)PageInfo.PageSize);
        
        if (!isSuccess)
        {
            foreach (var err in errors)
            {
                _snackbar.Add($"Error: {err.Message}");
            }
        }
    }
    
    protected async void PageChanged(int page)
    {
        PageInfo.Page = page;
        await GetData();
        StateHasChanged();
    }

    protected void PageSizeChanged(int size)
    {
        PageInfo.PageSize = size;
    }

    protected async void ApplyFilters()
    {
        await GetData();
        StateHasChanged();
    }
    
    protected void NavigateToBook(int Id)
    {
        Navigation.NavigateTo($"book/{Id}");
    }
}