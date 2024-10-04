using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership
{
    public enum ProviderBehaviour
    {
        FirstAvailable,
        Waterfall,
        Broadcast,
    }
    
    public abstract class MothershipModule : InitalizeableScriptableObject
    {
        public ProviderBehaviour ProviderBehaviour;
        public BaseProvider[] Providers;
        //protected int lastProviderWaterfallIndex = 0;
        
        public override async Task Initialize()
        {
            if (IsInitialized) return;
            if (!Enabled) return;
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{GetType()}:{nameof(Initialize)}] Initializing ....");
            
            foreach (BaseProvider provider in Providers)
            {
                if(provider.IsInitialized) continue;
                if(!provider.Enabled) continue;
                
                Task initTask = provider.Initialize(); //Should have a CancellationToken ?
                
                if (await Task.WhenAny(initTask, Task.Delay(provider.TimeOut * 1000)) != initTask)
                {
                    Debug.LogError($"[{GetType()}:{nameof(Initialize)}] {provider.GetType()} has timed out while initializing.");
                    continue;
                }
                
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                    Debug.Log($"[{GetType()}:{nameof(Initialize)}] {provider.GetType()} Initialized.");
            }

            IsInitialized = true;
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{GetType()}:{nameof(Initialize)}] Initialized.");
        }

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

        public override void Dispose()
        {
            foreach (BaseProvider provider in Providers)
            {
                if(provider.IsInitialized && provider.Enabled)
                    provider.Dispose();
            }
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{GetType()}:{nameof(Dispose)}]");
        }

        public T[] SelectProvidersByBehaviour<T>()
            where T : BaseProvider
        {
            switch (ProviderBehaviour)
            {
                case ProviderBehaviour.FirstAvailable:
                {
                    BaseProvider found = Array.Find(Providers, item => item.IsInitialized && item.Enabled);
                    return found == null ? Array.Empty<T>() : new T[] { found as T };
                }
                case ProviderBehaviour.Waterfall:
                {
                    //TODO
                    break;
                }
                case ProviderBehaviour.Broadcast:
                {
                    List<T> providersT = new List<T>(Providers.Length);
                    for (int i = 0; i < Providers.Length; i++)
                    {
                        if(!providersT[i].IsInitialized || !providersT[i].Enabled) continue;
                        providersT.Add(Providers[i] as T);
                    }
                    return providersT.ToArray();
                }
            }

            return Array.Empty<T>();
        }
        
        public T GetProviderOfType<T>()
            where T : BaseProvider
        {
            foreach (BaseProvider provider in Providers)
            {
                if (provider is T requestedProvider) return requestedProvider;
            }
            return null;
        }
        
        public BaseProvider GetProviderByName<T>(string providerName)
        {
            return Array.Find(Providers, item => item.name == providerName);
        }
    }
}