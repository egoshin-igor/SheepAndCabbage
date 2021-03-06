﻿using System;
using System.Collections.Generic;
using Assets.Achievements;
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
        private Text _pointsPerLevelLabel = null;

        [SerializeField]
        private Texture2D _progressEmptyImage = null;
        [SerializeField]
        private Texture2D _progressFullImage = null;
        [SerializeField]
        private Vector2 _barPosition = new Vector2( 20, 40 );
        [SerializeField]
        private Vector2 _barSize = new Vector2( 500, 30 );

        private List<int> _timeoutForLinesCount;
        private List<int> _linesCountCfs;
        private int _score = 0;
        private double _difficultCf = 0.01;
        private AchievmentPresentator _achievmentPresentator;

        public int CharactersCount { get; private set; } = MinCharactersCount;
        public int LinesCount { get; private set; } = MinLinesCount;
        public int MaxTime => _timeoutForLinesCount[ LinesCount ];

        public void OnGUI()
        {
            var color = GUI.color;
            GUI.color = new Color( color.r, color.g, color.b, 0.7f );
            GUI.DrawTexture( new Rect( _barPosition.x, _barPosition.y, _barSize.x, _barSize.y ), _progressEmptyImage );
            GUI.color = new Color( color.r, color.g, color.b, 0.9f );
            GUI.DrawTexture( new Rect( _barPosition.x, _barPosition.y, _barSize.x * Mathf.Clamp01( ( float )_difficultCf ), _barSize.y ), _progressFullImage );
            GUI.color = color;
        }

        public void Awake()
        {
            _timeoutForLinesCount = new List<int> { 0, 0, 30, 50, 80 };
            _linesCountCfs = new List<int> { 0, 0, 2, 10, 25 };
            _achievmentPresentator = GameObject.Find( "AchievmentPresentator" ).GetComponent<AchievmentPresentator>();
            LoadData();
        }

        public void OnRoundEnded( int time )
        {
            if ( _score == 0 )
            {
                _achievmentPresentator.Show( AchievementType.FirstLevelPassed );
            }
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

            int newPoints = ( int )( CharactersCount * timeCf * LinesCount );
            _score += newPoints;
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
            _scoreLabel.text = _score.ToString();
            _pointsPerLevelLabel.text = newPoints.ToString();
            if ( _score >= 200 )
            {
                _achievmentPresentator.Show( AchievementType.TwoHundredPointsAchieved );
            }
            if ( LinesCount == MaxLinesCount && DoubleUtil.EqualDoubles( _difficultCf, 1d ) )
            {
                _achievmentPresentator.Show( AchievementType.GameWon );
            }

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
            _scoreLabel.text = _score.ToString();
        }
    }
}
