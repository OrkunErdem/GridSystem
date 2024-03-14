using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GridSystemConfig", menuName = "ScriptableObjects/GridSystemConfig", order = 1)]
public class GridSystemConfig : ScriptableObject
{
    public float cellSize;
    public float offset;

    public List<NodeItemData> nodeItemDataList;
}

[Serializable]
public class NodeItemData
{
    public string id;
    public string poolId;
}