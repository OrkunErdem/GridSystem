using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GripMapContainer", menuName = "ScriptableObjects/GripMapContainer", order = 1)]
public class GripMapContainer : ScriptableObject
{
    public List<Map> levelList;

    public Map GetMap(int level)
    {
        var map = levelList.Find(x => x.level == level);
        if (map != null && map.isActive)
        {
            return map;
        }

        level++;
        var mapNext = levelList.Find(x => x.level == level);
        if (mapNext != null && mapNext.isActive)
        {
            return mapNext;
        }

        level -= 2;
        var mapPrev = levelList.Find(x => x.level == level);
        if (mapPrev != null && mapPrev.isActive)
        {
            return mapPrev;
        }

        return levelList[0];
    }
}