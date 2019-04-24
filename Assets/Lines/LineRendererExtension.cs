using UnityEngine;

namespace Assets.Lines
{
    public static class LineRendererExtension
    {
        public static LineRenderer SetColor( this LineRenderer lineRenderer, Color color )
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            return lineRenderer;
        }

        public static LineRenderer SetWidth( this LineRenderer lineRenderer, float width )
        {
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;

            return lineRenderer;
        }
    }
}