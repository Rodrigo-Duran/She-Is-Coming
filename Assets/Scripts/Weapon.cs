using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    // Components of the Scriptable Object
    public int damage;
    public Sprite sprite;
    public int shotsCount;
}
