using UnityEngine;

public class Cell : MonoBehaviour
{
    private string _prefabId;
    private BaseNode _node;

    public void SetNode(BaseNode node, string prefabId)
    {
        _node = node;
        _prefabId = prefabId;
    }

    public void RemoveCell()
    {
           Destroy(gameObject);// you can use pooling system here
    }
}