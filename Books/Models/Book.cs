using System.ComponentModel.DataAnnotations;

namespace Books.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    public string Language { get; set; }
    
    public string ImageLink { get; set; }
    
    [Required]
    public int Stock { get; set; }
    
    public string? Description { get; set; }
}