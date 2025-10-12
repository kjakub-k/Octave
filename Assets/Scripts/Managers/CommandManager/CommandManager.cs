using System.Collections.Generic;
public class CommandManager
{
    private Stack<Command> undoStack = new();
    private Stack<Command> redoStack = new();
    public void Execute(Command command)
    {
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
    }
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            var cmd = undoStack.Pop();
            cmd.Undo();
            redoStack.Push(cmd);
        }
    }
    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            var cmd = redoStack.Pop();
            cmd.Execute();
            undoStack.Push(cmd);
        }
    }
}