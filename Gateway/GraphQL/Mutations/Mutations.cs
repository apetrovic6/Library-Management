using Books;
using Grpc.Net.Client;

namespace Gateway.GraphQL.Mutations;

public class Mutations
{
    public async Task<CreateBookResponse> CreateBook(string name, int stock)
    {
        var channel = GrpcChannel.ForAddress("http://books-clusterip-service");
        var client = new Books.Books.BooksClient(channel);
        var request = new CreateBookRequest {Name = name, Stock = stock};
        CreateBookResponse reply = client.CreateBook(request);

        return reply;
    }

    public async Task<DeleteBookResponse> DeleteBook(int id)
    {
        var channel = GrpcChannel.ForAddress("http://books-clusterip-service");
        var client = new Books.Books.BooksClient(channel);
        var request = new DeleteBookRequest { Id = id };
        var reply = client.DeleteBook(request);
        return reply;
    }
}