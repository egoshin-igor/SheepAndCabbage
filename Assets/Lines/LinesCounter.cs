using System.Collections.Generic;
using UnityEngine;

namespace Assets.Lines
{
    public class LinesCounter
    {
        private readonly List<GameObject> _linesCount;
        private int _current;

        public LinesCounter( List<GameObject> linesCount )
        {
            _linesCount = linesCount;
            _current = _linesCount.Count - 1;

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
            if ( _current + 1 != _linesCount.Count )
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
    }
}
