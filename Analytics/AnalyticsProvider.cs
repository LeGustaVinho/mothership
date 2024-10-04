using System.Collections.Generic;

namespace LegedaryTools.Mothership.Analytics
{
    public interface IAnalyticsProvider : IInitalizeable
    {
        string UserId { get; set; }
        void LogEvent(string eventName, string parameterName, string parameterValue);
        void LogEvent(string eventName, string parameterName, double parameterValue);
        void LogEvent(string eventName, string parameterName, long parameterValue);
        void LogEvent(string eventName, string parameterName, int parameterValue);
        void LogEvent(string eventName);
        void LogEvent(string eventName, Dictionary<string, object> parameters);
        void SetUserProperty(string propertyName, string value);
        void SetConsent(bool granted);
    }
    
    public abstract class AnalyticsProvider : BaseProvider, IAnalyticsProvider
    {
        public abstract string UserId { get; set; }
        public abstract void LogEvent(string eventName, string parameterName, string parameterValue);
        public abstract void LogEvent(string eventName, string parameterName, double parameterValue);
        public abstract void LogEvent(string eventName, string parameterName, long parameterValue);
        public abstract void LogEvent(string eventName, string parameterName, int parameterValue);
        public abstract void LogEvent(string eventName);
        public abstract void LogEvent(string eventName, Dictionary<string, object> parameters);
        public abstract void SetUserProperty(string propertyName, string value);
        public abstract void SetConsent(bool granted);
    }
}