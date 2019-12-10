using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollView : MonoBehaviour
{
    private ScrollRect _scrollRect;

    [SerializeField]
    private List<GameObject> _itemList;

    [SerializeField]
    private int _numberOfItems;

    [Header("Layout Settings")]

    [SerializeField]
    private int _cellSize;

    [SerializeField]
    private int _spacing;

    [SerializeField]
    private int _scrollSensitivity = 0;

    private int _columnCount;

    private List<List<GameObject>> _objectPools;

    private bool _isPopulateComplete = false;

    private float _lastYPosition;

    private int _level = 1;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        _columnCount = _itemList.Count;
        InitializeObjectPools(_columnCount);
        Populate();
    }

    private void Update()
    {
        var contentYPosition = _scrollRect.content.anchoredPosition.y;

        if(_isPopulateComplete)
        {
            // Scroll Up
            if (contentYPosition > _lastYPosition && contentYPosition >= _level * (_cellSize + _spacing))
            {
                _lastYPosition = contentYPosition;
                _level++;

                for (int i = 0; i < _columnCount; i++)
                {
                    AdjustItem(_objectPools[i][0], _objectPools[i][_objectPools[i].Count - 1].GetComponent<RectTransform>().anchoredPosition.x - _cellSize, -_objectPools[i][_objectPools[i].Count - 1].GetComponent<RectTransform>().anchoredPosition.y + _spacing);
                    // to keep order of the items, add first element to end of the pool and remove it from first index
                    _objectPools[i].Add(_objectPools[i][0]);
                    _objectPools[i].RemoveAt(0);
                }
            }

            // Scroll Down
            if (contentYPosition < _lastYPosition &&  contentYPosition <= _level * (_cellSize + _spacing))
            {
                _lastYPosition = contentYPosition;
                _level--;

                for (int i = 0; i < _columnCount; i++)
                {
                    AdjustItem(_objectPools[i][_objectPools[i].Count - 1], _objectPools[i][0].GetComponent<RectTransform>().anchoredPosition.x - _cellSize, - _objectPools[i][0].GetComponent<RectTransform>().anchoredPosition.y - 2 * _cellSize - _spacing);
                    // to keep order of the items, add first element to start of the pool and remove it from last index
                    _objectPools[i].Insert(0, _objectPools[i][_objectPools[i].Count - 1]);
                    _objectPools[i].RemoveAt(_objectPools[i].Count - 1);
                }
            }
        }
    }

    /// <summary>
    /// Populates the scroll view according to itemList, cellSize and spacing between items
    /// </summary>
    private void Populate()
    {
        if (_itemList.Count <=0 || _numberOfItems <= 0) return;

        GameObject content = _scrollRect.content.gameObject;
        for (var i = 0; i < _numberOfItems; i++)
        {
            var y = i * (_cellSize + _spacing);
            for (int j = 0; j < _columnCount; j++)
            {
                var x = j * (_cellSize + _spacing);
                var obj = Instantiate(_itemList[j], content.transform);
                AdjustItem(obj, x, y);
                _objectPools[j].Add(obj);
            }
        }
        _isPopulateComplete = true;
    }

    /// <summary>
    /// Sets size and position of the item
    /// </summary>
    /// <param name="item">the item which will be placed</param>
    /// <param name="x">x offset</param>
    /// <param name="y">y offset</param>
    private void AdjustItem(GameObject item, float x, float y)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(_cellSize, _cellSize);
        rectTransform.anchoredPosition = new Vector2(_cellSize + x, -_cellSize - y);
    }

    /// <summary>
    /// Initialize lists of game object pools
    /// </summary>
    /// <param name="objectCount">the size of the itemList</param>
    private void InitializeObjectPools(int objectCount)
    {
        _objectPools = new List<List<GameObject>>();
        for (int i = 0; i < objectCount; i++)
        {
            List<GameObject> objPool = new List<GameObject>();
            _objectPools.Add(objPool);
        }
    }


}
