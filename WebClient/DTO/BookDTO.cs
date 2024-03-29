﻿using System.ComponentModel.DataAnnotations;

namespace WebClient.DTO;

public class BookDto
{
    [Required]
    public int Id { get; set; }
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
    [Required]
    public int Stock { get; set; }
    public string? Description { get; set; }
}