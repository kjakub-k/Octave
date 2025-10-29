namespace KJakub.Octave.Managers.CommandManager
{
    public interface ICommandManager
    {
        public void Undo();
        public void Redo();
        public void Execute(Command cmd);
    }
}