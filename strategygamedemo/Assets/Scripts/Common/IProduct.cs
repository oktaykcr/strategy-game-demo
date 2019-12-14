
public interface IProduct : ISpatial
{
    string ProductName { get; set; }
    ProductType type { get; set; }
    bool IsTemplate { get; set; }
}
