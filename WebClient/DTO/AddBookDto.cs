using System.ComponentModel.DataAnnotations;

namespace WebClient.DTO;

public class AddBookDto
{
    [Required] 
    public string Title { get; set; }

    [Required] 
    public string Author { get; set; }

    [Required] 
    public int Year { get; set; }

    [Required] 
    public string Country { get; set; }

    [Required] 
    public string Language { get; set; }

    [Required] 
    public string ImageLink { get; set; }

    [Required, Range(0, int.MaxValue, ErrorMessage = "Can't be a negative number")]
    public int Stock { get; set; }
}

