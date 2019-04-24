using System.Collections.Generic;
using System.Linq;
using Assets.Lines;
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
        private Vector2 _start;
        private Vector2 _end;

        private void Awake()
        {
            _linesController = new LinesController( _defaultMaterial, _maxLinesCount, _linesColor );
        }

        private void Update()
        {
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                _start = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            }
            if ( Input.GetMouseButtonUp( 0 ) )
            {
                _end = Camera.main.ScreenToWorldPoint( Input.mousePosition );
                _linesController.DrawLine( new Line( _start, _end ) );
            }
            if ( Input.GetMouseButtonUp( 1 ) )
            {
                _linesController.DestroyLastLine();
            }
        }
    }
}
