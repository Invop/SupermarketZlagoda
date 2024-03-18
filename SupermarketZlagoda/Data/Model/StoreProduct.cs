namespace SupermarketZlagoda.Data.Model;

public record StoreProduct(
    string Upc,
    string UpcProm,
    int IdProduct,
    decimal SellingPrice,
    int ProductsNumber,
    bool PromotionalProduct
);