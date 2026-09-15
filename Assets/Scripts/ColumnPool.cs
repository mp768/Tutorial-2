using Godot;

/**
 * A smaller wrapper class to simply pool multiple column objects
 */
public partial class ColumnPool : Node
{
	private Node2D[] columns;

	private int columnCount = 0;

	private Vector2 startPos;
	private Vector2 endPos;

	public override void _Ready()
	{
		var startPosNode = GetNode<Node2D>("StartPositionNode");
		startPos = startPosNode.Position;

		var endPosNode = GetNode<Node2D>("EndPositionNode");
		endPos = endPosNode.Position;

		// Once we retrieve our designated values from these helper nodes, we just get rid of them from the active scene.
		startPosNode.QueueFree();
		endPosNode.QueueFree();

		var columnInstance = ResourceLoader.Load<PackedScene>("uid://cidgnqavq2qdu");

		columns = new Node2D[Constants.MAX_COLUMNS];
		for (int i = 0; i < Constants.MAX_COLUMNS; i++)
		{
			var column = columnInstance.Instantiate<Node2D>();

			column.ProcessMode = ProcessModeEnum.Disabled;
			column.Visible = false;
			column.Position = startPos;

			columns[i] = column;
			AddChild(column);
		}

		var timer = new Timer();
		timer.Timeout += SpawnColumn;
		timer.WaitTime = 3.2f;
		timer.OneShot = false;
		timer.Autostart = true;

		AddChild(timer);
	}

	public override void _Process(double delta)
	{
		for (int i = 0; i < columnCount; i++)
		{
			var column = columns[i];

			if (column.Position.X <= endPos.X)
			{
				column.Position += new Vector2(startPos.X, 0);
				column.Position = new Vector2(column.Position.X, GetRandomHeight());
			}
		}
	}

	private void SpawnColumn()
	{
		if (columnCount >= Constants.MAX_COLUMNS) return;

		var column = columns[columnCount];
		columnCount++;

		column.ProcessMode = ProcessModeEnum.Pausable;
		column.Visible = true;

		column.Position = new Vector2(startPos.X, GetRandomHeight());
	}

	private static float GetRandomHeight()
	{
		// NOTE: These numbers were just pulled from moving the column in the editor up-and-down.
		return GD.RandRange(140, 721);
	}
}
