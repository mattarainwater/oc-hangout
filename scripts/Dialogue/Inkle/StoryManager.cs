using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using Godot;
using System;

public class StoryManager
{
	private Story _story;
	private readonly Dictionary<string, Type> _instructionsMap = new Dictionary<string, Type> 
	{
		{ "", typeof(ShowDialogueAction) },
		{ "SPAWN_ACTOR", typeof(SpawnActorAction) },
		{ "SET_BACKGROUND", typeof(SetBackgroundAction) },
		{ "MOVE_ACTOR", typeof(MoveActorAction) },
		{ "PLAY_ANIMATION", typeof(PlayAnimationAction) },
		{ "DESPAWN_ACTOR", typeof(DespawnActorAction) },
		{ "HIDE_DIALOGUE", typeof(HideDialogueAction) },
		{ "REMOVE_BACKGROUND", typeof(RemoveBackgroundAction) },
		{ "CLEAR", typeof(ClearAction) },
		{ "LOAD_STORY", typeof(LoadStoryAction) },
		{ "LOAD_BATTLE", typeof(LoadBattleAction) },
		{ "LOAD_MAP", typeof(LoadMapAction) },
		{ "FADE_IN", typeof(FadeInAction) },
		{ "FADE_OUT", typeof(FadeOutAction) },
		{ "PAUSE", typeof(PauseAction) },
	};

	public StoryManager(string fileName)
	{
		var file = new File();
		file.Open(fileName, File.ModeFlags.Read);
		_story = new Story(file.GetAsText());
	}

	public void Unload()
	{
		_story = null;
	}

	public DialogueAction Continue()
	{
		if(_story != null)
		{
			if (_story.canContinue)
			{
				_story.Continue();
				return GetCurrentInstruction();
			}
			else if (HasChoices())
			{
				var choices = GetChoices();
				return new ShowChoiceAction()
				{
					Choices = choices
				};
			}
		}
		return null;
	}

	public List<string> ContinueUntilBreak()
	{
		var texts = new List<string>();

		if(_story.canContinue)
		{
			ShowDialogueAction action;
			do
			{
				_story.Continue();
				action = GetCurrentInstruction() as ShowDialogueAction;
				if(action != null)
				{
					if(action.Text.StripWhitespace() == "BREAK")
					{
						break;
					}
					else
					{
						texts.Add("\t\r\n" + action.Text);
					}
				}

			}
			while (_story.canContinue);
		}

		return texts;
	}

	public void SetChoice(string choiceText)
	{
		var choice = _story.currentChoices.FirstOrDefault(x => x.text == choiceText);
		if(choice != null)
		{
			var choiceIndex = _story.currentChoices.IndexOf(choice);
			_story.ChooseChoiceIndex(choiceIndex);
		}
	}

	public bool HasChoices()
	{
		return _story.currentChoices.Any();
	}

	public List<string> GetChoices()
	{
		return _story.currentChoices.Select(x => x.text).ToList();
	}

	public void LoadVariables(Dictionary<string, object> data)
	{
		foreach(var key in data.Keys)
		{
			if(_story.variablesState.Contains(key))
			{
				_story.variablesState[key] = data[key];
			}
		}
	}

	public Dictionary<string, object> GetVariables()
	{
		var dict = new Dictionary<string, object>();
		foreach(var variable in _story.variablesState)
		{
			dict[variable] = _story.variablesState[variable];
		}
		return dict;
	}

	private DialogueAction GetCurrentInstruction()
	{
		var currentText = _story.currentText.StripWhitespace();
		var currentTags = _story.GetTokens();


		var typeToInstance = typeof(ShowDialogueAction);

		if (_instructionsMap.ContainsKey(currentText))
		{
			typeToInstance = _instructionsMap[currentText];
		}
		var toReturn = (DialogueAction)Activator.CreateInstance(typeToInstance);
		foreach (var prop in typeToInstance.GetProperties())
		{
			if(prop.Name == "Text")
			{
				prop.SetValue(toReturn, _story.currentText);
				continue;
			}
			var propInTags = currentTags.FirstOrDefault(x => x.Key == prop.Name);
			if (propInTags != null)
			{
				object propValue = propInTags.Value;
				if (prop.PropertyType == typeof(int))
				{
					propValue = int.Parse(propInTags.Value);
				}
				if (prop.PropertyType == typeof(float))
				{
					propValue = float.Parse(propInTags.Value);
				}
				if (prop.PropertyType == typeof(bool))
				{
					propValue = bool.Parse(propInTags.Value);
				}
				if (prop.PropertyType == typeof(Vector2))
				{
					var split = (propValue as string).Split(',');
					propValue = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
				}
				prop.SetValue(toReturn, propValue);
			}
		}
		return toReturn;
	}
}
