using Microsoft.Data.SqlClient;

namespace Zlagoda.Application.Database;

 public class DbInitializer
 {
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        //dropDatabase(connection);
        await CreateTablesIfNotExists(connection);
    }
    private async Task CreateTablesIfNotExists(SqlConnection connection)
    {   
        await CreateCategoryTableIfNotExists(connection);
        await CreateProductTableIfNotExists(connection);
        await CreateStoreProductTableIfNotExists(connection);
        await CreateEmployeeTableIfNotExists(connection);
        await CreateCustomerCardTableIfNotExists(connection);
        await CreateCheckTableIfNotExists(connection);
        await CreateSaleTableIfNotExists(connection);
    }

    private async Task dropDatabase(SqlConnection connection)
    {
        string databaseName = "zlagoda";
        string dropDatabaseQuery = $"DROP DATABASE [{databaseName}]";
        
        await ExecuteNonQueryAsync(connection, dropDatabaseQuery);
    }
    
    private async Task CreateCheckTableIfNotExists(SqlConnection connection)
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

        await ExecuteNonQueryAsync(connection, checksTableCreateQuery);
    }

    private async Task CreateCustomerCardTableIfNotExists(SqlConnection connection)
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
    
        await ExecuteNonQueryAsync(connection, customerCardsTableCreateQuery);
        
    }

    private async Task CreateEmployeeTableIfNotExists(SqlConnection connection)
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
     
        await ExecuteNonQueryAsync(connection, employeesTableCreateQuery);
    }

    private async Task CreateProductTableIfNotExists(SqlConnection connection)
    {
        const string productsTableCreateQuery = """
                                                
                                                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Products')
                                                    CREATE TABLE Products
                                                    (
                                                        id_product UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
                                                        category_number INT FOREIGN KEY REFERENCES Categories(category_number) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                        product_name VARCHAR(50) NOT NULL,
                                                        characteristics VARCHAR(100) NOT NULL
                                                    )
                                                """;
        await ExecuteNonQueryAsync(connection, productsTableCreateQuery);
        
    }
    
    //must be updated by yourself UPC_prom On update NO ACTION ON DELETE NO ACTION
    //OnUpdateCascade
    //On delete set NUll
    private async Task CreateStoreProductTableIfNotExists(SqlConnection connection)
    {
        const string storeProductsTableCreateQuery = """
                                                     IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Store_Products')
                                                     CREATE TABLE Store_Products
                                                     (
                                                         UPC VARCHAR(12) PRIMARY KEY NOT NULL,
                                                         UPC_prom VARCHAR(12),
                                                         id_product UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Products(id_product) ON UPDATE CASCADE ON DELETE NO ACTION,
                                                         selling_price DECIMAL(13,4) NOT NULL,
                                                         products_number INT NOT NULL,
                                                         promotional_product BIT NOT NULL
                                                     )
                                                     """;
        await ExecuteNonQueryAsync(connection, storeProductsTableCreateQuery);
    }
    private async Task CreateCategoryTableIfNotExists(SqlConnection connection)
    {
        const string categoriesTableCreateQuery = """
                                                  
                                                          IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Categories')
                                                          CREATE TABLE Categories
                                                          (
                                                              category_number INT PRIMARY KEY NOT NULL,
                                                              category_name VARCHAR(50) NOT NULL
                                                          )
                                                  """;
        await ExecuteNonQueryAsync(connection, categoriesTableCreateQuery);
        
    }

    private async Task CreateSaleTableIfNotExists(SqlConnection connection)
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

        await ExecuteNonQueryAsync(connection, salesTableCreateQuery);
    }
    private async Task ExecuteNonQueryAsync(SqlConnection connection, string commandText)
    {
        using var command = new SqlCommand(commandText, connection);
        await command.ExecuteNonQueryAsync();
    }
 }