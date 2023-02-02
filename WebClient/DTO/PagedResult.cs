using WebClient.DTO;

namespace Books.Models;

public class PagedResult<T>
{
    public PagedResult()
    {
    }

    public PagedResult(List<T> data, PageInfo pageInfo)
    {
        Data = data;
        PageInfo = pageInfo;
    }

    public List<T> Data { get; set; }
    public PageInfo PageInfo { get; set; } = new PageInfo() { Page = 1, PageSize = 10 };
}