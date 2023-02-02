using Authors.Models;
using Microsoft.EntityFrameworkCore;

namespace Authors.Data;

public class AuthorsDbContext : DbContext
{
    public AuthorsDbContext(DbContextOptions<AuthorsDbContext> options) : base(options) {}
    
    public DbSet<AuthorEntity> Authors { get; set; }
}