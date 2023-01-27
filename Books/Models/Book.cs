﻿using System.ComponentModel.DataAnnotations;

namespace Books.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public int Stock { get; set; }
}