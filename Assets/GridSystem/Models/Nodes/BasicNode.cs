using UnityEngine;

public class BasicNode : BaseNode
{
    [SerializeField] GameObject whitePlane;
    [SerializeField] GameObject blackPlane;

    public override string Id => "1";

    public override string PoolInstanceId
    {
        get => "BasicCell";
        protected internal set => throw new System.NotImplementedException();
    }

    public void SetPlane(bool isWhite)
    {
        whitePlane.SetActive(isWhite);
        blackPlane.SetActive(!isWhite);
    }


}