namespace KittyWorks.Coop.Web.Domain.Product;

public static class ProductModule
{
    public interface IAddProductContext
    {
        Action Commit { get; }
        Action<Product> AddProduct { get; }
    }

    public static void AddProduct(IAddProductContext context, string sku)
    {
        var product = new Product
        {
            Sku = sku,
        };

        context.AddProduct(product);
        context.Commit();
    }


    public interface IDeleteProductContext
    {
        Action Commit { get; }
        Action<Guid> DeleteProductById { get; }
    }

    public static void DeleteProduct(IDeleteProductContext context, Guid productId)
    {
        context.DeleteProductById(productId);
        context.Commit();
    }
}
