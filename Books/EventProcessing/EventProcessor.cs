using System.Text.Json;
using Authors.DTO;
using AutoMapper;
using Books.Data;
using Books.DTO;
using Microsoft.EntityFrameworkCore;

namespace Books.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IDbContextFactory<BooksDbContext> _contextFactory;
    private readonly IMapper _mapper;
    
    public EventProcessor(IDbContextFactory<BooksDbContext> contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.AuthorUpdated:
                //TODO
                
            break;
                default:
                    break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Author_Updated":
                Console.WriteLine("--> Author Updated Event Detected");
                UpdateAuthor(notificationMessage);
                return EventType.AuthorUpdated;
            default:
                Console.WriteLine("--> Event type unknown");
                return EventType.Undetermined;
        }
    }

    private async void UpdateAuthor(string authorPublishedMessage)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var authorPublishedDto = JsonSerializer.Deserialize<AuthorPublishedDto>(authorPublishedMessage);
        Console.WriteLine(authorPublishedDto);
        try
        {
            var author = new AuthorPublishedDto() {Id = authorPublishedDto.Id, Name = authorPublishedDto.Name, Event = authorPublishedDto.Event};
            var books = await dbContext.Books.Where(x => x.authorId == author.Id).ToListAsync();

            foreach (var book in books)
            {
                book.Author = author.Name;
            }
            
            dbContext.Books.UpdateRange(books);
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not update {e.Message}");
        }
    }
}

enum EventType
{
    AuthorUpdated,
    Undetermined
}