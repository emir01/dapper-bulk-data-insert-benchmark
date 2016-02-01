using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper.BulkInserts.Dtos;
using FastMember;

namespace Dapper.BulkInserts.DataAccess
{
    public class ProductWriter
    {
        public void WriteSingleProduct(Product product)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductDapperDb"].ConnectionString))
            {
                connection.Execute(Commands.WriteOne, product);
            }
        }

        public void WriteProductsWithExecuteForEach(List<Product> products)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductDapperDb"].ConnectionString))
            {
                foreach (var product in products)
                {
                    connection.Execute(Commands.WriteOne, product);
                }
            }
        }

        public void WriteProductCollection(List<Product> products)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductDapperDb"].ConnectionString))
            {
                connection.Execute(Commands.WriteOne, products);
            }
        }

        public void WriteProductCollectionUsingDataTable(List<Product> products)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductDapperDb"].ConnectionString))
            {
                DataTable dataTable = GetDataTableForProducts(products);
                connection.Execute(Commands.BatchInsert, new { @data = dataTable.AsTableValuedParameter("dbo.ProductType") });
            }
        }
        
        private DataTable GetDataTableForProducts(List<Product> products)
        {

            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(products))
            {
                table.Load(reader);
            }
            
            table.SetColumnsOrder(
                "Id",
                "Name",
                "Description",
                "Price",
                "Location",
                "Category",
                "Manufacturer",
                "Condition"
                );

            return table;
        }

        #region Clear Db

        public void CleanProducts()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductDapperDb"].ConnectionString))
            {
                connection.Execute(Commands.ClearProducts);
            }
        }

        #endregion
    }
}
