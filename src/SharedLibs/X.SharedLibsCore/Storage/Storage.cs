//using Microsoft.EntityFrameworkCore;
//using NuGet;
//using Popcorn.Comparers;
//using Popcorn.Helpers;
//using Popcorn.Models.Bandwidth;
//using Popcorn.Models.Movie;
//using Popcorn.Models.Shows;
//using Popcorn.Services.Cache;
//using Popcorn.Services.Download;
//using Popcorn.Services.Movies.Movie;
//using Popcorn.Services.Shows.Show;
//using Popcorn.Services.Tmdb;
//using Popcorn.Utils;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace X.SharedLibsCore.Storage
//{
//    public class BloggingContext : DbContext
//    {
//        public DbSet<Blog> Blogs { get; set; }
//        public DbSet<Post> Posts { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlite("Data Source=blogging.db");
//        }
//    }

//    public class Blog
//    {
//        public int BlogId { get; set; }
//        public string Url { get; set; }
//        public string Title { get; set; }

//        public List<Post> Posts { get; set; }
//    }

//    public class Post
//    {
//        public int PostId { get; set; }
//        public string Title { get; set; }
//        public string Content { get; set; }

//        public int BlogId { get; set; }
//        public Blog Blog { get; set; }
//    }
//}
