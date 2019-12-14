using UnityEngine;
using UnityEngine.UI;

public class GameBoardViewModel : Singleton<GameBoardViewModel>
{
    public Vector2 MousePosition { get; private set; }

    private GameObject _selectedProduct;

    #region Map

    [Header("Map Settings")]

    [SerializeField]
    private GameObject _groundPrefab;

    [SerializeField]
    private int _width;

    [SerializeField]
    private int _height;

    private Sprite _groundTile;
    
    private Vector3 WorldStartPos
    {
        get
        {
            var x = _groundTile.bounds.size.x * (_width / 2);
            var y = _groundTile.bounds.size.y * (_height / 2);
            var vec = new Vector3(x, y, 0);
            return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane)) - vec;
        }
    }

    #endregion

    private void Start()
    {
        _groundTile = _groundPrefab.GetComponent<SpriteRenderer>().sprite;
        CreateMap();
    }

    private void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectProduct();
        }
    }

    private void CreateMap()
    {
        var map = new GameObject("map");
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                var xPos = WorldStartPos.x + (_groundTile.bounds.size.x * x);
                var yPos = WorldStartPos.y + (_groundTile.bounds.size.y * y);

                var ground = new Ground(xPos, yPos, 1f, ""+x+y, true);
                var created = ViewFactory.Create<IGround, GroundTileViewModel>(ground, _groundPrefab, map.transform);
                created.name = "" + x + y;
            }
        }
        map.AddComponent<BoxCollider2D>();
    }

    public void SelectProduct(GameObject product)
    {
        _selectedProduct = product;
    }

    public void DeselectProduct()
    {
        if (_selectedProduct != null)
        {
            if (_selectedProduct.name.Contains("Template"))
            {
                Destroy(_selectedProduct);
            }
            _selectedProduct = null;
        }
    }

    public bool IsProductSelected()
    {
        return _selectedProduct != null;
    }

}
