namespace CardMatching.Core.Interfaces
{
    public interface ICommand
    {
        public void Execute();

        public void Undo();
    }
}