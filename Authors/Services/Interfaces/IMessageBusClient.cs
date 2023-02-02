using Authors.DTO;

namespace Authors.Services.Interfaces;

public interface IMessageBusClient
{
    void Publish(AuthorPublishedDto authorPublishedDto);
}