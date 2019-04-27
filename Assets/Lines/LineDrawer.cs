using UnityEngine;

namespace Assets.Lines
{
    public static class LineDrawer
    {
        public static GameObject GetDrawed( Line line, Color color, Material material )
        {
            var startV3 = new Vector3( line.Start.x, line.Start.y, 0 );
            var endV3 = new Vector3( line.End.x, line.End.y, 0 );

            var result = new GameObject( "Line" );
            var lineRenderer = result.AddComponent<LineRenderer>();
            lineRenderer.SetColor( color );
            lineRenderer.SetWidth( 0.1f );
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition( 0, startV3 );
            lineRenderer.SetPosition( 1, endV3 );
            lineRenderer.material = material;

            return result;
        }
    }
}
