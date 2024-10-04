using System;
using UnityEngine;

namespace LegedaryTools.Mothership.Analytics.Firebase
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Analytics/PlayerPrefsGuidUserIdProvider", fileName = "PlayerPrefsGuidUserIdProvider", order = 0)]
    public class PlayerPrefsGuidUserIdProvider : UserIdProvider
    {
        public override bool UserIdIsSet => PlayerPrefs.HasKey("AnalyticsUserID");
        
        public override string GenerateUserId()
        {
            return Guid.NewGuid().ToString();
        }

        public override string Load()
        {
            return PlayerPrefs.GetString("AnalyticsUserID");
        }
        
        public override void Save(string userId)
        {
            PlayerPrefs.SetString("AnalyticsUserID", userId);
        }
        
        public override void Reset()
        {
            PlayerPrefs.DeleteKey("AnalyticsUserID");
        }
    }
}