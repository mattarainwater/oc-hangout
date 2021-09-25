using Godot;
using System;
using System.Linq;

public class DialogueBoxView : Node2D
{
	private Sprite _portrait;
	private Label _namePlate;
	private Label _text;

	private string _fullText = "";
	private string _displayText = "";
	private string _textRemaining = "";
	private bool _isFinished;
	private bool _inProgess;

	private float _dt;
	private float _letterTimeStep = .05f;

	private const int _charPerLine = 43;
	private const int _maxLines = 2;

	private ShowDialogueAction _overflowAction;

	public override void _Ready()
	{
		this.AddObserver(OnDialogue, Global.PerformNotification<ShowDialogueAction>());
		this.AddObserver(OnHide, Global.PerformNotification<HideDialogueAction>());
		this.AddObserver(OnChoice, Global.PerformNotification<ShowChoiceAction>());
		this.AddObserver(OnClear, Global.PerformNotification<ClearAction>());
		Visible = false;

		_portrait = GetNode<Sprite>("PortraitBox/Portrait");
		_namePlate = GetNode<Label>("NameText");
		_text = GetNode<Label>("Text");
		ZIndex = 999;
	}

	public override void _ExitTree()
	{
		this.RemoveObserver(OnDialogue, Global.PerformNotification<ShowDialogueAction>());
		this.RemoveObserver(OnHide, Global.PerformNotification<HideDialogueAction>());
		this.RemoveObserver(OnChoice, Global.PerformNotification<ShowChoiceAction>());
		this.RemoveObserver(OnClear, Global.PerformNotification<ClearAction>());
	}

	public override void _UnhandledKeyInput(InputEventKey @event)
	{
		if (@event.Pressed && @event.Scancode == (int)KeyList.Space)
		{
			if (!_isFinished)
			{
				_displayText = _fullText;
				_text.Text = _fullText;
				_textRemaining = "";
				_isFinished = true;
			}
			else if(_inProgess)
			{
				Next();
			}
		}
	}

	public override void _Process(float delta)
	{
		if (!_textRemaining.Any())
		{
			_isFinished = true;
		}
		else
		{
			_dt += delta;
			if (_dt >= _letterTimeStep)
			{
				Visible = true;
				_dt = 0f;
				_displayText += _textRemaining.First();
				_textRemaining = _textRemaining.Remove(0, 1);
				_text.Text = _displayText;
			}
		}
	}

	private void OnDialogue(object sender, object args)
	{
		var action = args as ShowDialogueAction;
		if(action.Style == "Default")
		{
			_displayText = "";
			_inProgess = true;
			_isFinished = false;
			_overflowAction = null;

			var fullText = action.Text.WrapText(_charPerLine).Split("\r\n");
			_fullText = string.Join("\r\n", fullText.Take(_maxLines));

			var nextLines = fullText.Skip(_maxLines);
			if(nextLines.Any())
			{
				_overflowAction = new ShowDialogueAction 
				{
					PortraitSprite = action.PortraitSprite,
					Style = action.Style,
					Name = action.Name,
					Text = string.Join("", nextLines),
					Index = action.Index
				};
			}

			_textRemaining = _fullText;
			_namePlate.Text = action.Name;
			_portrait.Texture = ResourceLoader.Load<Texture>("res://assets/" + action.PortraitSprite);
			_portrait.Vframes = _portrait.Texture.GetHeight() / 63;
			_portrait.Hframes = _portrait.Texture.GetWidth() / 69;
			_portrait.Frame = action.Index;
		}
		else
		{
			Visible = false;
		}
	}

	private void Next()
	{
		_inProgess = false;
		this.PostNotification(DialogueManager.DIALOGUE_NEXT, _overflowAction);
	}

	private void OnHide(object arg1, object arg2)
	{
		Visible = false;
	}

	private void OnChoice(object sender, object args)
	{
		Visible = false;
	}

	private void OnClear(object sender, object args)
	{
		var action = args as ClearAction;
		Visible = false;
		_fullText = "";
		_displayText = "";
		_textRemaining = "";
		_isFinished = false;
		_inProgess = false;
		_dt = 0f;
		this.PostNotification(DialogueManager.DIALOGUE_NEXT, null);
	}
}
