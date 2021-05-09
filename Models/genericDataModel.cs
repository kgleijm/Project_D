using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_D.Models
{
    public class genericDataModel : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // KEKW
            String conString = ("User ID=postgres;" +
                                "Password=none;" +
                                "Host=localhost;" +
                                "port=5432;" +
                                "Database=Project_D_Data;" +
                                "Pooling=true; ");
            optionsBuilder.UseNpgsql(conString);
        }
    }
}