using UnityEngine;

public class ViewFactory
{
 
    public static GameObject Create<TM, TVm>(TM model, GameObject prefab, Transform parentTransform)
        where TVm : SpatialViewModel<TM> where TM : ISpatial
    {
        GameObject gameObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        gameObject.transform.parent = parentTransform;
        gameObject.GetComponent<TVm>().Initialize(model);
        return gameObject;
    }
}
