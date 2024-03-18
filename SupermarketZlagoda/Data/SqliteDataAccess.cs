using System.Data;
using Microsoft.Data.SqlClient;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Data;

public static class SqliteDataAccess
{
    private const string ConnectionStringMaster = "Server=localhost;"+
                                            "Database=master;"+
                                            "Integrated Security=True;" +
                                            "TrustServerCertificate=True;";
    private const string ConnectionStringZlagoda = "Server=localhost;"+
                                                  "Database=zlagoda;"+
                                                  "Integrated Security=True;" +
                                                  "TrustServerCertificate=True;";

    #region InitDatabaseAndTables
    public static void InitDatabaseAndTables()
    {
        using var connection = new SqlConnection(ConnectionStringMaster);
        connection.Open();
        Console.WriteLine("Connected successfully.");
        CreateDatabaseIfNotExists(connection);
        CreateTablesIfNotExists(connection);
    }
    private static void CreateDatabaseIfNotExists(SqlConnection connection)
    {
        const string dbCreateQuery = @"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name='zlagoda') CREATE DATABASE zlagoda";
        ExecuteNonQuery(connection, dbCreateQuery);
        Console.WriteLine("Database 'zlagoda' ensured. It's created if it was not existing.");
        connection.ChangeDatabase("zlagoda");
    }
    
    private static void CreateTablesIfNotExists(SqlConnection connection)
    {
        CreateCategoryTableIfNotExists(connection);
        CreateProductTableIfNotExists(connection);
        CreateStoreProductTableIfNotExists(connection);
        CreateEmployeeTableIfNotExists(connection);
        CreateCustomerCardTableIfNotExists(connection);
        CreateCheckTableIfNotExists(connection);
        CreateSaleTableIfNotExists(connection);
    }

    private static void CreateCheckTableIfNotExists(SqlConnection connection)
    {
        const string checksTableCreateQuery = """
                                              
                                                  IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Checks')
                                                  CREATE TABLE Checks
                                                  (
                                                      check_number VARCHAR(10) PRIMARY KEY NOT NULL,
                                                      id_employee VARCHAR(10) FOREIGN KEY REFERENCES Employees(id_employee) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                      card_number VARCHAR(13) FOREIGN KEY REFERENCES Customer_Cards(card_number) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                      print_date DATETIME NOT NULL,
                                                      sum_total DECIMAL(13,4) NOT NULL,
                                                      vat DECIMAL(13,4) NOT NULL
                                                  )
                                              """;

        ExecuteNonQuery(connection, checksTableCreateQuery);
    }

    private static void CreateCustomerCardTableIfNotExists(SqlConnection connection)
    {
        const string customerCardsTableCreateQuery = """
                                                     
                                                             IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Customer_Cards')
                                                             CREATE TABLE Customer_Cards
                                                             (
                                                                 card_number VARCHAR(13) PRIMARY KEY NOT NULL,
                                                                 cust_surname VARCHAR(50) NOT NULL,
                                                                 cust_name VARCHAR(50) NOT NULL,
                                                                 cust_patronymic VARCHAR(50),
                                                                 phone_number VARCHAR(13) NOT NULL,
                                                                 city VARCHAR(50),
                                                                 street VARCHAR(50),
                                                                 zip_code VARCHAR(9),
                                                                 [percent] INT NOT NULL
                                                             )
                                                     """;
    
        ExecuteNonQuery(connection, customerCardsTableCreateQuery);
        
    }

    private static void CreateEmployeeTableIfNotExists(SqlConnection connection)
    {
        const string employeesTableCreateQuery = """
                                                 
                                                         IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Employees')
                                                         CREATE TABLE Employees
                                                         (
                                                             id_employee VARCHAR(10) PRIMARY KEY NOT NULL,
                                                             empl_surname VARCHAR(50) NOT NULL,
                                                             empl_name VARCHAR(50) NOT NULL,
                                                             empl_patronymic VARCHAR(50),
                                                             empl_role VARCHAR(10) NOT NULL,
                                                             salary DECIMAL(13,4) NOT NULL,
                                                             date_of_birth DATE NOT NULL,
                                                             date_of_start DATE NOT NULL,
                                                             phone_number VARCHAR(13) NOT NULL,
                                                             city VARCHAR(50) NOT NULL,
                                                             street VARCHAR(50) NOT NULL,
                                                             zip_code VARCHAR(9) NOT NULL
                                                         )
                                                 """;
    
        ExecuteNonQuery(connection, employeesTableCreateQuery);
    }

    private static void CreateProductTableIfNotExists(SqlConnection connection)
    {
        const string productsTableCreateQuery = """
                                                
                                                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Products')
                                                        CREATE TABLE Products
                                                        (
                                                            id_product INT PRIMARY KEY NOT NULL,
                                                            category_number INT FOREIGN KEY REFERENCES Categories(category_number) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                            product_name VARCHAR(50) NOT NULL,
                                                            characteristics VARCHAR(100) NOT NULL
                                                        )
                                                """;
        ExecuteNonQuery(connection, productsTableCreateQuery);
        
    }
    
    //must be updated by yourself UPC_prom On update NO ACTION ON DELETE NO ACTION
    private static void CreateStoreProductTableIfNotExists(SqlConnection connection)
    {
        const string storeProductsTableCreateQuery = """
                                                     
                                                         IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Store_Products')
                                                         CREATE TABLE Store_Products
                                                         (
                                                             UPC VARCHAR(12) PRIMARY KEY NOT NULL,
                                                             UPC_prom VARCHAR(12) FOREIGN KEY REFERENCES Store_Products(UPC) ON UPDATE NO ACTION ON DELETE NO ACTION,
                                                             id_product INT FOREIGN KEY REFERENCES Products(id_product) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                             selling_price DECIMAL(13,4) NOT NULL,
                                                             products_number INT NOT NULL,
                                                             promotional_product BIT NOT NULL
                                                         )
                                                     """;
        ExecuteNonQuery(connection, storeProductsTableCreateQuery);
    }

    private static void CreateCategoryTableIfNotExists(SqlConnection connection)
    {
        const string categoriesTableCreateQuery = """
                                                  
                                                          IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Categories')
                                                          CREATE TABLE Categories
                                                          (
                                                              category_number INT PRIMARY KEY NOT NULL,
                                                              category_name VARCHAR(50) NOT NULL
                                                          )
                                                  """;
        ExecuteNonQuery(connection, categoriesTableCreateQuery);
    }

    private static void CreateSaleTableIfNotExists(SqlConnection connection)
    {
        const string salesTableCreateQuery = """
                                             
                                                 IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Sales')
                                                 CREATE TABLE Sales
                                                 (
                                                     UPC VARCHAR(12) NOT NULL FOREIGN KEY REFERENCES Store_Products(UPC) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                     check_number VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Checks(check_number) ON UPDATE CASCADE ON DELETE CASCADE,
                                                     product_number INT NOT NULL,
                                                     selling_price DECIMAL(13,4) NOT NULL,
                                                     PRIMARY KEY (check_number, UPC)
                                                 )
                                             """;

        ExecuteNonQuery(connection, salesTableCreateQuery);
    }
    private static void ExecuteNonQuery(SqlConnection connection, string commandText)
    {
        using var command = new SqlCommand(commandText, connection);
        command.ExecuteNonQuery();
    }
    #endregion
     public static async Task<List<StoreProduct>> FetchStoreProductsData()
    {
        var storeProducts = new List<StoreProduct>();
         await using var connection = new SqlConnection(ConnectionStringZlagoda);
        const string sqlQuery = """
                                SELECT UPC, UPC_prom, id_product, selling_price, products_number, promotional_product
                                FROM Store_Products
                                """;

        await using var command = new SqlCommand(sqlQuery, connection);
        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var upcProm = reader.IsDBNull(1) ? null : reader.GetString(1);

            var storeProduct = new StoreProduct(
                reader.GetString(0),
                upcProm,
                reader.GetInt32(2),
                reader.GetDecimal(3),
                reader.GetInt32(4),
                reader.GetBoolean(5)
            );
    
            storeProducts.Add(storeProduct);
        }

        return storeProducts;
    }

    public static async Task<List<Product>> FetchProductsData()
    {
        var products = new List<Product>();
        const string productsFetchQuery = "SELECT id_product, category_number, product_name, characteristics FROM Products";
        await using var connection = new SqlConnection(ConnectionStringZlagoda);
        await connection.OpenAsync();
        await using var command = new SqlCommand(productsFetchQuery, connection);
        var reader = await command.ExecuteReaderAsync();

        while(await reader.ReadAsync())
        {
            var idProduct = reader.GetInt32(0);
            var categoryNumber = reader.GetInt32(1);
            var productName = reader.GetString(2);
            var characteristics = reader.IsDBNull(3) ? null : reader.GetString(3);
                
            products.Add(new Product(idProduct, categoryNumber, productName, characteristics));
        }

        return products;
    }
}