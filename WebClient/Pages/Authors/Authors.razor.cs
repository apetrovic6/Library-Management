using LibraryGQL;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebClient.DTO.Authors;
using WebClient.Services.Interfaces;

namespace WebClient.Pages.Authors;

public class AuthorsBase : ComponentBase
{
    [Inject] private IGenericService<AuthorDto> _authorService { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }
    protected List<AuthorDto> FetchedAuthors { get; set; } = new();

    private async Task GetData()
    {
        var input = new PagingInput { Page = 1, PageSize = 10 };
        var filter = new AuthorFilterInput { AuthorName = "" };
        var (res, errors, isSuccessful) = await _authorService.GetAll(input, filter);

        if (!isSuccessful)
        {
            foreach (var err in errors)
            {
                _snackbar.Add($"Error: {err.Message}");
            }
        }

        FetchedAuthors = res.Data;
    }

    protected override async Task OnInitializedAsync()
    {
       await  GetData();
    }
}