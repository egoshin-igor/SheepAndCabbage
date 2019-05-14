using Assets.Enums;
using UnityEngine;

namespace Assets
{
    public class Character
    {
        public GameObject Itself { get; set; }
        public CharacterType Type { get; set; }
        public Vector2 Position { get; set; }
    }
}
