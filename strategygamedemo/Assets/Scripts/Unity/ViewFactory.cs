﻿using UnityEngine;

public class ViewFactory
{
 
    public static GameObject Create<TM, TVm>(TM model, GameObject prefab, Transform parentTransform)
        where TVm : SpatialViewModel<TM> where TM : ISpatial
    {
        GameObject gameObject = GameObject.Instantiate(prefab, new Vector3(20f, 20f), Quaternion.identity);
        gameObject.transform.parent = parentTransform;
        gameObject.GetComponent<TVm>().Initialize(model);
        return gameObject;
    }
}
