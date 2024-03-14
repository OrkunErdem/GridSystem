public class Box : BaseNode
{
    public override string Id => "2";

    public override string PoolInstanceId
    {
        get => "BoxItem";
        protected internal set => throw new System.NotImplementedException();
    }
}