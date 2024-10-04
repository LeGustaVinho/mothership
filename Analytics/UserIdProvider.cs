using UnityEngine;

namespace LegedaryTools.Mothership.Analytics.Firebase
{
    public abstract class UserIdProvider : ScriptableObject
    {
        public abstract bool UserIdIsSet { get; }
        public abstract string GenerateUserId();
        public abstract string Load();
        public abstract void Save(string userId);
        public abstract void Reset();
    }
}