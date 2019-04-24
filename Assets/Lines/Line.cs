using UnityEngine;

namespace Assets.Lines
{
    public class Line
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }

        public Line( Vector2 start, Vector2 end )
        {
            Start = start;
            End = end;
        }
    }
}
