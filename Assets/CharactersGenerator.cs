using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Enums;
using Assets.Lines;
using Assets.Util;
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
        private List<LinePlain> _lps = new List<LinePlain>();
        private LevelManager _levelManager;

        public List<Character> Characters { get; } = new List<Character>();

        void Start()
        {
            _levelManager = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();
            _linesController = new LinesController( _defaultMaterial, _levelManager.LinesCount, Color.black );
            _generateButton.onClick.AddListener( OnGenerateButtonClick );
            Generate( _levelManager.CharactersCount );
        }

        public void OnGenerateButtonClick()
        {
            _linesController.ChangeMaxLinesCount( _levelManager.LinesCount );
            foreach ( LinePlain lp in _lps )
            {
                _linesController.DrawLine( lp.GetPointsForFullScreen() );
            }
            StartCoroutine( StartNewDelayed() );
        }

        public void Generate( int charactersCount )
        {
            bool isGenerated = false;
            while ( !isGenerated )
            {
                isGenerated = true;
                DestroyGenerated();

                var points = new List<Vector2>();

                for ( int i = 0; i < 4; i++ )
                {
                    points.Add( GetRandomPointBySector( i ) );
                }

                switch ( _levelManager.LinesCount )
                {
                    case 3:
                    case 2:
                        points.RemoveAt( _random.Next() % 4 );
                        break;
                    default:
                        break;
                }
                _lps.Shuffle();
                if ( _levelManager.LinesCount == 2 )
                {
                    _lps.Add( new LinePlain( points[ 0 ], points[ 1 ] ) );
                    _lps.Add( new LinePlain( points[ 2 ], points[ 1 ] ) );
                }
                else
                {
                    for ( int i = 0; i < _levelManager.LinesCount; i++ )
                    {
                        _lps.Add( new LinePlain( points[ i ], points[ ( i + 1 ) % points.Count ] ) );
                    }
                }


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

                        isPointHasMinDistanceToLines = IsPointHasMinDistanceToLines( _lps, point );
                        isPointHasMinDistanceToPoints = IsPointHasMinDistanceToPoints( point, Characters );
                    } while ( ( !isPointHasMinDistanceToLines || !isPointHasMinDistanceToPoints ) && currentSpin <= SpinCount );
                    if ( currentSpin >= SpinCount )
                    {
                        isGenerated = false;
                        break;
                    }
                    int k = 0;

                    for ( int j = 0; j < _lps.Count; j++ )
                    {
                        k += _lps[ j ].Relation( point ) == LineRelation.Above ? 1 : 0;
                    }

                    CharacterType characterType = k % 2 == _characterSeparator ? CharacterType.Cabbage : CharacterType.Sheep;
                    Characters.Add( GenerateCharacter( point, characterType ) );
                }

                _characterSeparator = _randomizer.Next( 1000 ) % 2;
                if ( isGenerated )
                {
                    if ( Characters.Select( c => c.Type ).Distinct().Count() != 2 )
                    {
                        isGenerated = false;
                    }
                }
            }
        }

        public void DestroyGenerated()
        {
            _lps.Clear();
            _linesController.DestroyAll();
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

        private IEnumerator StartNewDelayed()
        {
            yield return new WaitForSeconds( 1 );
            _linesController.DestroyAll();
            Generate( _levelManager.CharactersCount );
            yield return null;
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
                    x = ( float )( _random.NextDouble() * ( 0.45 - 0.1 ) + 0.1 );
                    y = ( float )( _random.NextDouble() * ( 0.45 - 0.1 ) + 0.1 );
                    break;
                case 1:
                    x = ( float )( _random.NextDouble() * ( 0.9 - 0.55 ) + 0.55 );
                    y = ( float )( _random.NextDouble() * ( 0.1 - 0.45 ) + 0.45 );
                    break;
                case 2:
                    x = ( float )( _random.NextDouble() * ( 0.45 - 0.1 ) + 0.1 );
                    y = ( float )( _random.NextDouble() * ( 0.9 - 0.55 ) + 0.55 );
                    break;
                case 3:
                    x = ( float )( _random.NextDouble() * ( 0.9 - 0.55 ) + 0.55 );
                    y = ( float )( _random.NextDouble() * ( 0.9 - 0.55 ) + 0.55 );
                    break;
                default:
                    throw new System.ApplicationException();
            }

            return new Vector2( x, y );
        }
    }
}
