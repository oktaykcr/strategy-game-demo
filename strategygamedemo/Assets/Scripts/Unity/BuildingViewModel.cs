using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingViewModel : SpatialViewModel<IProduct>
{
    [SerializeField]
    private LayerMask _tilesLayer;

    private BoxCollider2D _collider;

    private List<GameObject> _overlapGroundTileList;

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
                    Vector2.zero, 2f, _tilesLayer);

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
        _overlapGroundTileList = new List<GameObject>();
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.tag.Equals("Ground"))
            {
                hit.collider.gameObject.GetComponent<GroundTileViewModel>().IsWalkable = false;
                _overlapGroundTileList.Add(hit.collider.gameObject);
            }
        }

        // sorting the overlap list to get first and last ground tile properly for spawning soldier unit
        _overlapGroundTileList = _overlapGroundTileList.OrderBy(o => o.GetHashCode()).ToList();
        _overlapGroundTileList.Reverse();
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
            var hitObject = rayHit.collider.gameObject;
            if ((hitObject.tag.Equals("Building") || hitObject.tag.Equals("Soldier")) && !hitObject.name.Contains("Template"))
            {
                GameManager.Instance.GiveNotSuitableAreaWarning();
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Spawns a soldier around the barrack if the ground tile is empty (IsWalkable = false)
    /// according to name index of overlapped grounds of barrack and looks left, bottom, right and upper
    /// ground tiles
    /// </summary>
    /// <param name="groundTileModels">the groundTileViewModel of created ground tiles at the start of the game</param>
    /// <returns></returns>
    public GameObject GetEmptyGroundTileNearTheSelectedBarrack(GroundTileViewModel[,] groundTileModels)
    {
        var firstGroundTileModel = _overlapGroundTileList[0].GetComponent<GroundTileViewModel>();
        var lastGroundTileModel = _overlapGroundTileList[_overlapGroundTileList.Count - 1].GetComponent<GroundTileViewModel>();

        // Getting indices of ground tile from ground tile name (Pattern is x_y)
        var firstIndexNumberX = int.Parse(firstGroundTileModel.name.Split('_')[0]);
        var firstIndexNumberY = int.Parse(firstGroundTileModel.name.Split('_')[1]);

        var lastIndexNumberX = int.Parse(lastGroundTileModel.name.Split('_')[0]);
        var lastIndexNumberY = int.Parse(lastGroundTileModel.name.Split('_')[1]);

        // Checking the indices are valid numbers for out of bound
        if (firstIndexNumberX - 1 < 0) firstIndexNumberX+=1;
        if (firstIndexNumberY - 1 < 0) firstIndexNumberY+=1;
        if (lastIndexNumberX + 1 > groundTileModels.GetLength(0) - 1) lastIndexNumberX-=1;
        if (lastIndexNumberY + 1 > groundTileModels.GetLength(1) - 1) lastIndexNumberY-=1;

        for (int i = firstIndexNumberX - 1, j = firstIndexNumberY - 1; i <= lastIndexNumberX + 1 && j <= lastIndexNumberY + 1; i++, j++)
        {
            // checks the indices greater and equals 0 and less than number of ground tile length
            if (i >= 0 && j < groundTileModels.GetLength(1))
            {
                var leftCheck = groundTileModels[firstIndexNumberX - 1, j];
                var bottomCheck = groundTileModels[i, firstIndexNumberY - 1];
                var rightCheck = groundTileModels[lastIndexNumberX + 1, j];
                var upperCheck = groundTileModels[i, lastIndexNumberY + 1];

                if (leftCheck.IsWalkable)
                {
                    return GameObject.Find(leftCheck.name);
                }

                if (bottomCheck.IsWalkable)
                {
                    return GameObject.Find(bottomCheck.name);
                }

                if (rightCheck.IsWalkable)
                {
                    return GameObject.Find(rightCheck.name);
                }

                if (upperCheck.IsWalkable)
                {
                    return GameObject.Find(upperCheck.name);
                }
            }
        }

        return null;
    }


}
