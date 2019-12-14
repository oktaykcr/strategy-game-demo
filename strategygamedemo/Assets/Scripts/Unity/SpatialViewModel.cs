using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpatialViewModel<T> : MonoBehaviour where T : ISpatial
{
    protected T DataContext { get; private set; }

    protected SpriteRenderer SpriteRenderer { get; set; }

    private Vector2 _cachedPosition;
    private Color _cachedColor;

    public void Initialize(T dataContext)
    {
        this.DataContext = dataContext;
        SpriteRenderer = this.GetComponent<SpriteRenderer>();

        DataContext.XSize = SpriteRenderer.bounds.size.x;
        DataContext.YSize = SpriteRenderer.bounds.size.y;
    }

    public virtual void Update()
    {
        _cachedPosition = transform.position;
        _cachedColor = SpriteRenderer.color;
        if (!_cachedPosition.x.Equals(DataContext.XPos) || !_cachedPosition.y.Equals(DataContext.YPos))
        {
            transform.localPosition = new Vector2(DataContext.XPos, DataContext.YPos);
        }

        if (!_cachedColor.a.Equals(DataContext.Opacity))
        {
            SpriteRenderer.color = new Color(1f, 1f, 1f, DataContext.Opacity);
        }
        this.OnUpdate();
    }

    protected abstract void OnUpdate();
}
