using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// View d'une actor action
/// </summary>
[Serializable]
public class ActorActionViewLite
{
    public string Name;
    public SpriteAnimationView Animation;
    public string Sound;
    public float AnimationSpeed;
    public string NextAction;


    [HideInInspector]
    public float CurrentTimer;
}

[Serializable]
public class SpriteAnimationView
{
    public List<Sprite> Sprites;
}

