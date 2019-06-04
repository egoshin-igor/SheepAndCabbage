using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Achievements
{
    public static class AchievementStorage
    {
        private const string AchievmentsStorageName = "Achievments";
        private readonly static Dictionary<AchievementType, AchievementInfo> _achievementByType;

        public static List<AchievementInfo> Achievements => _achievementByType.Values.ToList();

        static AchievementStorage()
        {
            string json = PlayerPrefs.GetString( AchievmentsStorageName );
            if ( !string.IsNullOrWhiteSpace( json ) )
            {
                _achievementByType = JsonConvert.DeserializeObject<Dictionary<AchievementType, AchievementInfo>>( json );
                return;
            }

            _achievementByType = new Dictionary<AchievementType, AchievementInfo>();
            _achievementByType.Add( AchievementType.FirstLevelPassed, new AchievementInfo
            {
                Message = "First level passed!",
                Description = "Pass any level first time",
                Type = AchievementType.FirstLevelPassed
            } );
            _achievementByType.Add( AchievementType.TwoHundredPointsAchieved, new AchievementInfo
            {
                Message = "Got two handreeds points",
                Description = "Get 200 points in general",
                Type = AchievementType.TwoHundredPointsAchieved
            } );
            _achievementByType.Add( AchievementType.GameWon, new AchievementInfo
            {
                Message = "You have completed the game!!!",
                Description = "Get 4th level and fill difficulty scale",
                Type = AchievementType.GameWon
            } );
        }

        public static AchievementInfo Get( AchievementType type )
        {
            return _achievementByType[ type ];
        }

        public static void SetAchieved( AchievementType type )
        {
            _achievementByType[ type ].IsAchieved = true;
            PlayerPrefs.SetString( AchievmentsStorageName, JsonConvert.SerializeObject( _achievementByType ) );
        }
    }
}
