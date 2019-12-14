
public class GroundTileViewModel : SpatialViewModel<IGround>
{

    protected override void OnUpdate()
    {
       
    }

    public bool IsWalkable
    {
        get => DataContext.IsWalkable;
        set => DataContext.IsWalkable = value;
    }

    public float XPos => DataContext.XPos;

    public float YPos => DataContext.YPos;
}
