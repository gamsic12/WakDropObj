using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DogGame.Game
{
    public partial class InGeme
    {
        public class Manager : Core.InGame.Manager
        {
            public override void Prepare()
            {
                base.Prepare();
                // Add(new MainMenu());
            }
        }
    }
}