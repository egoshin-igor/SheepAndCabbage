using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Lines
{
    class LinesController
    {
        private readonly List<GameObject> _lines = new List<GameObject>();

        private readonly Material _defaultMaterial;
        private int _maxLinesCount;
        private Color _linesColor;

        public LinesController( Material material, int maxLinesCount, Color linesColor )
        {
            _defaultMaterial = material;
            _maxLinesCount = maxLinesCount;
            _linesColor = linesColor;
        }

        public void ChangeMaxLinesCount( int count )
        {
            _maxLinesCount = count;
        }

        public void DrawLine( Line line )
        {
            if ( _lines.Count < _maxLinesCount )
            {
                GameObject drawedLine = LineDrawer.GetDrawed( line, _linesColor, _defaultMaterial );
                _lines.Add( drawedLine );
            }
        }

        public void DestroyLastLine()
        {
            if ( _lines.Count > 0 )
            {
                Object.Destroy( _lines.Last() );
                _lines.RemoveAt( _lines.Count - 1 );
            }
        }

        public void DestroyAll()
        {
            foreach ( GameObject line in _lines )
            {
                Object.Destroy( line );
            }
            _lines.Clear();
        }
    }
}
