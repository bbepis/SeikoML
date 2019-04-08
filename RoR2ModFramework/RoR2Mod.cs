using System;
using BepInEx;
using UnityEngine;

namespace SeikoML
{
    public abstract class RoR2Mod : MonoBehaviour
    {
        public abstract void Awake();
    }


    public class CharacterMod : Attribute
    {
        public string Name { get; protected set; }
    }
}
