using Data.Identity;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    
    public class BookDbContext:IdentityDbContext<AppUser,AppRole,int>
	{
       
        public BookDbContext(DbContextOptions<BookDbContext>options):base(options) { }
		public DbSet<Books> Books { get; set; }
		public DbSet<BookTypes> BookTypes { get; set; }
		public DbSet<Borrow> Borrows { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
            //    builder.Entity<Borrow>()
            //.HasOne(bb => bb.Books)
            //.WithMany()
            //.HasForeignKey(bb => bb.BookId);

            //    builder.Entity<Borrow>()
            //        .HasOne(bb => bb.UserId)
            //        .WithMany()
            //        .HasForeignKey(bb => bb.UserId);
           builder.Entity<Borrow>()
    .HasOne(b => b.Book)
    .WithMany()
    .HasForeignKey(b => b.BookId)
    .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<BookTypes>().HasData(
				new BookTypes { Id = 1, Name = "Roman" },
				new BookTypes { Id = 2, Name = "Hikaye" },
				new BookTypes { Id = 3, Name = "Edebiyat" }

				);

			builder.Entity<Books>().HasData(
				new Books { Id = 1, BookName = "Yeni Hayat", Writer = "Orhan Pamuk", StockCount = 40, YearOfPublication = new DateTime(1970, 1, 1), BookTypesId =1 }


				);
		}
	}
}
