using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using Godot;
using System;
using Newtonsoft.Json;

public class DialogueManager : ViewportContainer
{
	public const string DIALOGUE_NEXT = "DIALOGUE_NEXT";

	[Export(PropertyHint.File)]
	public string _filePath;

	private StoryManager _story;
	private DialogueAction _currentAction;
	private DialogueAction _queuedAction;
	private Timer _t;
	private Tween _tween;
	private Polygon2D _fade;
	private Node2D _manager;

	public override void _Ready()
	{
		this.AddObserver(OnSpawn, Global.PerformNotification<SpawnActorAction>());
		this.AddObserver(OnPause, Global.PerformNotification<PauseAction>());
		this.AddObserver(OnBackground, Global.PerformNotification<SetBackgroundAction>());
		this.AddObserver(OnRemoveBackground, Global.PerformNotification<RemoveBackgroundAction>());
		this.AddObserver(OnDespawn, Global.PerformNotification<DespawnActorAction>());
		this.AddObserver(OnMakeChoice, Global.PerformNotification<MakeChoiceAction>());
		this.AddObserver(OnFadeIn, Global.PerformNotification<FadeInAction>());
		this.AddObserver(OnFadeOut, Global.PerformNotification<FadeOutAction>());
		this.AddObserver(OnNext, DIALOGUE_NEXT);

		_manager = GetNode<Node2D>("Viewport/DialogManager");

		_tween = new Tween();
		_manager.AddChild(_tween);
		_fade = GetNode<Polygon2D>("Viewport/DialogManager/Fade");
		_fade.Color = new Color(0, 0, 0, 1);

		_t = new Timer();
		_t.WaitTime = 1f;
		_t.OneShot = true;
		if (!_t.IsConnected("timeout", this, "Next"))
		{
			_t.Connect("timeout", this, "Next");
		}
		_manager.AddChild(_t);
		if (!string.IsNullOrEmpty(_filePath))
		{
			Load(_filePath);
		}
	}

	public override void _ExitTree()
	{
		this.RemoveObserver(OnSpawn, Global.PerformNotification<SpawnActorAction>());
		this.RemoveObserver(OnPause, Global.PerformNotification<PauseAction>());
		this.RemoveObserver(OnBackground, Global.PerformNotification<SetBackgroundAction>());
		this.RemoveObserver(OnRemoveBackground, Global.PerformNotification<RemoveBackgroundAction>());
		this.RemoveObserver(OnDespawn, Global.PerformNotification<DespawnActorAction>());
		this.RemoveObserver(OnMakeChoice, Global.PerformNotification<MakeChoiceAction>());
	}

	public override void _Process(float delta)
	{
		if(_currentAction == null)
		{
			if (_queuedAction != null)
			{
				_currentAction = _queuedAction;
				_queuedAction = null;
				this.PostNotification(Global.PerformNotification(_currentAction.GetType()), _currentAction);
			}
			else if(_story != null)
			{
				_currentAction = _story.Continue();
				GD.Print(_currentAction.GetType().ToString());
				if (_currentAction != null)
				{
					var currActionAsDialog = _currentAction as ShowDialogueAction;
					if (currActionAsDialog != null && currActionAsDialog.Style == "Crawl")
					{
						var linesUntilBreak = _story.ContinueUntilBreak();
						foreach (var line in linesUntilBreak)
						{
							currActionAsDialog.Text += line;
						}
					}
					this.PostNotification(Global.PerformNotification(_currentAction.GetType()), _currentAction);
				}
			}
		}
	}

	public void Load(string path)
	{
		_fade.Color = new Color(0, 0, 0, 1);
		_story = new StoryManager(path);
		_story.LoadVariables(MainGameState.Instance.AllData);
		_currentAction = null;
		_queuedAction = null;
		Visible = true;
	}

	public void Deactivate()
	{
		Visible = false;
		if(_story != null)
		{
			var variables = _story.GetVariables();
			MainGameState.Instance.AddData(variables);
			_story.Unload();
		}
	}

	private void OnSpawn(object sender, object args)
	{
		var action = args as SpawnActorAction;
		var scene = GD.Load<PackedScene>("res://components/Dialogue/DialogueActor.tscn");
		var node = scene.Instance() as DialogueActorView;
		_manager.AddChild(node);
		node.ZIndex = action.ZIndex;
		node.Setup(action);
		this.PostNotification(DIALOGUE_NEXT, null);
	}

	private void OnBackground(object sender, object args)
	{
		var action = args as SetBackgroundAction;
		var sprite = new Sprite();
		sprite.Texture = ResourceLoader.Load<Texture>("res://assets/" + action.Texture);
		sprite.Centered = false;
		sprite.ZIndex = action.ZIndex;
		sprite.Position = Vector2.Zero;
		sprite.Name = "BG" + action.Id;
		_manager.AddChild(sprite);
		this.PostNotification(DIALOGUE_NEXT, null);
	}

	private void OnRemoveBackground(object sender, object args)
	{
		var action = args as RemoveBackgroundAction;
		var children = _manager.GetChildren().OfType<Sprite>();
		var child = children.FirstOrDefault(x => x.Name == "BG" + action.Id);
		if(child != null)
		{
			child.QueueFree();
		}
		this.PostNotification(DIALOGUE_NEXT, null);
	}

	private void OnDespawn(object sender, object args)
	{
		var action = args as DespawnActorAction;
		var children = _manager.GetChildren().OfType<DialogueActorView>();
		var childToDespawn = children.FirstOrDefault(x => x.Id == action.Id);
		if (childToDespawn != null)
		{
			childToDespawn.QueueFree();
		}
		this.PostNotification(DIALOGUE_NEXT, null);
	}

	private void OnMakeChoice(object sender, object args)
	{
		var action = args as MakeChoiceAction;
		_story.SetChoice(action.Text);
		this.PostNotification(DIALOGUE_NEXT, null);
	}

	private void OnPause(object sender, object args)
	{
		var action = args as PauseAction;
		_t.WaitTime = action.Time;
		_t.Start();
	}

	private void Next()
	{
		this.PostNotification(DIALOGUE_NEXT, null);
	}

	private void OnFadeIn(object sender, object args)
	{
		var action = args as FadeInAction;
		_tween.StopAll();
		_tween.InterpolateProperty(_fade, "color", new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), action.Time);
		_tween.Start();
		_t.WaitTime = action.Time;
		_t.Start();
	}

	private void OnFadeOut(object sender, object args)
	{
		var action = args as FadeOutAction;
		_tween.StopAll();
		_tween.InterpolateProperty(_fade, "color", new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), action.Time);
		_tween.Start();
		_t.WaitTime = action.Time;
		_t.Start();
	}

	private void OnNext(object sender, object args)
	{
		var action = args as DialogueAction;
		_queuedAction = action;
		_currentAction = null;
	}
}
