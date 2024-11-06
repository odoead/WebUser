namespace Test;
using System;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;

public static class InmemoryTestDBGenerator
{
    public static DbContextOptions<DB_Context> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<DB_Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }
}
