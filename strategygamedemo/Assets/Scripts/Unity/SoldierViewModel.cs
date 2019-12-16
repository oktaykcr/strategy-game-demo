using System.Collections.Generic;
using UnityEngine;

public class SoldierViewModel : SpatialViewModel<IProduct>
{
    private List<Vector2> _paths;

    private GameObject _currentGroundTile;

    private GameObject _nextGroundTile;

    private GameObject _endGroundTile;

    private int _currentTileIndex = 1;

    [SerializeField]
    private LayerMask _tilesLayer;

    private Rigidbody2D _rigidbody2D;

    private float _moveSpeed;

    private void Start()
    {
        _moveSpeed = 2f;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _endGroundTile = _currentGroundTile;
    }

    protected override void OnUpdate()
    {
        if (IsMove())
        {
            if (_paths == null)
            {
                var startX = int.Parse(_currentGroundTile.name.Split('_')[0]);
                var startY = int.Parse(_currentGroundTile.name.Split('_')[1]);

                var endX = int.Parse(_endGroundTile.name.Split('_')[0]);
                var endY = int.Parse(_endGroundTile.name.Split('_')[1]);

                /*Debug.Log("Start: " + startX +"-"+ startY);
                Debug.Log("End: " + endX +"-"+ endY);*/

                var start = new int[] {startX, startY};
                var end = new int[] {endX, endY};

                _paths = new Astar(GameBoardViewModel.Instance.GetWalkableStatusMap(), start, end, "").Result;
                
                /*Debug.Log("FOUND PATHS : " + _paths.Count);
                Debug.Log(" First is: x:" + _paths[0].x + " y:" + _paths[0].y);
                Debug.Log(" Second is: x:" + _paths[1].x + " y:" + _paths[1].y);
                Debug.Log(" Third is: x:" + _paths[2].x + " y:" + _paths[2].y);
                Debug.Log(" Last is: x:" + _paths[_paths.Count - 1].x + " y:" + _paths[_paths.Count - 1].y);*/

                _nextGroundTile = GameObject.Find(_paths[_currentTileIndex].x + "_" + _paths[_currentTileIndex].y);

                // if there are paths so change status of the current ground tile as true
                if (_paths.Count > 1)
                {
                    SetCurrentGroundTileWalkable(true);
                }
            }
            else
            {
                DataContext.XPos = Vector2.MoveTowards(transform.position, _nextGroundTile.transform.position,
                    _moveSpeed * Time.deltaTime).x;
                DataContext.YPos = Vector2.MoveTowards(transform.position, _nextGroundTile.transform.position,
                    _moveSpeed * Time.deltaTime).y;

                // if the soldier reaches the nextGroundTile position
                if (transform.position == _nextGroundTile.transform.position)
                {
                    // then choose nex ground tile
                    if (_currentTileIndex < _paths.Count - 1)
                    {
                        _currentTileIndex += 1;
                        _currentGroundTile = _nextGroundTile;
                        _nextGroundTile = GameObject.Find(_paths[_currentTileIndex].x + "_" + _paths[_currentTileIndex].y);
                    }
                    else // if the paths finish, now current ground tile is end tile, set IsWalkable attribute as false
                    {
                        _paths = null;
                        _currentTileIndex = 0;
                        _currentGroundTile = _endGroundTile;
                        SetCurrentGroundTileWalkable(false);
                    }
                }
            }
        }
    }

    private bool IsMove()
    {
        return _currentGroundTile.name != _endGroundTile.name;
    }

    public void SetPosition(Vector2 position)
    {
        DataContext.XPos = position.x;
        DataContext.YPos = position.y;
    }

    public void SetEndGroundTile(GameObject endGroundTile)
    {
        _endGroundTile = endGroundTile;
    }

    public void SetCurrentGroundTile(GameObject currentGroundTile)
    {
        _currentGroundTile = currentGroundTile;
    }

    private void SetCurrentGroundTileWalkable(bool isWalkable)
    {
        _currentGroundTile.GetComponent<GroundTileViewModel>().IsWalkable = isWalkable;
    }

}
