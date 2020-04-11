namespace Model4You.Services.Data.Tests
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data;

    public class InitializeContext
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new ApplicationDbContext(options);

            return context;
        }
    }
}