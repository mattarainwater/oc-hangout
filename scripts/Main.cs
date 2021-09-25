using Godot;
using System;

public class Main : Node
{
	private DialogueManager _dialogueManager;

	public override void _Ready()
	{
		this.AddObserver(OnLoadStory, Global.PerformNotification<LoadStoryAction>());


		_dialogueManager = GetNode<DialogueManager>("DialogManager");

		_dialogueManager.Deactivate();

		if(GlobalAutoLoad.LoadedState == null)
		{
			OnLoadStory(this, new LoadStoryAction { 
				Path = GlobalAutoLoad.START_STORY_PATH
			});
		}
	}

	public override void _ExitTree()
	{
		this.RemoveObserver(OnLoadStory, Global.PerformNotification<LoadStoryAction>());
	}

	private void OnLoadStory(object sender, object args)
	{
		var action = args as LoadStoryAction;
		_dialogueManager.Load("res://assets/" + action.Path);
	}

	private void Next()
	{
		this.PostNotification(DialogueManager.DIALOGUE_NEXT, null);
	}
}
