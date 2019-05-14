using UnityEngine;

namespace Assets.Util
{
    public static class TapUtil
    {
        public static bool IsDoubleTap()
        {
            bool result = false;
            float MaxTimeWait = 1;
            float VariancePosition = 1;

            if ( Input.touchCount == 1 && Input.GetTouch( 0 ).phase == TouchPhase.Began )
            {
                float DeltaTime = Input.GetTouch( 0 ).deltaTime;
                float DeltaPositionLenght = Input.GetTouch( 0 ).deltaPosition.magnitude;

                if ( DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition )
                    result = true;
            }
            return result;
        }

        public static bool IsLongTap()
        {
            float touchTime = 0;
            Touch touch = Input.touches[ 0 ];
            if ( touch.phase == TouchPhase.Began )
            {
                touchTime = Time.time;
            }

            if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
            {
                if ( Time.time - touchTime >= 0.5 )
                    return true;
            }

            return false;
        }
    }
}
