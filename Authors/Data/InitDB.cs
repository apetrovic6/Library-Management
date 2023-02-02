using System.Text;
using System.Text.Json;
using Authors.Models;

namespace Authors.Data;

public static class InitDB
{
    public static async Task Init(AuthorsDbContext context, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("InitDB");
        logger.LogInformation("--> Loading Sample Data...");
        string jsonData = File.ReadAllText("./Data/SampleData.json", Encoding.UTF8);
        var data = JsonSerializer.Deserialize<List<SampleDataModel>>(jsonData);

        List<Author> authors = new(); 
        foreach (var author in data)
        {
            var a = new Author { Name = author.name };
            authors.Add(a);
        }
        
        try
        {
            logger.LogInformation("--> Saving to DB");
            if (!context.Authors.Any())
            {
                await context.Authors.AddRangeAsync(authors);
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