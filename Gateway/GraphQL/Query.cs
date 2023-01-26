using Books;
using Grpc.Net.Client;

namespace Gateway.GraphQL;

public class Query
{
    public async Task<HelloReply>  GetHello()
    {
        var channel = GrpcChannel.ForAddress("http://books-clusterip-service");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(new HelloRequest{ Name = "manjo"});
        Console.WriteLine(reply);
        return  reply;
    }
}