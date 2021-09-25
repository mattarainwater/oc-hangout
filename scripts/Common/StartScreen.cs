using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StartScreen : Node2D
{
	private TextureProgress _rightSwirl;
	private TextureProgress _leftSwirl;
	private Sprite _title;
	private Sprite _subtitle;
	private Tween _tween;
	private Label _press;
	private bool _ready;

	private bool _tweenStarted = false;

	public override void _Ready()
	{
		_rightSwirl = GetNode<TextureProgress>("RightSwirl");
		_leftSwirl = GetNode<TextureProgress>("LeftSwirl");
		_title = GetNode<Sprite>("Title");
		_subtitle = GetNode<Sprite>("Subtitle");
		_tween = new Tween();
		AddChild(_tween);
		_press = GetNode<Label>("Label");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(_ready)
		{
			GetTree().ChangeScene("res://scenes/Main.tscn");
		}
	}

	public override void _Process(float delta)
	{
		_rightSwirl.Value += 2;
		_leftSwirl.Value += 2;
		if(_rightSwirl.Value == 100 && _leftSwirl.Value == 100 && !_tweenStarted)
		{
			_tweenStarted = true;
			_title.Modulate = new Color(1, 1, 1, 0);
			_title.Visible = true;
			_subtitle.Modulate = new Color(1, 1, 1, 0);
			_subtitle.Visible = true;
			_tween.InterpolateProperty(_title, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1f, delay: .5f);
			_tween.InterpolateProperty(_subtitle, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1f, delay: 1.5f);
			_tween.InterpolateCallback(this, 3f, "Next");
			_tween.Start();
		}
	}

	private void Next()
	{
		var savedGames = GameSaveManagement.GetSaves();
		if(savedGames != null && savedGames.Any())
		{
			// TODO start or continue - pick a save
			_press.Visible = true;
			_ready = true;
		}
		else
		{
			_press.Visible = true;
			_ready = true;
		}
	}
}
