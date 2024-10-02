using System;
using System.Threading.Tasks;
using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership.Iap
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Modules/IapModule", fileName = "IapModule", order = 0)]
    public class IapModule : MothershipModule
    {
        public IapProvider[] Providers;
        public override async Task Initialize()
        {
            if (IsInitialized) return;
            if (!Enabled) return;
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(IapModule)}:{nameof(Initialize)}] Initializing ....");
            
            foreach (IapProvider provider in Providers)
            {
                if(provider.IsInitialized) continue;
                if(!provider.Enabled) continue;
                Task initTask = provider.Initialize(); //Should have a CancellationToken ?
                
                if (await Task.WhenAny(initTask, Task.Delay(provider.TimeOut * 1000)) != initTask)
                {
                    Debug.LogError($"[{nameof(IapModule)}:{nameof(Initialize)}] {provider.GetType()} has timed out while initializing.");
                    continue;
                }
                
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                    Debug.Log($"[{nameof(IapModule)}:{nameof(Initialize)}] {provider.GetType()} Initialized.");
            }

            IsInitialized = true;
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(IapModule)}:{nameof(Initialize)}] Initialized.");
        }

        public override async void OnEnableChange(bool newMode)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(IapModule)}:{nameof(OnEnableChange)}] state changed to {newMode}");
            
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

        public override void Dispose()
        {
            foreach (IapProvider provider in Providers)
            {
                if(provider.IsInitialized && provider.Enabled)
                    provider.Dispose();
            }
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(IapModule)}:{nameof(Dispose)}]");
        }
    }
}