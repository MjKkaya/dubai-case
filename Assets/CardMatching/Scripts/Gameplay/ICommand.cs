namespace CardMatching.Gameplay
{
    public interface ICommand
    {
        public void Execute();

        public void Undo();
    }
}