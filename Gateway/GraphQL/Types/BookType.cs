﻿namespace Gateway.GraphQL.Types;

public class BookType
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Country { get; set; }
    public string Language { get; set; }
    public string ImageLink { get; set; }
    public int Stock { get; set; }
}