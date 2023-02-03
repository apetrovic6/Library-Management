namespace Authors.Models;

public class PagedResult<T>
{
    public PagedResult()
    {
    }

    public PagedResult(List<T> data, AuthorPageInfo pageInfo)
    {
        Data = data;
        PageInfo = pageInfo;
    }

    public List<T> Data { get; set; }
    public AuthorPageInfo PageInfo { get; set; } = new AuthorPageInfo() { Page = 1, PageSize = 10 };
}