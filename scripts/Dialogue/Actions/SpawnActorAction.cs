using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SpawnActorAction : DialogueAction
{
    public string Id { get; set; }
    public int ZIndex { get; set; } = 1;
    public string Sprite { get; set; }
    public Vector2 Position { get; set; }
    public string DefaultAnimationName { get; set; }
    public bool Flip { get; set; }
}
