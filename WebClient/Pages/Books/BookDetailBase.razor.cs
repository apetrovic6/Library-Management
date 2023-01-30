using System.Threading.Tasks;
using BooksGQL;
using Microsoft.AspNetCore.Components;
namespace WebClient.Pages.Books;

public class BookDetailBase : ComponentBase
{
    [Inject] private BooksClient client { get; set; }
    
    [Parameter]
    public int bookId { get; set; }

    protected  IGetBookById_BookById_Book? book { get; set; }
     
    protected override async Task OnInitializedAsync()
    {
     var res = await client.GetBookById.ExecuteAsync(bookId);

      book = res.Data.BookById.Book;
    }
}