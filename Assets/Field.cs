using System;
using Assets;
using Assets.Lines;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private Material _defaultMaterial = null;
    [SerializeField]
    private GameObject _cabbage = null;
    [SerializeField]
    private GameObject _sheep = null;
    private CharactersGenerator _charactersGenerator;

    private LinesController _linesController;
    private System.Random _random = new System.Random();

    void Start()
    {
        _linesController = new LinesController( _defaultMaterial, 3, Color.black );
        _charactersGenerator = new CharactersGenerator( _cabbage, _sheep, gameObject );
    }

    void Update()
    {
        if ( Input.GetKeyDown( "space" ) )
        {
            GenerateField();
        }
    }

    private void GenerateField()
    {
        _linesController.DestroyAll();
        _charactersGenerator.DestroyGenerated();
        Vector2 p1 = GetRandomPointBySector( 0 );
        Vector2 p2 = GetRandomPointBySector( 1 );
        Vector2 p3 = GetRandomPointBySector( 2 );
        LinePlain lp1 = new LinePlain( p1, p2 );
        LinePlain lp2 = new LinePlain( p2, p3 );
        LinePlain lp3 = new LinePlain( p3, p1 );

        _charactersGenerator.Generate( lp1, lp2, lp3 );
        _linesController.DrawLine( lp1.GetPointsForFullScreen() );
        _linesController.DrawLine( lp2.GetPointsForFullScreen() );
        _linesController.DrawLine( lp3.GetPointsForFullScreen() );
    }

    // есть 3 сектора
    private Vector2 GetRandomPointBySector( int sectorNum )
    {
        float x, y;
        //         float K = ( float )( random.NextDouble() * ( 1.7 - 0.3 ) + 0.3 );
        switch ( sectorNum )
        {
            case 0:
                x = ( float )( _random.NextDouble() * ( 0.9 - 0.1 ) + 0.1 );
                y = ( float )( _random.NextDouble() * ( 0.4 - 0.1 ) + 0.1 );
                break;
            case 1:
                x = ( float )( _random.NextDouble() * ( 0.4 - 0.1 ) + 0.1 );
                y = ( float )( _random.NextDouble() * ( 0.9 - 0.5 ) + 0.5 );
                break;
            case 2:
                x = ( float )( _random.NextDouble() * ( 0.9 - 0.5 ) + 0.5 );
                y = ( float )( _random.NextDouble() * ( 0.9 - 0.5 ) + 0.5 );
                break;
            default:
                throw new ApplicationException();
        }

        return new Vector2( x, y );
    }
}
