using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    static Cell[,] cells;
    private static int rows;
    private static int cols;
    private static float cellSize;
    private static float offSet;

    private bool _isMapInitialized;

    private GameObject Map;

    private readonly Dictionary<string, string> _prefabDictionary = new Dictionary<string, string>();

    private Dictionary<node, BaseNode> _nodeDictionary =
        new Dictionary<node, BaseNode>();

    private List<Cell> _cellList = new List<Cell>();

    #region prefabs

    [SerializeField] private GameObject _mapPrefab;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _baseNodePrefab;

    #endregion

    public class node
    {
        public int X;
        public int Y;

        public node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public void Initialize(Action onReady)
    {
        var a = Resources.Load<GridSystemConfig>("GridSystemConfig");
        cellSize = a.cellSize;
        offSet = a.offset;
        InitializeDictionary(a.nodeItemDataList);
        onReady?.Invoke();
    }

    private void InitializeDictionary(List<NodeItemData> aNodeItemDataList)
    {
        foreach (var data in aNodeItemDataList)
        {
            _prefabDictionary.Add(data.id, data.poolId);
        }
    }

    public void InitializeMap(int level = -1)
    {
        if (_isMapInitialized) ClearMap();
        Map = Instantiate(_mapPrefab, Vector3.zero, Quaternion.identity);
        var mapData = GetMapDataAndSet(level);
        var originalMap = GetMap(level);
        cells = new Cell[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float x = col * cellSize;
                float z = row * cellSize;
                Vector3 position = new Vector3(x, 0, z);

                var cell = Instantiate(_cellPrefab, position, Quaternion.identity);
                _cellList.Add(cell.GetComponent<Cell>());
                cell.transform.localScale = new Vector3(cellSize * 1, 1, cellSize * 1);
                cell.transform.parent = Map.GetComponent<MapController>().cellParent;
                cells[row, col] = cell.GetComponent<Cell>();
            }
        }

        FillCell(originalMap, mapData);
    }


    private void FillCell(int[,] map, Map mapData)
    {
        SetDictionaries(map, mapData);
        var counter = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var id = map[row, col];
                if (id == 0) continue;
                _prefabDictionary.TryGetValue(id.ToString(), out var prefabId);
                if (prefabId == "") continue;

                var node = Instantiate(_baseNodePrefab, cells[row, col].transform.position, Quaternion.identity);
                node.transform.parent = cells[row, col].transform;
                var nodeComponent = node.GetComponent<BaseNode>();
                cells[row, col].SetNode(nodeComponent, prefabId);
                if (GetForcedNodeData(row, col) != null)
                {
                    nodeComponent.IsObstacle = GetForcedNodeData(row, col).isObstacle;
                    nodeComponent.Health = GetForcedNodeData(row, col).health;
                }
                else
                {
                    nodeComponent.IsObstacle = GetNodeData(id.ToString()).isObstacle;
                    nodeComponent.Health = GetNodeData(id.ToString()).health;
                }

                if (id == 1)
                {
                    node.GetComponent<BasicNode>().SetPlane(counter % 2 == 0);
                    counter++;
                }

                _nodeDictionary.Add(new node(
                    x: row,
                    y: col), nodeComponent);
            }

            counter++;
        }

        _isMapInitialized = true;
    }


    #region MapDictionaries

    private Dictionary<Vector2, ForcedNodeData> forcedNodeDataDictionary = new Dictionary<Vector2, ForcedNodeData>();

    private Dictionary<string, NodeData> nodeDataDictionary = new Dictionary<string, NodeData>();

    private ForcedNodeData GetForcedNodeData(int row, int col)
    {
        forcedNodeDataDictionary.TryGetValue(new Vector2(row, col), out var forcedNodeData);
        return forcedNodeData;
    }

    private NodeData GetNodeData(string id)
    {
        nodeDataDictionary.TryGetValue(id, out var nodeData);
        return nodeData;
    }

    private void SetDictionaries(int[,] map, Map mapData)
    {
        foreach (var forcedNodeData in mapData.forcedNodeDataList)
        {
            forcedNodeDataDictionary.Add(new Vector2(forcedNodeData.x, forcedNodeData.y), forcedNodeData);
        }

        foreach (var nodeData in mapData.nodeDataList)
        {
            nodeDataDictionary.Add(nodeData.id.ToString(), nodeData);
        }
    }

    private void ClearDictionaries()
    {
        forcedNodeDataDictionary.Clear();
        nodeDataDictionary.Clear();
    }

    #endregion


    public void ClearMap()
    {
        foreach (var cell in _cellList)
        {
            cell.RemoveCell();
        }

        Destroy(Map);
        ClearDictionaries();
        Map = null;
        _nodeDictionary.Clear();
        _cellList.Clear();
        _isMapInitialized = false;
    }

    public List<BaseNode> GetBaseNodes()
    {
        var baseNodes = new List<BaseNode>();
        foreach (var node in _nodeDictionary.Values)
        {
            baseNodes.Add(node);
        }

        return baseNodes;
    }

    public BaseNode GetNode(Vector3 position)
    {
        var x = (int)(position.x + (cols * cellSize) / 2) / (int)cellSize;
        var y = (int)(position.z + (rows * cellSize) / 2) / (int)cellSize;
        if (x < 0 || x >= rows || y < 0 || y >= cols) return null;
        _nodeDictionary.TryGetValue(new node(x, y), out var node);
        return node;
    }

    public Vector3 GetPosition(int x, int y)
    {
        float xPosition = x * cellSize - (cols * cellSize) / 2 + cellSize / 2;
        float zPosition = y * cellSize - (rows * cellSize) / 2 + cellSize / 2;
        return new Vector3(xPosition, 0, zPosition);
    }

    private int[,] GetMap(int level = -1)
    {
        if (level == -1)
        {
            level = 0;
        }

        var gripMapContainer = Resources.Load<GripMapContainer>("GripMapContainer");
        var map = gripMapContainer.GetMap(level);
        var matrix = map.GetMatrix();
        return matrix;
    }

    private Map GetMapDataAndSet(int level = -1)
    {
        if (level == -1)
        {
            level = 0;
        }

        var gripMapContainer = Resources.Load<GripMapContainer>("GripMapContainer");
        var map = gripMapContainer.GetMap(level);
        var originalMap = map.GetMatrix();
        cols = originalMap.GetLength(1);
        rows = originalMap.GetLength(0);
        return map;
    }

    public Map GetMapData(int level)
    {
        if (level == -1)
        {
            level = 0;
        }

        var gripMapContainer = Resources.Load<GripMapContainer>("GripMapContainer");
        var map = gripMapContainer.GetMap(level);
        return map;
    }
}