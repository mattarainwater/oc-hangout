using System.Collections.Generic;

public class Tokenizer
{
    public List<Token> Tokenize(List<string> tags, char seperator)
    {
        var tokens = new List<Token>();
        foreach(string tag in tags)
        {
            var split = tag.Split(seperator);
            if(split.Length == 2)
            {
                tokens.Add(new Token{
                    Key = split[0],
                    Value = split[1]
                });
            }
        }
        return tokens;
    }
}
