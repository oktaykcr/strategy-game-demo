﻿public class SoldierUnit : IProduct
{
    public float XPos { get; set; }
    public float YPos { get; set; }
    public float XSize { get; set; }
    public float YSize { get; set; }
    public float Opacity { get; set; }
    public string ProductName { get; set; }
    public ProductType type { get; set; }
    public bool IsTemplate { get; set; }

    public SoldierUnit(string productName, ProductType type, bool isTemplate, float opacity)
    {
        ProductName = productName;
        this.type = type;
        IsTemplate = isTemplate;
        Opacity = opacity;
    }
}
