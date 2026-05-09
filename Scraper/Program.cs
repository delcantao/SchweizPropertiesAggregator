
using Commons;
using Data.Entities;

var dbContext = new AppDbContext();


Console.WriteLine(dbContext.Properties.ToList().First());