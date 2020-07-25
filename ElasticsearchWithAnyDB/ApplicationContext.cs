using ElasticsearchWithAnyDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ElasticsearchWithAnyDB
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Product> Products { get; set; }

		public ApplicationContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(GetConnectionString());
		}

		private string GetConnectionString()
		{
			IConfiguration config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var uriString = config.GetConnectionString("postgres");
			var uri = new Uri(uriString);
			var db = uri.AbsolutePath.Trim('/');
			var user = uri.UserInfo.Split(':')[0];
			var passwd = uri.UserInfo.Split(':')[1];
			var port = uri.Port > 0 ? uri.Port : 5432;

			return $"Server={uri.Host};Database={db};User Id={user};Password={passwd};Port={port}";
		}
	}
}
