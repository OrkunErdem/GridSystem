using System.Collections.Generic;

public interface IGridProvider
{
    IGridProvider CreateSelf();
    void Initialize(System.Action onReady);
    void InitializeMap(int level = -1);
    void ClearMap();
    Map GetMapData(int level = -1);

    List<BaseNode> GetBaseNodes();
}