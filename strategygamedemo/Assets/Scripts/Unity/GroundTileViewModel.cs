using UnityEngine;

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

}
