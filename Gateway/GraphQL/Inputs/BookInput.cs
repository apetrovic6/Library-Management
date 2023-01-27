namespace Gateway.GraphQL.Inputs;

public class BookInput : InputObjectType<BookInput>
{
    public string title { get; set; }
    public int stock { get; set; }
}