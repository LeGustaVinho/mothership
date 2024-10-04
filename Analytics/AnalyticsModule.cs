using System.Collections.Generic;
using UnityEngine;

namespace LegedaryTools.Mothership.Analytics
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Modules/AnalyticsModule", fileName = "AnalyticsModule", order = 0)]
    public class AnalyticsModule : MothershipModule, IAnalyticsProvider
    {
        public string UserId
        {
            get
            {
                foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
                {
                    return analyticsProvider.UserId;
                }
                return null;
            }
            set
            {
                foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
                {
                    analyticsProvider.UserId = value;
                }
            }
        }
        
        public void LogEvent(string eventName, string parameterName, string parameterValue)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.LogEvent(eventName, parameterName, parameterValue);
            }
        }

        public void LogEvent(string eventName, string parameterName, double parameterValue)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.LogEvent(eventName, parameterName, parameterValue);
            }
        }

        public void LogEvent(string eventName, string parameterName, long parameterValue)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.LogEvent(eventName, parameterName, parameterValue);
            }
        }

        public void LogEvent(string eventName, string parameterName, int parameterValue)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.LogEvent(eventName, parameterName, parameterValue);
            }
        }

        public void LogEvent(string eventName)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.LogEvent(eventName);
            }
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.LogEvent(eventName, parameters);
            }
        }

        public void SetUserProperty(string propertyName, string value)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.SetUserProperty(propertyName, value);
            }
        }

        public void SetConsent(bool granted)
        {
            foreach (AnalyticsProvider analyticsProvider in SelectProvidersByBehaviour<AnalyticsProvider>())
            {
                analyticsProvider.SetConsent(granted);
            }
        }
    }
}