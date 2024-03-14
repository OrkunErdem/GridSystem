using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject frame;
    public Transform cellParent;


    public void SetSize(int x, int y, float cellSize)
    {
        //  frame.transform.localScale = new Vector3(x * cellSize, 1, y * cellSize);//TODO
    }
}