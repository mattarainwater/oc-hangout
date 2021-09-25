using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayAnimationAction : DialogueAction
{
    public string Id { get; set; }
    public string AnimationName { get; set; }
    public bool Flip { get; set; }
    public bool Halting { get; set; }
}
