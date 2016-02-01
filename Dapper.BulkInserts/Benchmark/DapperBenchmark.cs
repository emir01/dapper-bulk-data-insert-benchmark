using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dapper.BulkInserts.DataAccess;
using Dapper.BulkInserts.Dtos;
using Dapper.BulkInserts.TestData;
using static System.Console;

namespace Dapper.BulkInserts.Benchmark
{
    public class DapperBenchmark
    {
        private readonly ProductWriter productWriter;

        private readonly List<Product> products;

        public DapperBenchmark()
        {
            productWriter = new ProductWriter();
            products = new List<Product>();
        }

        public DapperBenchmarkResult RunBenchmark(int numberOfEntriesToInsertPerRun, int numberOfRuns)
        {
            var result = new DapperBenchmarkResult(numberOfEntriesToInsertPerRun, numberOfRuns);

            WriteLine(
                $"Starging a dapper Benchmark run with {numberOfEntriesToInsertPerRun} per run, repeated {numberOfRuns} times");

            GenerateProducts(numberOfEntriesToInsertPerRun);

            for (int i = 0; i < numberOfRuns; i++)
            {
                WriteLine(@"Cleaning Database!");
                productWriter.CleanProducts();

                WriteLine(@"====================================================");
                WriteLine($"Starting run number {i + 1}");
                WriteLine(@"====================================================");

                WriteLine(@"Running inserts with separate Dapper Execute Calls");
                WriteLine(@"Running connection.Execute(Commands.WriteOne, product);");
                var insertUsingForLoopTime = InsertUsingForLoop();

                WriteLine(@"Cleaning Database!");
                productWriter.CleanProducts();

                WriteLine(@"===============================================");
                WriteLine(@"Running inserts with Single Dapper Execute Call and Passing a Collection!");
                WriteLine(@"Running connection.Execute(Commands.WriteOne, products);");
                var insertUsingDapperCollectionTime = InsertUsingDapperCollectionInsert();

                WriteLine(@"Cleaning Database!");
                productWriter.CleanProducts();

                WriteLine(@"===============================================");
                WriteLine(@"Running inserts with Data Table, table value parameter!");
                WriteLine(@"connection.Execute(Commands.BatchInsert, new { @data = dataTable.AsTableValuedParameter(""dbo.ProductType"") });");
                var insertUsingBatcAndTableValueParamTime = InsertUsingDataTable();
                
                WriteLine(@"Cleaning Database!");
                productWriter.CleanProducts();

                result.Add(insertUsingForLoopTime, insertUsingDapperCollectionTime, insertUsingBatcAndTableValueParamTime);
            }

            return result;
        }

        private TimeSpan InsertUsingForLoop()
        {
            var stopwatch = Stopwatch.StartNew();

            //foreach (var product in products)
            //{
            //    productWriter.WriteSingleProduct(product);
            //}

            productWriter.WriteProductsWithExecuteForEach(products);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private TimeSpan InsertUsingDapperCollectionInsert()
        {
            var stopwatch = Stopwatch.StartNew();

            productWriter.WriteProductCollection(products);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private TimeSpan InsertUsingDataTable()
        {
            var stopwatch = Stopwatch.StartNew();

            productWriter.WriteProductCollectionUsingDataTable(products);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private void GenerateProducts(int numberOfProducts)
        {
            if (products.Count >= numberOfProducts) return;

            var productsToGenerate = numberOfProducts - products.Count;
            products.AddRange(TestDataGenerator.GetProductCollection(productsToGenerate));
        }
    }
}
