using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : MonoBehaviour
{
    public int count;
    public ParticleSystem effect;
    public abstract void Use();
}
