using System.Collections.Generic;
using UnityEngine;

namespace Assets.Lines
{
    public class LinesCounter
    {
        private readonly List<GameObject> _linesCount;
        private int _current;
        private int _maxLinesCount;

        public LinesCounter( List<GameObject> linesCount, int maxLinesCount )
        {
            _linesCount = linesCount;
            ChangeMaxLinesCount( maxLinesCount );
        }

        public void ChangeMaxLinesCount( int maxLinesCount )
        {
            HideAll();
            _current = 0;
            _maxLinesCount = maxLinesCount;
            ShowLastHiddens( _maxLinesCount );
        }

        public void HideLast()
        {
            if ( _current != -1 )
            {
                _linesCount[ _current-- ].SetActive( false );
            }
        }

        public bool ShowLastHidden()
        {
            if ( _current != _maxLinesCount )
            {
                _linesCount[ ++_current ].SetActive( true );
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ShowLastHiddens( int count )
        {
            for ( int i = 0; i < count; i++ )
            {
                if ( !ShowLastHidden() )
                    return;
            }
        }

        private void HideAll()
        {
            for ( int i = 0; i < _linesCount.Count; i++ )
            {
                _linesCount[ i ].SetActive( false );
            }
        }
    }
}
