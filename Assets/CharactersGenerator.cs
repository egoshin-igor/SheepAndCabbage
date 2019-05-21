using System.Collections.Generic;
using Assets.Enums;
using Assets.Lines;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class CharactersGenerator : MonoBehaviour
    {
        private const int SpinCount = 200;

        [SerializeField]
        private Material _defaultMaterial = null;
        [SerializeField]
        private GameObject _cabbage = null;
        [SerializeField]
        private GameObject _sheep = null;
        [SerializeField]
        private double _minDistance = 0.05;
        [SerializeField]
        private Button _generateButton = null;

        private LinesController _linesController;
        private System.Random _random = new System.Random();

        private System.Random _randomizer = new System.Random();
        private int _characterSeparator = 0;
        private LevelManager _levelManager;

        public List<Character> Characters { get; } = new List<Character>();

        void Start()
        {
            _levelManager = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();
            _linesController = new LinesController( _defaultMaterial, 3, Color.black );
            _generateButton.onClick.AddListener( () => Generate( _levelManager.CharactersCount ) );
            Generate( _levelManager.CharactersCount );
        }

        public void Generate( int charactersCount )
        {
            bool isGenerated = false;
            while ( !isGenerated )
            {
                isGenerated = true;
                _linesController.DestroyAll();
                DestroyGenerated();

                Vector2 p1 = GetRandomPointBySector( 0 );
                Vector2 p2 = GetRandomPointBySector( 1 );
                Vector2 p3 = GetRandomPointBySector( 2 );
                LinePlain lp1 = new LinePlain( p1, p2 );
                LinePlain lp2 = new LinePlain( p2, p3 );
                LinePlain lp3 = new LinePlain( p3, p1 );

                int currentSpin = 0;
                for ( int i = 0; i < charactersCount; i++ )
                {
                    currentSpin = 0;
                    bool isPointHasMinDistanceToLines, isPointHasMinDistanceToPoints = false;
                    Vector2 point;
                    do
                    {
                        point = GetRandomPoint();
                        currentSpin++;

                        isPointHasMinDistanceToLines = IsPointHasMinDistanceToLines( new List<LinePlain> { lp1, lp2, lp3 }, point );
                        isPointHasMinDistanceToPoints = IsPointHasMinDistanceToPoints( point, Characters );
                    } while ( ( !isPointHasMinDistanceToLines || !isPointHasMinDistanceToPoints ) && currentSpin <= SpinCount );
                    if ( currentSpin >= SpinCount )
                    {
                        isGenerated = false;
                        break;
                    }
                    int k = 0;
                    k += lp1.Relation( point ) == LineRelation.Above ? 1 : 0;
                    k += lp2.Relation( point ) == LineRelation.Above ? 1 : 0;
                    k += lp3.Relation( point ) == LineRelation.Above ? 1 : 0;
                    CharacterType characterType = k % 2 == _characterSeparator ? CharacterType.Cabbage : CharacterType.Sheep;
                    Characters.Add( GenerateCharacter( point, characterType ) );
                }

                // _linesController.DrawLine( lp1.GetPointsForFullScreen() );
                // _linesController.DrawLine( lp2.GetPointsForFullScreen() );
                // _linesController.DrawLine( lp3.GetPointsForFullScreen() );

                _characterSeparator = _randomizer.Next( 1000 ) % 2;
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

        private Character GenerateCharacter( Vector2 point, CharacterType characterType )
        {
            Vector3 pointInScreen = Camera.main.ScreenToWorldPoint( new Vector2( point.x * Screen.width, point.y * Screen.height ) );
            var position = new Vector3( pointInScreen.x, pointInScreen.y, 0 );
            GameObject characterTemplate = characterType == CharacterType.Sheep ? _sheep : _cabbage;
            GameObject characterCopy = Instantiate( characterTemplate, position, Quaternion.identity, transform );
            return new Character
            {
                Itself = characterCopy,
                Position = point,
                Type = characterType
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

        private bool IsPointHasMinDistanceToLine( LinePlain lp, Vector2 p )
        {
            if ( lp.A == 0 && lp.B == 0 )
                return false;

            double distance = System.Math.Abs( lp.A * p.x + lp.B * p.y + lp.C ) / System.Math.Sqrt( lp.A * lp.A + lp.B * lp.B );

            return distance >= _minDistance;
        }

        private bool IsPointHasMinDistanceToLines( List<LinePlain> lps, Vector2 p )
        {
            foreach ( LinePlain lp in lps )
            {
                if ( !IsPointHasMinDistanceToLine( lp, p ) )
                    return false;
            }

            return true;
        }

        private Vector2 GetRandomPointBySector( int sectorNum )
        {
            float x, y;
            switch ( sectorNum )
            {
                case 0:
                    x = ( float )( _random.NextDouble() * ( 0.9 - 0.1 ) + 0.1 );
                    y = ( float )( _random.NextDouble() * ( 0.4 - 0.1 ) + 0.1 );
                    break;
                case 1:
                    x = ( float )( _random.NextDouble() * ( 0.4 - 0.1 ) + 0.1 );
                    y = ( float )( _random.NextDouble() * ( 0.9 - 0.5 ) + 0.5 );
                    break;
                case 2:
                    x = ( float )( _random.NextDouble() * ( 0.9 - 0.5 ) + 0.5 );
                    y = ( float )( _random.NextDouble() * ( 0.9 - 0.5 ) + 0.5 );
                    break;
                default:
                    throw new System.ApplicationException();
            }

            return new Vector2( x, y );
        }
    }
}
