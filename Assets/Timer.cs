using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private Text _timerLabel = null;

        private float _time = 0;
        private bool _stop = false;

        public int TimeInSeconds => ( int )_time;

        void Update()
        {
            if ( _stop )
                return;

            _time += Time.deltaTime;

            var minutes = _time / 60;
            var seconds = _time % 60;
            var fraction = ( _time * 100 ) % 100;

            _timerLabel.text = string.Format( "{0:00} : {1:00}", minutes, seconds, fraction );
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
