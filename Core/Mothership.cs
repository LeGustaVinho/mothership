using System;
using System.Threading.Tasks;
using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership
{
    public class Mothership : IDisposable
    {
        public MothershipConfig Config;

        public static MothershipLogLevel LogLevel = MothershipLogLevel.Info;
        
        public Mothership(MothershipConfig config)
        {
            Config = config;
            LogLevel = config.LogLevel;
        }

        public async Task Initialize()
        {
            if(LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(Mothership)}:{nameof(Initialize)}] Initializing ...");
            
            foreach (MothershipModule module in Config.Modules)
            {
                try
                {
                    await module.Initialize();
                }
                catch (Exception ex)
                {
                    if (LogLevel.HasFlags(MothershipLogLevel.Error))
                    {
                        Debug.LogError($"[{nameof(Mothership)}:{nameof(Initialize)}] initialization error on {module.GetType()} module.");
                        Debug.LogException(ex);
                    }
                    continue;
                }
            }
            
            if(LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(Mothership)}:{nameof(Initialize)}] Initialized.");
        }

        public T GetModuleOfType<T>()
            where T : MothershipModule
        {
            foreach (MothershipModule mothershipModule in Config.Modules)
            {
                if (mothershipModule is T requestedModule) return requestedModule;
            }
            return null;
        }
        
        public MothershipModule GetModuleByName<T>(string name)
        {
            return Array.Find(Config.Modules, item => item.name == name);
        }
        
        public void Dispose()
        {
            foreach (MothershipModule module in Config.Modules)
            {
                module.Dispose();
            }
            
            if(LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(Mothership)}:{nameof(Dispose)}].");
        }
    }
}