using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute();
    void Undo();
}

public class StrokeCommand : ICommand
{
    private PaintGrid paintGrid;

    // Backup-Daten
    private Paint[] backupContent;
    private ColumnInfo[] backupInfo;
    private Vector3Int backupSize;

    public StrokeCommand(PaintGrid grid)
    {
        this.paintGrid = grid;
    }

    //Backing of the current state of the canvas
    public void Execute()
    {
        paintGrid.ReadbackContent();
        paintGrid.ReadbackInfo();

        backupSize = paintGrid.Size;
        backupContent = (Paint[])paintGrid.ContentData.Clone();
        backupInfo = (ColumnInfo[])paintGrid.InfoData.Clone();
    }
    
    //reverting to the saved state
    public void Undo()
    {
        paintGrid.Size = backupSize;
        paintGrid.Content.SetData(backupContent);
        paintGrid.Info.SetData(backupInfo);
    }
}


public class CommandManager
{
    private Stack<ICommand> commandStack = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandStack.Push(command);
    }

    public void Undo()
    {
        if (commandStack.Count > 0)
        {
            ICommand lastCommand = commandStack.Pop();
            lastCommand.Undo();
        }
        else
        {
            Debug.Log("Nothing to Undo.");
        }
    }
}