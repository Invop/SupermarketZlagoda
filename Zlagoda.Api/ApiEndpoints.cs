﻿namespace Zlagoda.Api;

public class ApiEndpoints
{
    private const string ApiBase = "api";
    public static class Products
    {
        private const string Base = $"{ApiBase}/products";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAllSortedAscending = $"{Base}/asc";
        public const string GetAllSortedDescending = $"{Base}/desc";
        public const string GetAllUnused = $"{Base}/unused";
        public const string GetAllUnusedAndCurrent = $"{Base}/unused/{{id:guid}}";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class StoreProducts
    {
        private const string Base = $"{ApiBase}/store-products";
        private const string PromoBase = $"{Base}/promo";
        public const string Create = Base;
        public const string Get = $"{Base}/{{upc}}";
        public const string GetByPromoUpc = $"{PromoBase}/{{upc}}";
        public const string GetAll = Base;
        public const string GetAllPromo = PromoBase;
        public const string GetQuantityByUpcProm = $"{Base}/{{upcProm}}/quantity";
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
    
    public static class Checks
    {
        private const string Base = $"{ApiBase}/check";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Sales
    {
        private const string Base = $"{ApiBase}/sale";
        public const string Create = Base;
        public const string Get = $"{Base}/{{upc}}/{{check:guid}}";
        public const string GetById = $"{Base}/{{check:guid}}";
        public const string GetAll = Base;
        public const string Delete = $"{Base}/{{check:guid}}"; 
    }

}