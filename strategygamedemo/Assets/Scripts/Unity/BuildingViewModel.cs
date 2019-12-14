using UnityEngine;

public class BuildingViewModel : SpatialViewModel<IProduct>
{
    [SerializeField]
    private LayerMask _tilesLayer;

    private BoxCollider2D _collider;

    void Start()
    {
        _collider = transform.GetComponent<BoxCollider2D>();
    }

    protected override void OnUpdate()
    {
        if (this.DataContext.IsTemplate && this.DataContext.type.Equals(ProductType.Building))
        {
            DataContext.XPos = Mathf.Round(GameBoardViewModel.Instance.MousePosition.x);
            DataContext.YPos = Mathf.Round(GameBoardViewModel.Instance.MousePosition.y);

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2) transform.position + _collider.offset, _collider.size, transform.eulerAngles.z,
                    Vector2.zero, Mathf.Infinity, _tilesLayer);

                Debug.Log(hits.Length);

                if (hits.Length > 0 && CheckTheAreaIsSuitableForBuild(hits))
                {
                    SetGroundTileAsIsNotWalkable(hits);
                    PlaceBuilding();
                }
            }
        }
    }

    /// <summary>
    /// Places the building on the ground
    /// Changes the name of the building by removing 'Template'
    /// </summary>
    private void PlaceBuilding()
    {
        gameObject.name = gameObject.name.Split('_')[0];
        DataContext.IsTemplate = false;
        DataContext.Opacity = 1f;
        GameBoardViewModel.Instance.DeselectProduct();
    }

    /// <summary>
    /// Sets the ground tile as a IsNotWalkable after placing the building
    /// </summary>
    /// <param name="hits"></param>
    private void SetGroundTileAsIsNotWalkable(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.tag.Equals("Ground"))
            {
                hit.collider.gameObject.GetComponent<GroundTileViewModel>().IsWalkable = false;
            }
        }
    }

    /// <summary>
    /// Checks the overlap area of the building which is template,
    /// if the area is not contain Building tag then the area is suitable for building
    /// </summary>
    /// <param name="hits">RaycastHits2D of building</param>
    /// <returns></returns>
    private bool CheckTheAreaIsSuitableForBuild(RaycastHit2D[] hits)
    {
        // Hits should be overlap area + itself so the hits count less than calculation,
        // is should not place the area
        if (hits.Length < DataContext.XSize * DataContext.YSize + 1) return false;

        foreach (var rayHit in hits)
        {
            var gameObject = rayHit.collider.gameObject;
            if (gameObject.tag.Equals("Building") && !gameObject.name.Contains("Template"))
            {
                GameManager.Instance.GiveNotSuitableAreaWarning();
                return false;
            }
        }

        return true;
    }


}
