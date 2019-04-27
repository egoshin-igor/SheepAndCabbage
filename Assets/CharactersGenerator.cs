using System.Collections.Generic;
using Assets.Enums;
using UnityEngine;

namespace Assets
{
    public class CharactersGenerator
    {
        private readonly GameObject _cabbage;
        private readonly GameObject _sheep;
        private readonly GameObject _parent;
        private readonly List<GameObject> _characters = new List<GameObject>();

        private System.Random _randomizer = new System.Random();
        private int _characterSeparator = 0;

        public CharactersGenerator( GameObject cabbage, GameObject sheep, GameObject parent )
        {
            _cabbage = cabbage;
            _sheep = sheep;
            _parent = parent;
        }

        public void Generate( LinePlain lp1, LinePlain lp2, LinePlain lp3 )
        {
            _characterSeparator = _randomizer.Next( 1000 ) % 2;

            for ( int i = 0; i < 30; i++ )
            {
                int k = 0;
                Vector2 point = GetRandomPoint();
                k += lp1.Relation( point ) == LineRelation.Above ? 1 : 0;
                k += lp2.Relation( point ) == LineRelation.Above ? 1 : 0;
                k += lp3.Relation( point ) == LineRelation.Above ? 1 : 0;
                var character = k % 2 == _characterSeparator ? _cabbage : _sheep;
                _characters.Add( GenerateCharacter( point, character ) );
            }
        }

        public void DestroyGenerated()
        {
            foreach ( GameObject character in _characters )
            {
                Object.Destroy( character );
            }
            _characters.Clear();
        }

        private GameObject GenerateCharacter( Vector2 point, GameObject character )
        {
            var pointInScreen = Camera.main.ScreenToWorldPoint( new Vector2( point.x * Screen.width, point.y * Screen.height ) );
            var position = new Vector3( pointInScreen.x, pointInScreen.y, 0 );

            return Object.Instantiate( character, position, Quaternion.identity, _parent.transform );
        }

        private Vector2 GetRandomPoint()
        {
            var point = new Vector2( ( float )_randomizer.NextDouble(), ( float )_randomizer.NextDouble() );

            return point;
        }
    }
}
