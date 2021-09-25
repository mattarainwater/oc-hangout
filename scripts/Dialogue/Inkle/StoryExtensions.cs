using Ink.Runtime;
using System.Collections.Generic;

public static class StoryExtensions
{
	public static List<Token> GetTokens(this Story story)
	{
		var tokenizer = new Tokenizer();
		return tokenizer.Tokenize(story.currentTags, ':');
	}
}
