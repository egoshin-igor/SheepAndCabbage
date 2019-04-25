using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LinePlain {
        // y = k * x + b
        public float K { get; }
        public float B { get; }

        public LinePlain(float k, float b) 
        {
            K = k;
            B = b;
        }

        public LinePlain(Vector2 p1, Vector2 p2) 
        {
            K = Math.Abs(p1.y - p2.y) / Math.Abs(p1.x - p2.x);
            B = p1.y - K * p1.x;
        }

        public (Vector2, Vector2)getPointsForFullScreen() 
        {
            Vector2 start = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height * B));
            Vector2 end =  Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height * (K + B)));

            return (start, end);
        }

        public LineRelation relation(Vector2 point)
        {
            return (point.y > K * point.x + B ) ? LineRelation.Above : LineRelation.Under;
        }
    }
