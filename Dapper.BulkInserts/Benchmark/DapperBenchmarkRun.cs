using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.BulkInserts.Benchmark
{
    public class DapperBenchmarkRun
    {
        public TimeSpan InsertUsingForSingleInsertElapsed { get; set; }

        public TimeSpan InsertUsingDapperCollectionElapsed { get; set; }
        public TimeSpan InsertUsingDataTableParameter { get; set; }
    }

    public class DapperBenchmarkResult
    {
        public int NumberOfEntriesToInsertPerRun { get; set; }

        public int NumberOfRuns { get; set; }

        public DateTime StartTime { get; set; }

        public List<DapperBenchmarkRun> Results { get; set; }

        public DapperBenchmarkResult()
        {
            Results = new List<DapperBenchmarkRun>();
            StartTime = DateTime.UtcNow;
        }

        public DapperBenchmarkResult(int numberOfEntriesToInsertPerRun, int numberOfRuns) : this()
        {
            NumberOfEntriesToInsertPerRun = numberOfEntriesToInsertPerRun;
            NumberOfRuns = numberOfRuns;
        }

        public void Add(TimeSpan singleInsertTime, TimeSpan collectionInsertTime, TimeSpan dataTableInsertTime)
        {
            Results.Add(new DapperBenchmarkRun
            {
                InsertUsingDapperCollectionElapsed = collectionInsertTime,
                InsertUsingForSingleInsertElapsed = singleInsertTime,
                InsertUsingDataTableParameter = dataTableInsertTime
            });
        }

        public string RenderResults()
        {
            var builder = new StringBuilder();
            builder.Append("======================================================== \n");
            builder.Append(
                $"Displaying the results of {NumberOfRuns} runs with {NumberOfEntriesToInsertPerRun} entries per run");

            builder.Append("\n");

            builder.Append("Single Insert \t \t || \t Collection Insert \t || \t Data Table Insert \n");
            builder.Append("==================================================================================================\n");

            foreach (var resultSet in Results)
            {
                builder.Append(
                    $"{resultSet.InsertUsingForSingleInsertElapsed} \t || \t {resultSet.InsertUsingDapperCollectionElapsed} \t || \t {resultSet.InsertUsingDataTableParameter} \n");
            }

            return builder.ToString();
        }
    }
}
