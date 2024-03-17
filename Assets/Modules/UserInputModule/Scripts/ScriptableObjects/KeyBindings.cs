using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SDRGames.Whist.UserInputModule.ScriptableObjects
{
    public abstract class KeyBindings : ScriptableObject
    {
        public abstract Key[] GetKeys(); 
    }
}
