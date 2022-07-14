using System;
using AutoMapper;
using InternetAuction.BLL;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using InternetAuction.DAL.MSSQL;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Imternet.Tests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<MsSqlContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<MsSqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new MsSqlContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static void SeedData(MsSqlContext context)
        {
            context.Users.AddRange(
                new User { Id = "1", UserName = "Name1", Email = "Surname1", PasswordHash = "1111" },
                new User { Id = "2", UserName = "Name2", Email = "Surname2", PasswordHash = "dddd" });
            context.Roles.AddRange(
                new Role { Id = "1", Name = "Role1" },
                new Role { Id = "2", Name = "Role1" });
            /* context.UserRoles.AddRange(
                 new RoleUser { Id = "1" },
                 new RoleUser { Id = "2" });*/
            context.LotCategories.AddRange(
                new LotCategory { Id = 1, NameCategory = "NameCategory1", DescriptionCategory = "DescriptionCategory1" },
                 new LotCategory { Id = 2, NameCategory = "NameCategory2", DescriptionCategory = "DescriptionCategory2" });
            context.AutctionStatuses.AddRange(
                new AutctionStatus { Id = 1, NameStatus = "NameStatus1", DescriptionStatus = "DescriptionStatus1" },
                 new AutctionStatus { Id = 2, NameStatus = "NameStatus2", DescriptionStatus = "DescriptionStatus2" });
            context.SaveChanges();/*
            context.Products.AddRange(
                new Product { Id = 1, ProductCategoryId = 1, ProductName = "Name1", Price = 20 },
                new Product { Id = 2, ProductCategoryId = 2, ProductName = "Name2", Price = 50 });
            context.Receipts.AddRange(
                new Receipt { Id = 1, CustomerId = 1, OperationDate = new DateTime(2021, 7, 5), IsCheckedOut = true },
                new Receipt { Id = 2, CustomerId = 1, OperationDate = new DateTime(2021, 8, 10), IsCheckedOut = true },
                new Receipt { Id = 3, CustomerId = 2, OperationDate = new DateTime(2021, 10, 15), IsCheckedOut = false });
            context.ReceiptsDetails.AddRange(
                new ReceiptDetail { Id = 1, ReceiptId = 1, ProductId = 1, UnitPrice = 20, DiscountUnitPrice = 16, Quantity = 3 },
                new ReceiptDetail { Id = 2, ReceiptId = 1, ProductId = 2, UnitPrice = 50, DiscountUnitPrice = 40, Quantity = 1 },
                new ReceiptDetail { Id = 3, ReceiptId = 2, ProductId = 2, UnitPrice = 50, DiscountUnitPrice = 40, Quantity = 2 },
                new ReceiptDetail { Id = 4, ReceiptId = 3, ProductId = 1, UnitPrice = 20, DiscountUnitPrice = 18, Quantity = 2 },
                new ReceiptDetail { Id = 5, ReceiptId = 3, ProductId = 2, UnitPrice = 50, DiscountUnitPrice = 45, Quantity = 5 });
            context.SaveChanges();*/
        }
    }
}