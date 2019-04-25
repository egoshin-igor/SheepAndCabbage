using System.Collections.Generic;
using System.Linq;
using Assets.Lines;
using UnityEngine;
using System;

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

        private LinePlain _l1;
        private LinePlain _l2;

        private void GenerateRandomLines()
        {
            _linesController.DestroyAll();
            System.Random random = new System.Random();
            float K = (float) (random.NextDouble() * (1.7 - 0.3) + 0.3);
            float B = (float) (random.NextDouble() * (0.25 + 0.25) - 0.25);
            _l1 = new LinePlain(K, B);

            K = (float) (random.NextDouble() * (-0.3 + 1.7) -1.7);
            B = (float) (random.NextDouble() * (1.25 - 0.75) + 0.75);
            _l2 = new LinePlain(K, B);

            var points = _l1.getPointsForFullScreen();
            _linesController.DrawLine( new Line(points.Item1, points.Item2) );
            // _linesController.DrawLine( new Line(new Vector2(0, 0), new Vector2(1, 1)) );

            points = _l2.getPointsForFullScreen();
            _linesController.DrawLine( new Line(points.Item1, points.Item2) );
        }

        private void Awake()
        {
            _linesController = new LinesController( _defaultMaterial, _maxLinesCount, _linesColor );
        }

        private void Update()
        {
            if( Input.GetKeyDown("space") )
            {
                GenerateRandomLines();
            }

            if ( Input.GetMouseButtonDown( 0 ) )
            {
                _start = Camera.main.ScreenToWorldPoint( Input.mousePosition );
                print(Input.mousePosition);
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
