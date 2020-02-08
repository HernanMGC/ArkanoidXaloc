using UnityEngine;
using System;

[Serializable]
public class CapsulePrize
{
    public Capsule capsule;
    [Min(1)]
    public int capsuleWeight;
}
