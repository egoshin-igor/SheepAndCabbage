using UnityEngine;

namespace Assets.Lines
{
    public static class LineDrawer
    {
        public static GameObject GetDrawed( Line line, Material material )
        {
            var startV3 = new Vector3( line.Start.x, line.Start.y, 0 );
            var endV3 = new Vector3( line.End.x, line.End.y, 0 );

            var result = new GameObject( "Line" );
            var lineRenderer = result.AddComponent<LineRenderer>();
            lineRenderer.SetWidth( 0.3f );
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition( 0, startV3 );
            lineRenderer.SetPosition( 1, endV3 );
            lineRenderer.material = new Material( material );
            float distance = Vector2.Distance( line.Start, line.End );
            lineRenderer.material.mainTextureScale = new Vector2( distance / 40 * 6, 1 );

            return result;
        }
    }
}
