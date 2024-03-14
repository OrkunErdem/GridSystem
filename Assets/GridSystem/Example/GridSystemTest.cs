using System;
using System.Threading.Tasks;
using UnityEngine;

public class GridSystemTest : MonoBehaviour
{
    private void Start()
    {
        InitializeGridManager();
    }

    private static void InitializeGridManager()
    {
        GridManager.Instance.Initialize(TestGridManager );
    }

    private static async void TestGridManager()
    {
        await Task.Delay(100);
        GridManager.Instance.InitializeMap();
    }
}