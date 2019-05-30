using System;
using System.Collections.Generic;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class LevelManager : MonoBehaviour
    {
        private const int MinLinesCount = 2;
        private const int MaxLinesCount = 4;

        private const int MinCharactersCount = 6;
        private const int MaxCharactersCount = 40;

        [SerializeField]
        private Text _scoreLabel = null;
        [SerializeField]
        private Text _diffucultLabel = null;

        private List<int> _timeoutForLinesCount;
        private List<int> _linesCountCfs;
        private int _score = 0;
        private double _difficultCf = 0.01;

        public int CharactersCount { get; private set; } = MinCharactersCount;
        public int LinesCount { get; private set; } = MinLinesCount;


        public void Awake()
        {
            _timeoutForLinesCount = new List<int> { 0, 0, 20, 40, 60 };
            _linesCountCfs = new List<int> { 0, 0, 2, 10, 25 };
            LoadData();
        }

        public void OnRoundEnded( int time )
        {
            _difficultCf = ( ( double )CharactersCount - MinCharactersCount ) / ( MaxCharactersCount - MinCharactersCount );
            _difficultCf *= 10 / _linesCountCfs[ LinesCount ];
            _difficultCf = _difficultCf > 1 ? 1 : _difficultCf;
            _difficultCf = Math.Round( _difficultCf, 2 );
            if ( DoubleUtil.EqualDoubles( _difficultCf, 0 ) )
            {
                _difficultCf = 0.01;
            }

            double timeCf = ( ( double )_timeoutForLinesCount[ LinesCount ] - time ) / _timeoutForLinesCount[ LinesCount ];
            if ( timeCf < 0 )
            {
                timeCf = 0;
            }

            _score += ( int )( CharactersCount * timeCf * LinesCount );
            CharactersCount += ( int )( ( ( timeCf + _difficultCf ) / 2 ) * ( MaxCharactersCount - MinCharactersCount ) * 0.1 );
            if ( CharactersCount > MaxCharactersCount )
            {
                CharactersCount = MaxCharactersCount;
            }
            else if ( CharactersCount < MinCharactersCount )
            {
                CharactersCount = MinCharactersCount;
            }

            if ( ( int )( _difficultCf * 100 ) <= 0 && LinesCount != MinLinesCount )
            {
                _difficultCf = 0.98;
                LinesCount -= 1;
            }
            else if ( ( int )( _difficultCf * 100 ) >= 100 && LinesCount != MaxCharactersCount )
            {
                _difficultCf = 0;
                LinesCount += 1;
            }
            _diffucultLabel.text = $"{( int )( _difficultCf * 100 )} / 100";
            _scoreLabel.text = _score.ToString();
            SaveData();
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt( "score", _score );
            PlayerPrefs.SetFloat( "difficultCf", ( float )_difficultCf );
            PlayerPrefs.SetInt( "linesCount", LinesCount );
            PlayerPrefs.SetInt( "charactersCount", CharactersCount );
        }

        private void LoadData()
        {
            _score = PlayerPrefs.GetInt( "score", 0 );
            _difficultCf = Math.Round( PlayerPrefs.GetFloat( "difficultCf", 0 ), 2 );
            LinesCount = PlayerPrefs.GetInt( "linesCount", MinLinesCount );
            CharactersCount = PlayerPrefs.GetInt( "charactersCount", MinCharactersCount );
            _diffucultLabel.text = $"{( int )( _difficultCf * 100 )} / 100";
            _scoreLabel.text = _score.ToString();
        }
    }
}
