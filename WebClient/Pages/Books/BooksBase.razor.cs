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
    
    protected string ChosenAuthor { get; set; }
    protected string[] FetchedAuthors { get; set; } = {};
    protected PageInfo PageInfo { get; set; } = new() { Page = 1, PageSize = 10};
    protected  int PageCount { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await GetData();
    }

    private async Task GetData()
    {
        var input = new PagingInput() { Page = PageInfo.Page, PageSize = PageInfo.PageSize };
        var filters = new BookFilterInput() { AuthorName = ChosenAuthor ?? ""};
        var (result, errors, isSuccess) = await _bookService.GetAll(input, filters);
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

    private async void GetAuthors(string name)
    {
        var res =await _Client.GetAuthorByName.ExecuteAsync(name);
        var authors = res?.Data?.AuthorByName.Authors;
        
        List<string> list = new();
        
        foreach (var author in authors)
        {
            list.Add(author.Name);
        }

        FetchedAuthors = list.ToArray();
    }

    protected Task<IEnumerable<string>> AuthorFilterFunction(string arg)
    {
        
        
        if (string.IsNullOrEmpty(arg))
            return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());
        
        GetAuthors(arg);

        var res = FetchedAuthors.AsEnumerable()
            .Where(x => x.Contains(arg, StringComparison.InvariantCultureIgnoreCase));
        
        return Task.FromResult(res);
    }
    
    protected void NavigateToBook(int Id)
    {
        Navigation.NavigateTo($"book/{Id}");
    }
}