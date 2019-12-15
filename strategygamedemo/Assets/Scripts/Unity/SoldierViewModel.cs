using UnityEngine;

public class SoldierViewModel : SpatialViewModel<IProduct>
{

    [SerializeField]
    private LayerMask _tilesLayer;

    protected override void OnUpdate()
    {
        
    }

    public void SetPosition(Vector2 position)
    {
        DataContext.XPos = position.x;
        DataContext.YPos = position.y;
    }
}
