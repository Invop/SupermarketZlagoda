namespace Zlagoda.Api;

public class ApiEndpoints
{
    private const string ApiBase = "api";
    public static class Products
    {
        private const string Base = $"{ApiBase}/products";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class StoreProducts
    {
        private const string Base = $"{ApiBase}/store-products";
        public const string Create = Base;
        public const string Get = $"{Base}/{{upc}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{prevUpc}}";
        public const string Delete = $"{Base}/{{upc}}";
    }
    
    public static class CustomerCards
    {
        private const string Base = $"{ApiBase}/customer-card";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }

    
    public static class Employees
    {
        private const string Base = $"{ApiBase}/employees";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Categories
    {
        private const string Base = $"{ApiBase}/categories";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
}