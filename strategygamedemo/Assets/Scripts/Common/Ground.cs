
public class Ground : IGround
{
    public float XPos { get; set; }
    public float YPos { get; set; }
    public float XSize { get; set; }
    public float YSize { get; set; }
    public float Opacity { get; set; }
    public string Name { get; set; }
    public bool IsWalkable { get; set; }

    public Ground(float xPos, float yPos, float opacity, string name, bool isWalkable)
    {
        XPos = xPos;
        YPos = yPos;
        Opacity = opacity;
        Name = name;
        IsWalkable = isWalkable;
    }
}
