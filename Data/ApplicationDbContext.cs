using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using LuxStayApi.Models;

namespace LuxStayApi.Data;

public class ApplicationDbContext : DbContext {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Home> Homes { get; set; }
    public DbSet<Tour> Tours { get; set; }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingTour> BookingTours { get; set; }
    // public DbSet<ImagesDetail> ImagesDetails { get; set; }


}

