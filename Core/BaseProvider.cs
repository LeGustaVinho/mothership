using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership
{
    public abstract class BaseProvider : InitalizeableScriptableObject
    {
        public override async void OnEnableChange(bool newMode)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{GetType()}:{nameof(OnEnableChange)}] state changed to {newMode}");
            
            if (newMode)
            {
                await Initialize();
            }
            else
            {
                Dispose();
                IsInitialized = false;
            }
        }
    }
}