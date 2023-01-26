﻿using Books;
using Grpc.Net.Client;

namespace Gateway.GraphQL;

public class Query
{
    public async Task<HelloReply>  GetHello(string name)
    {
        var channel = GrpcChannel.ForAddress("http://books-clusterip-service");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(new HelloRequest{ Name = name});
        Console.WriteLine(reply);
        return  reply;
    }

    public async Task<GetBooksResponse> GetBooks()
    {
        var channel = GrpcChannel.ForAddress("http://books-clusterip-service");
        var client = new Books.Books.BooksClient(channel);
        var reply = await client.GetBooksAsync(new GetBooksRequest());
        Console.WriteLine(reply.Books);
        
        return reply;
    }
}