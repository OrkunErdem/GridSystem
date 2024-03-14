using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class Map : ScriptableObject
{
    public bool isActive;
    public int level;
    private int[,] _mapMatrix;

    [SerializeField] public List<NodeData> nodeDataList;
    [SerializeField] public List<ForcedNodeData> forcedNodeDataList;
    [SerializeField] public int maxMoves;


    public string map;

    private int[,] GenerateRandomMap()
    {
        var numRows = 6;
        var numCols = 7;
        int[,] mapData = new int[numCols, numRows];

        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                mapData[i, j] = 1;
            }
        }

        return mapData;
    }

    public int[,] GetMatrix()
    {
        return _mapMatrix ??= GenerateRandomMap();
    }
}

[Serializable]
public class NodeData
{
    public int id;
    public bool isObstacle;
    public int health;
}

[Serializable]
public class ForcedNodeData
{
    public int x;
    public int y;
    public bool isObstacle;
    public int health;
}

