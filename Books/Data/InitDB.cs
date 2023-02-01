using System.Text;
using System.Text.Json;
using Books.Models;

namespace Books.Data;

public static class InitDB
{
    public static async Task Init(BooksDbContext context,ILoggerFactory loggerFactory )
    {
        var logger = loggerFactory.CreateLogger("InitDB");
        logger.LogInformation("--> Loading Sample Data..");
        string jsonData =  File.ReadAllText("./Data/SampleData.json", Encoding.UTF8);
        List<SampleDataModel> data = JsonSerializer.Deserialize<List<SampleDataModel>>(jsonData);

        List<Book> books = new();

        logger.LogInformation("--> Transforming Data");
        foreach (var book in data)
        {
            var bookToAdd = new Book()
            {
                Title = book.title, 
                Stock = Random.Shared.Next(0, 100),
                Author = book.author,
                Country = book.country,
                Language = book.language,
                Year = book.year,
                ImageLink = book.imageLink,
                Description = book?.description,
                authorId = book.authorId
            };
            books.Add(bookToAdd);
        }

        try
        {
            logger.LogInformation("--> Saving to DB");
            if (!context.Books.Any())
            {
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            logger.LogError($"--> Error: {e.Message}");
            throw;
        }
    }
}