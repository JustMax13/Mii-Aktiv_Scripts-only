namespace General
{
    public interface IStarter
    {
        public bool IsStart { get; }
        public void Launch();
    }
}