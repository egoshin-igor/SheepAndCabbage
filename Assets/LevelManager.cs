using System.Collections.Generic;
using Assets.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class LevelManager : MonoBehaviour
    {
        private const int MinCharactersCount = 6;
        private const int MaxCharactersCount = 40;

        [SerializeField]
        private Text _scoreLabel = null;
        [SerializeField]
        private Text _diffucultLabel = null;

        private List<int> _timeoutForLinesCount;
        private int _score = 0;
        private double _difficultCf = 0.01;

        public int CharactersCount { get; private set; } = MinCharactersCount;
        public int LinesCount { get; private set; } = 3;


        public void Awake()
        {
            _timeoutForLinesCount = new List<int> { 0, 0, 0, 40, 0 };
        }

        public void OnRoundEnded( int time )
        {
            _difficultCf = ( ( double )CharactersCount - MinCharactersCount ) / ( MaxCharactersCount - MinCharactersCount );
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

            _scoreLabel.text = _score.ToString();
            _diffucultLabel.text = $"{( int )( _difficultCf * 100 )} / 100";
        }
    }
}
