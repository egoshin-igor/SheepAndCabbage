namespace Assets.Achievements
{
    public class AchievementInfo
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public AchievementType Type { get; set; }
        public bool IsAchieved { get; set; } = false;
    }
}
