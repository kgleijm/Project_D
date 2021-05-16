using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using project_D.Models;

public class project_DContext : DbContext
{
    public project_DContext(DbContextOptions<project_DContext> options) : base(options)
    {


    }


    public DbSet<Data> Data { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<User> User { get; set; }


}

