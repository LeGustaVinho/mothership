#if ENABLE_FIREBASE_ANALYTICS
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;
using LegendaryTools;

namespace LegedaryTools.Mothership.Analytics.Firebase
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Analytics/FirebaseAnalyticsProvider", fileName = "FirebaseAnalyticsProvider", order = 0)]
    public class FirebaseAnalyticsProvider : AnalyticsProvider, IAnalyticsProvider
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.SuffixLabel("minutes")]
#endif
        public int SessionTimeDuration = 30;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
        public UserIdProvider UserIdProvider;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#if UNITY_EDITOR
        [Sirenix.OdinInspector.HorizontalGroup("UserId")]
#endif
#endif
        public override string UserId
        {
            get => UserIdProvider?.Load();
            set
            {
                if(IsInitialized && Enabled) FirebaseAnalytics.SetUserId(value);
                
                UserIdProvider?.Save(value);
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                    Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(UserId)}] User id: {value}");
            }
        }
        
        public override async Task Initialize()
        {
            if (IsInitialized) return;
            if (!Enabled) return;
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(Initialize)}] Initialized.");
            
            DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (status == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.SetSessionTimeoutDuration(TimeSpan.FromMinutes(SessionTimeDuration));
                if(!UserIdProvider.UserIdIsSet) UserId = UserIdProvider.GenerateUserId();
                IsInitialized = true;
            }
            else
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AnalyticsProvider)}:{nameof(Initialize)}] Cannot resolve Firebase dependencies, result: " + status);
            }
        }
        
        public override void LogEvent(string eventName, string parameterName, string parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(LogEvent)}] Name {eventName}, [{parameterName}] => {parameterValue}");
        }

        public override void LogEvent(string eventName, string parameterName, double parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(LogEvent)}] Name {eventName}, [{parameterName}] => {parameterValue}");
        }

        public override void LogEvent(string eventName, string parameterName, long parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(LogEvent)}] Name {eventName}, [{parameterName}] => {parameterValue}");
        }

        public override void LogEvent(string eventName, string parameterName, int parameterValue)
        {
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(LogEvent)}] Name {eventName}, [{parameterName}] => {parameterValue}");
        }

        public override void LogEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(LogEvent)}] Name {eventName}");
        }

        public override void LogEvent(string eventName, Dictionary<string, object> parameters)
        {
            Parameter[] firebaseParameters = new Parameter[parameters.Count];
            int i = 0;
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                firebaseParameters[i] = parameter.Value switch
                {
                    string valueString => new Parameter(parameter.Key, valueString),
                    int valueInt => new Parameter(parameter.Key, valueInt),
                    float valueFloat => new Parameter(parameter.Key, valueFloat),
                    _ => firebaseParameters[i]
                };
                i++;
            }
            FirebaseAnalytics.LogEvent(eventName, firebaseParameters);

            if (Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[{nameof(AnalyticsProvider)}:{nameof(LogEvent)}] Name {eventName}, with params:");
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    sb.AppendLine($"[{parameter.Key}] => {parameter.Value}");
                }
                Debug.Log(sb.ToString());
            }
        }

        public override void SetUserProperty(string propertyName, string value)
        {
            FirebaseAnalytics.SetUserProperty(propertyName, value);
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(AnalyticsProvider)}:{nameof(SetUserProperty)}]{propertyName} => {value}");
        }

        public override void SetConsent(bool granted)
        {
            FirebaseAnalytics.SetConsent(new Dictionary<ConsentType, ConsentStatus>()
            {
                { ConsentType.AdStorage, granted ? ConsentStatus.Granted : ConsentStatus.Denied },
                { ConsentType.AnalyticsStorage, granted ? ConsentStatus.Granted : ConsentStatus.Denied },
                { ConsentType.AdUserData, granted ? ConsentStatus.Granted : ConsentStatus.Denied },
                { ConsentType.AdPersonalization, granted ? ConsentStatus.Granted : ConsentStatus.Denied },
            });
        }

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.HorizontalGroup("UserId", width: 100)]
        [Sirenix.OdinInspector.ShowIf("HasUserId")]
#endif
        private void DeleteUserId()
        {
            UserIdProvider?.Reset();
        }

        private bool HasUserId()
        {
            return UserIdProvider != null && UserIdProvider.UserIdIsSet;
        }
#endif
        
        public override void Dispose()
        {
        }
    }
}
#endif