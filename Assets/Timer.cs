using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Timer : MonoBehaviour
    {
        public Text TimerLabel = null;

        private float _time = 0;
        private bool _stop = false;

        public int TimeInSeconds => ( int )_time;

        void Update()
        {
            if ( _stop )
                return;

            _time += Time.deltaTime;

            float minutes = _time / 60;
            float seconds = _time % 60;
            float fraction = ( _time * 100 ) % 100;

            TimerLabel.text = string.Format( "{0:00} : {1:00}", ( int )minutes, ( int )seconds );
        }

        public void Restart()
        {
            _time = 0;
        }

        public void Stop()
        {
            _stop = true;
        }

        public void Start()
        {
            _stop = false;
        }
    }
}
