using System;
using System.Collections.Generic;
using Dapper.BulkInserts.Dtos;
using Faker;

namespace Dapper.BulkInserts.TestData
{
    public static class TestDataGenerator
    {
        public static Product GetProduct()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Category = StringFaker.Alpha(10),
                Condition = TextFaker.Sentences(4),
                Description = TextFaker.Sentences(7),
                Name = StringFaker.Alpha(40),
                Location = LocationFaker.City(),
                Manufacturer = StringFaker.Alpha(25),
                Price = NumberFaker.Number(1, 500)
            };

            return product;
        }


        public static List<Product> GetProductCollection(int count)
        {
            var products = new List<Product>();

            for (int i = 0; i < count; i++)
            {
                products.Add(GetProduct());
            }

            return products;
        }
    }
}
