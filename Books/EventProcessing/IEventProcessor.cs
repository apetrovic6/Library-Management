namespace Books.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}