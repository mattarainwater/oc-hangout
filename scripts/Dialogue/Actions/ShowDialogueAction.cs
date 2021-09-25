using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShowDialogueAction : DialogueAction
{
    public string Name { get; set; }
    public string Text { get; set; }
    public string PortraitSprite { get; set; }
    public string Style { get; set; } = "Default";
    public int Index { get; set; }
}
