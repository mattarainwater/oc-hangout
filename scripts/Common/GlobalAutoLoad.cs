using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class GlobalAutoLoad : Node
{
	private static Theme _defaultTheme;
	private static Theme _odTheme;

	public const string START_STORY_PATH = "story.json";
	public static MainGameState LoadedState { get; set; }
	public static readonly Vector2 Resolution = new Vector2(640, 360);
	public static int ResolutionScale { get; set; }

	public override void _Ready()
	{
		_defaultTheme = ResourceLoader.Load<Theme>("res://assets/themes/default.tres");
		_odTheme = ResourceLoader.Load<Theme>("res://assets/themes/od.tres");
		ResolutionScale = (int)(GetTree().Root.Size.x / Resolution.x);
	}

	public override void _Input(InputEvent @event)
	{
	}

	private List<Node> GetAllNodes()
	{
		return GetAllNodes(GetTree().Root);
	}

	private List<Node> GetAllNodes(Node node)
	{
		List<Node> nodes = new List<Node>();
		foreach(Node n in node.GetChildren())
		{
			nodes.Add(n);
			if (n.GetChildCount() > 0)
			{
				nodes.AddRange(GetAllNodes(n));
			}
		}
		return nodes;
	}
}
