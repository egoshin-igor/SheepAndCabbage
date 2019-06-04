using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

namespace Assets.Achievements
{
    public class AchievementsList : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect _achievementsScroll = null;
        [SerializeField]
        private GameObject _achievementTemplate = null;

        private void Awake()
        {
            var templatePosition = _achievementTemplate.transform.position;
            for ( int i = 0; i < AchievementStorage.Achievements.Count; i++ )
            {
                AchievementInfo achievement = AchievementStorage.Achievements[ i ];
                var position = new Vector3( templatePosition.x, templatePosition.y - i * 300, templatePosition.z );
                GameObject achievementCopy = Instantiate( _achievementTemplate, position, Quaternion.identity );
                if ( !achievement.IsAchieved )
                {
                    var image = achievementCopy.GetComponent<UnityEngine.UI.Image>();
                    image.color = Color.black;
                }
                achievementCopy.transform.SetParent( _achievementsScroll.content, false );
                achievementCopy.GetComponentInChildren<Text>().text = achievement.Description;
            }

        }

        private void Update()
        {
        }

    }
}
