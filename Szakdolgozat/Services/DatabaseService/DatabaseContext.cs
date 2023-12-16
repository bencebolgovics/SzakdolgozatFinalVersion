using Microsoft.EntityFrameworkCore;
using Szakdolgozat.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public string DbPath { get; }

    public DatabaseContext()
    {
        DbPath = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Database\\books.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}