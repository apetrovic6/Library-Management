using System.ComponentModel.DataAnnotations;

namespace Authors.Models;

public class AuthorEntity
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}