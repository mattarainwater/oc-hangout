using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SetBackgroundAction : DialogueAction
{
    public string Id { get; set; }
    public string Texture { get; set; }
    public int ZIndex { get; set; } = 0;
}
