using Microsoft.EntityFrameworkCore;

namespace OnlineStore.DAL.Data;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
}
