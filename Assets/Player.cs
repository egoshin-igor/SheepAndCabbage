using System.Collections.Generic;
using System.Linq;
using Assets.Enums;
using Assets.Lines;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Material _defaultMaterial = null;
        [SerializeField]
        private int _maxLinesCount = 3;
        [SerializeField]
        private Color _linesColor = Color.black;

        private LinesController _linesController;
        private List<LinePlain> _lines = new List<LinePlain>();
        private Vector2 _start;
        private Vector2 _end;
        private CharactersGenerator _charactersGeneratorScript;

        private void Awake()
        {
            _linesController = new LinesController( _defaultMaterial, _maxLinesCount, _linesColor );
            _charactersGeneratorScript = gameObject.GetComponent<CharactersGenerator>();
        }

        private void Update()
        {
            if ( TapUtil.IsLongTap() )
            {
                _linesController.DestroyAll();
                _lines.Clear();
            }
            else if ( TapUtil.IsDoubleTap() )
            {
                _linesController.DestroyLastLine();
                if ( _lines.Count != 0 )
                {
                    _lines.RemoveAt( _lines.Count - 1 );
                }
            }
            else if ( Input.GetMouseButtonDown( 0 ) )
            {
                _start = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            }
            else if ( Input.GetMouseButtonUp( 0 ) )
            {
                _end = Camera.main.ScreenToWorldPoint( Input.mousePosition );

                DrawPlayerLine();
                if ( IsWon() )
                {
                    _linesController.DestroyAll();
                    _lines.Clear();
                    _charactersGeneratorScript.Generate();
                }
            }
        }

        private bool IsWon()
        {
            var typesBySector = new List<HashSet<CharacterType>>();
            for ( int i = 0; i < ( int )System.Math.Pow( 2, _lines.Count ); i++ )
            {
                typesBySector.Add( new HashSet<CharacterType>() );
            }
            List<Character> characters = _charactersGeneratorScript.Characters;

            foreach ( Character character in characters )
            {
                HashSet<int> sectors = GetSectors( character.Position );

                foreach ( int sector in sectors )
                {
                    typesBySector[ sector ].Add( character.Type );
                }
            }

            foreach ( var types in typesBySector )
            {
                if ( types.Count > 1 )
                    return false;
            }

            return true;
        }

        private HashSet<int> GetSectors( Vector2 p )
        {
            var result = new HashSet<int>();
            result.Add( 0 );
            for ( int i = 0; i < _lines.Count; i++ )
            {
                int relationCode = 0;
                if ( _lines[ i ].Relation( p ) == LineRelation.OnLine )
                {
                    var resultCopy = new HashSet<int>( result );
                    foreach ( var item in resultCopy )
                    {
                        result.Add( item + ( int )System.Math.Pow( 2, i ) );
                    }
                }
                if ( _lines[ i ].Relation( p ) == LineRelation.Above )
                {
                    relationCode = 1;
                }
                var newResult = new HashSet<int>();

                foreach ( var item in result )
                {
                    newResult.Add( item + relationCode * ( int )System.Math.Pow( 2, i ) );
                }
                result = newResult;
            }
            return result;
        }

        private void DrawPlayerLine()
        {
            Vector2 _screenStart = Camera.main.WorldToScreenPoint( _start );
            Vector2 _startPoint = new Vector2( _screenStart.x / Screen.width, _screenStart.y / Screen.height );

            Vector2 _screenEnd = Camera.main.WorldToScreenPoint( _end );
            Vector2 _endPoint = new Vector2( _screenEnd.x / Screen.width, _screenEnd.y / Screen.height );

            _lines.Add( new LinePlain( _startPoint, _endPoint ) );
            _linesController.DrawLine( _lines.Last().GetPointsForFullScreen() );
        }
    }
}
