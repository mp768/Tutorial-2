using Godot;

/**
 * A smaller wrapper class to simply pool multiple column objects
 */
public partial class ColumnPool : Node
{
	private Column[] columns;

	private int columnCount = 0;

	private Vector2 startPos;
	private Vector2 endPos;

	private Timer timer;

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

		columns = new Column[Constants.MAX_COLUMNS];

		for (int i = 0; i < Constants.MAX_COLUMNS; i++)
		{
			columns[i] = columnInstance.Instantiate<Column>();
			AddChild(columns[i]);
		}

		timer = new Timer();
		timer.Timeout += SpawnColumn;
		timer.WaitTime = 3.2f;
		timer.OneShot = false;
		timer.Autostart = true;

		AddChild(timer);

		Reset();

		GameSignals.Instance.ResetGame += Reset;
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

		column.Enable();
		column.Visible = true;

		column.Position = new Vector2(startPos.X, GetRandomHeight());
	}

	private void Reset()
	{
		for (int i = 0; i < Constants.MAX_COLUMNS; i++)
		{
			var column = columns[i];

			column.Visible = false;
			column.Position = startPos;
			column.Disable();
		}

		columnCount = 0;

		// Reset the timer.
		timer.Start();
	}

	private static float GetRandomHeight()
	{
		// NOTE: These numbers were just pulled from moving the column in the editor up-and-down.
		return GD.RandRange(140, 721);
	}
}
