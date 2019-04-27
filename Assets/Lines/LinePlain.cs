using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Enums;
using Assets.Lines;

public class LinePlain
{
    // Ax + By + C = 0
    public float A { get; }
    public float B { get; }
    public float C { get; }

    public LinePlain( Vector2 p1, Vector2 p2 )
    {
        A = p1.y - p2.y;
        B = p2.x - p1.x;
        C = p1.x * p2.y - p2.x * p1.y;
    }

    public Line GetPointsForFullScreen()
    {
        Vector2 start = Camera.main.ScreenToWorldPoint( new Vector2( -0.1f * Screen.width, GetYByX( -0.1f ) * Screen.height ) );
        Vector2 end = Camera.main.ScreenToWorldPoint( new Vector2( 1.1f * Screen.width, GetYByX( 1.1f ) * Screen.height ) );

        return new Line( start, end );
    }

    public LineRelation Relation( Vector2 point )
    {
        float lineY = GetYByX( point.x );

        return ( point.y < lineY ) ? LineRelation.Under : LineRelation.Above;
    }

    private float GetYByX( float x )
    {
        return -( ( C + A * x ) / B );
    }
}
