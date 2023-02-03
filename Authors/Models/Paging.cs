namespace Authors.Models;

public class Paging
{
    public Paging(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; set; }
    public int PageSize { get; set; }

    public int Skip
    {
        get { return PageSize * (Page - 1); }
    }
}