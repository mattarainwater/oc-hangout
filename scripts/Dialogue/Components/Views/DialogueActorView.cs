using Godot;
using System;

public class DialogueActorView : Node2D
{
	private AnimatedSprite _sprite;
	private Tween _tween;
	public string Id { get; set; }
	private string _defaultAnimation;

	public override void _Ready()
	{
		this.AddObserver(OnMove, Global.PerformNotification<MoveActorAction>());
		this.AddObserver(OnAnimation, Global.PerformNotification<PlayAnimationAction>());
		_sprite = GetNode<AnimatedSprite>("Sprite");
		_tween = new Tween();
		AddChild(_tween);
	}

	public override void _ExitTree()
	{
		this.RemoveObserver(OnMove, Global.PerformNotification<MoveActorAction>());
		this.RemoveObserver(OnAnimation, Global.PerformNotification<PlayAnimationAction>());
	}

	public void Setup(SpawnActorAction action)
	{
		_sprite.Frames = ResourceLoader.Load<SpriteFrames>("res://assets/sprites/" + action.Sprite);
		_sprite.Centered = false;
		_defaultAnimation = action.DefaultAnimationName;
		_sprite.Animation = _defaultAnimation;
		_sprite.FlipH = action.Flip;
		_sprite.Play();
		Position = action.Position;
		Id = action.Id;
	}

	private void OnMove(object sender, object args)
	{
		var action = args as MoveActorAction;
		if(action.Id == Id)
		{
			if(!string.IsNullOrEmpty(action.EndAnimation))
			{
				_defaultAnimation = action.EndAnimation;
			}
			_sprite.Animation = action.AnimationName;
			_sprite.FlipH = action.Flip;
			_sprite.Play();
			_tween.InterpolateProperty(this, "position", Position, action.To, action.TimeToMove);
			_tween.Start();
			_tween.InterpolateCallback(this, action.TimeToMove, "NextMove");
			if (action.Halting)
			{
				_tween.InterpolateCallback(this, action.TimeToMove, "Next");
			}
			else
			{
				Next();
			}
		}
	}

	private void OnAnimation(object sender, object args)
	{
		var action = args as PlayAnimationAction;
		if(action.Id == Id)
		{
			_sprite.Animation = action.AnimationName;
			_sprite.FlipH = action.Flip;
			_sprite.Play();
			if (action.Halting)
			{
				_sprite.Connect("animation_finished", this, "Next");
			}
			else
			{
				Next();
			}
		}
	}

	private void NextMove()
	{
		_sprite.Animation = _defaultAnimation;
	}

	private void Next()
	{
		if (_sprite.IsConnected("animation_finished", this, "Next"))
		{
			_sprite.Disconnect("animation_finished", this, "Next");
		}
		this.PostNotification(DialogueManager.DIALOGUE_NEXT, null);
	}
}
