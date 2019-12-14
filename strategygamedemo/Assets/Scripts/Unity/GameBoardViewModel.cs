using UnityEngine;

public class GameBoardViewModel : Singleton<GameBoardViewModel>
{
    [SerializeField]
    private RectTransform _gameBoardPanel;

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

    private GroundTileViewModel[,] _grounds;

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

        if (Input.GetMouseButtonDown(0) && _selectedProduct == null && IsMouseInGameBoard())
        {
            RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero, 2f);
            if (hit.collider != null)
            {
                var hitObject = hit.collider.gameObject;
                if (hitObject.tag.Equals("Building"))
                {
                    switch (hitObject.name)
                    {
                        case "Barrack":
                            GameManager.Instance.ShowProductInformation(GameManager.Products.Barrack);
                            break;
                        case "PowerPlant":
                            GameManager.Instance.ShowProductInformation(GameManager.Products.PowerPlant);
                            break;
                        case "SoldierUnit":
                            GameManager.Instance.ShowProductInformation(GameManager.Products.SoldierUnit);
                            break;
                    }
                }
            }
            else
            {
                GameManager.Instance.HideProductInformation();
            }
        }
    }

    /// <summary>
    /// Creates a map at the start of the game according to given attributes (width, height)
    /// and saves their models into array
    /// </summary>
    private void CreateMap()
    {
        var map = new GameObject("map");
        _grounds = new GroundTileViewModel[_height, _width];
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                var xPos = WorldStartPos.x + (_groundTile.bounds.size.x * x);
                var yPos = WorldStartPos.y + (_groundTile.bounds.size.y * y);

                var ground = new Ground(xPos, yPos, 1f, ""+x+y, true);
                var created = ViewFactory.Create<IGround, GroundTileViewModel>(ground, _groundPrefab, map.transform);
                created.name = "" + x + y;
                _grounds[y, x] = created.GetComponent<GroundTileViewModel>();
            }
        }
    }

    /// <summary>
    /// Selects specified product and shows information about the product
    /// </summary>
    /// <param name="product"></param>
    public void SelectProduct(GameObject product)
    {
        if (product != null)
        {
            _selectedProduct = product;
            if(_selectedProduct.name.Contains("Barrack")) 
                GameManager.Instance.ShowProductInformation(GameManager.Products.Barrack);
            else if(_selectedProduct.name.Contains("PowerPlant"))
                GameManager.Instance.ShowProductInformation(GameManager.Products.PowerPlant);
        }
    }

    /// <summary>
    /// Deselect the product, this method destroy the selected product if it is template,
    /// and hide the information of the product
    /// </summary>
    public void DeselectProduct()
    {
        if (_selectedProduct != null)
        {
            if (_selectedProduct.name.Contains("Template"))
            {
                Destroy(_selectedProduct);
            }
            _selectedProduct = null;
            GameManager.Instance.HideProductInformation();
        }
    }

    /// <summary>
    /// Checks the the player select any product
    /// </summary>
    /// <returns></returns>
    public bool IsProductSelected()
    {
        return _selectedProduct != null;
    }


    /// <summary>
    /// Checks the mouse position, if it is inside the game board panel then return true, otherwise false
    /// </summary>
    /// <returns></returns>
    public bool IsMouseInGameBoard()
    {
        Vector2 localMousePosition = _gameBoardPanel.InverseTransformPoint(Input.mousePosition);
        if (_gameBoardPanel.rect.Contains(localMousePosition))
        {
            return true;
        }

        return false;
    }
}
