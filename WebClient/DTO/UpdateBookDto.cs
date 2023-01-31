using System.ComponentModel.DataAnnotations;

namespace WebClient.DTO;

public class UpdateBookDto : AddBookDto
{
    [Required]
    public int Id { get; set; }
}