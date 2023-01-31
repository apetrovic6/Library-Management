using System.ComponentModel.DataAnnotations;

namespace WebClient.DTO;

public class UpdateBookDto : CreateBookDto
{
    [Required]
    public int Id { get; set; }
}