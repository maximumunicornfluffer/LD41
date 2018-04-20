using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// View d'un state d'actor
/// </summary>
[Serializable]
public class ActorStateViewLite
{
    public string Name;
    public SpriteAnimationView Animation;
    public float AnimationSpeed = 20;

    [HideInInspector]
    public int ID;

}

