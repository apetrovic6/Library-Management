using BooksGQL;
using Microsoft.AspNetCore.Components;
namespace WebClient.Pages.Books;

public class BookDetailBase : ComponentBase
{
    [Inject] private BooksClient client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    
    [Parameter]
    public int BookId { get; set; }

 
    protected  IGetBookById_BookById_Book? Book { get; set; }

    protected void GoToEditPage(IGetBookById_BookById_Book? book)
    {
        Navigation.NavigateTo($"/book/{book?.Id}/edit");
    } 
    
    protected override async Task OnInitializedAsync()
    {
     var res = await client.GetBookById.ExecuteAsync(BookId);

      Book = res.Data?.BookById.Book;
    }
}