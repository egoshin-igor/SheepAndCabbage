using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    class TravaGenerator : MonoBehaviour
    {
        private const int TravasCount = 220;
        private const int SpinCount = 200;
        private readonly double _minDistance = 0.01;
        [SerializeField]
        private GameObject _trava = null;
        [SerializeField]
        private Button _generateButton = null;

        private System.Random _random = new System.Random();
        private System.Random _randomizer = new System.Random();
        public List<Character> Characters { get; } = new List<Character>();

        void Start()
        {
            _generateButton.onClick.AddListener( OnGenerateButtonClick );
            Generate( TravasCount );
        }

        public void OnGenerateButtonClick()
        {
            StartCoroutine( StartNewDelayed() );
        }

        private IEnumerator StartNewDelayed()
        {
            yield return new WaitForSeconds( 1 );
            Generate( TravasCount );
            yield return null;
        }

        public void Generate( int charactersCount )
        {
            bool isGenerated = false;
            while ( !isGenerated )
            {
                isGenerated = true;
                DestroyGenerated();

                int currentSpin = 0;
                for ( int i = 0; i < charactersCount; i++ )
                {
                    currentSpin = 0;
                    bool isPointHasMinDistanceToPoints = false;
                    Vector2 point;
                    do
                    {
                        point = GetRandomPoint();
                        currentSpin++;

                        isPointHasMinDistanceToPoints = IsPointHasMinDistanceToPoints( point, Characters );
                    } while ( ( !isPointHasMinDistanceToPoints ) && currentSpin <= SpinCount );
                    if ( currentSpin >= SpinCount )
                    {
                        isGenerated = false;
                        break;
                    }

                    Characters.Add( GenerateCharacter( point ) );
                }
            }
        }

        public void DestroyGenerated()
        {
            foreach ( Character character in Characters )
            {
                Object.Destroy( character.Itself );
            }
            Characters.Clear();
        }


        private Character GenerateCharacter( Vector2 point )
        {
            Vector3 pointInScreen = Camera.main.ScreenToWorldPoint( new Vector2( point.x * Screen.width, point.y * Screen.height ) );
            var position = new Vector3( pointInScreen.x, pointInScreen.y, pointInScreen.y );
            GameObject characterTemplate = _trava;
            GameObject characterCopy = Instantiate( characterTemplate, position, Quaternion.identity, transform );
            return new Character
            {
                Itself = characterCopy,
                Position = point
            };
        }

        private Vector2 GetRandomPoint()
        {
            var point = new Vector2( ( float )_randomizer.NextDouble(), ( float )_randomizer.NextDouble() );

            return point;
        }


        private bool IsPointHasMinDistanceToPoint( Vector2 first, Vector2 second )
        {
            double distance = System.Math.Sqrt( ( first.x - second.x ) * ( first.x - second.x ) + ( first.y - second.y ) * ( first.y - second.y ) );
            return distance >= _minDistance * 1.4;
        }

        private bool IsPointHasMinDistanceToPoints( Vector2 first, List<Character> characters )
        {
            foreach ( Character character in characters )
            {
                Vector2 p = character.Position;
                if ( !IsPointHasMinDistanceToPoint( first, p ) )
                    return false;
            }

            return true;
        }
    }
}
