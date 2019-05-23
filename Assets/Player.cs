﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Enums;
using Assets.Lines;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        #region SerializField definitions
        [SerializeField]
        private Material _defaultMaterial = null;
        [SerializeField]
        private Color _linesColor = Color.black;
        [SerializeField]
        private Button _clearAllButton = null;
        [SerializeField]
        private Button _undoButton = null;
        [SerializeField]
        private List<GameObject> _linesCount = new List<GameObject>();
        #endregion

        private LinesCounter _linesCounter;
        private LinesController _linesController;
        private List<LinePlain> _lines = new List<LinePlain>();
        private Vector2 _start;
        private Vector2 _end;
        private CharactersGenerator _charactersGeneratorScript;
        private Timer _timer;
        private LevelManager _levelManager;

        private void Awake()
        {
            _timer = GameObject.Find( "Timer" ).GetComponent<Timer>();
            _levelManager = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();

            _linesController = new LinesController( _defaultMaterial, _levelManager.LinesCount, _linesColor );
            _charactersGeneratorScript = gameObject.GetComponent<CharactersGenerator>();
            _clearAllButton.onClick.AddListener( ClearAll );
            _undoButton.onClick.AddListener( UndoLastAction );
            _linesCounter = new LinesCounter( _linesCount, _levelManager.LinesCount );
        }

        public void ClearAll()
        {
            _linesController.DestroyAll();
            _lines.Clear();
            _linesCounter.ChangeMaxLinesCount( _levelManager.LinesCount );
            _linesCounter.ShowLastHiddens( _levelManager.LinesCount );
            _timer.Stop();
            _linesController.ChangeMaxLinesCount( _levelManager.LinesCount );
            // todo: отнимание сложности
            StartCoroutine( StartNewDelayed() );
        }

        public void UndoLastAction()
        {
            _linesController.DestroyLastLine();
            if ( _lines.Count != 0 )
            {
                _lines.RemoveAt( _lines.Count - 1 );
                _linesCounter.ShowLastHidden();
            }
        }

        private IEnumerator StartNewDelayed()
        {
            yield return new WaitForSeconds( 1 );
            _timer.Restart();
            _timer.Start();
            yield return null;
        }

        private void Update()
        {
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                _start = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            }
            else if ( Input.GetMouseButtonUp( 0 ) )
            {
                _end = Camera.main.ScreenToWorldPoint( Input.mousePosition );

                DrawPlayerLine();
                if ( IsWon() )
                {
                    OnWon();
                }
            }
        }

        private void OnWon()
        {
            _levelManager.OnRoundEnded( _timer.TimeInSeconds );
            _linesController.ChangeMaxLinesCount( _levelManager.LinesCount );
            _linesCounter.ChangeMaxLinesCount( _levelManager.LinesCount );
            _linesCounter.ShowLastHiddens( _levelManager.LinesCount );
            _linesController.DestroyAll();
            _lines.Clear();
            _charactersGeneratorScript.Generate( _levelManager.CharactersCount );
            _timer.Start();
            _timer.Restart();
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
            Vector2 screenStart = Camera.main.WorldToScreenPoint( _start );
            Vector2 startPoint = new Vector2( screenStart.x / Screen.width, screenStart.y / Screen.height );

            Vector2 screenEnd = Camera.main.WorldToScreenPoint( _end );
            Vector2 endPoint = new Vector2( screenEnd.x / Screen.width, screenEnd.y / Screen.height );

            if ( Vector2.Distance( startPoint, endPoint ) > 0.05 )
            {
                _lines.Add( new LinePlain( startPoint, endPoint ) );
                _linesController.DrawLine( _lines.Last().GetPointsForFullScreen() );
                _linesCounter.HideLast();
            }
        }
    }
}
