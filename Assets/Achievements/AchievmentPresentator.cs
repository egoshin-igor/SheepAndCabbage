using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Achievements
{
    public class AchievmentPresentator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _achievementBackground = null;
        [SerializeField]
        private Text _achievementLabel = null;

        private void Awake()
        {
            _achievementBackground.SetActive( false );
        }

        public void Show( AchievementType type )
        {
            AchievementInfo achievementInfo = AchievementStorage.Get( type );
            if ( achievementInfo.IsAchieved )
                return;

            AchievementStorage.SetAchieved( type );
            _achievementLabel.text = achievementInfo.Message;
            _achievementBackground.SetActive( true );
            StartCoroutine( HideDelayed() );
        }

        private IEnumerator HideDelayed()
        {
            yield return new WaitForSeconds( 2f );
            _achievementBackground.SetActive( false );
            yield return null;
        }
    }
}
