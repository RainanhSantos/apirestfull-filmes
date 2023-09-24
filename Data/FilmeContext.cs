using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apifilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace apifilmes.Data;

public class FilmeContext : DbContext
{
    public FilmeContext(DbContextOptions<FilmeContext> options)
        : base(options)
    {
        
    }

    public DbSet<Filme> Filmes { get; set; }
}