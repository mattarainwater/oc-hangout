using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MoveActorAction : DialogueAction
{
    public string Id { get; set; }
    public string AnimationName { get; set; } = "walk";
    public Vector2 To { get; set; }
    public bool Flip { get; set; }
    public float TimeToMove { get; set; }
    public bool Halting { get; set; }
    public string EndAnimation { get; set; }
}
