using System;
using System.Threading.Tasks;

namespace LegedaryTools.Mothership
{
    public interface IInitalizeable : IDisposable
    {
        bool Enabled { get; }
        int TimeOut { get; set; }
        bool IsInitialized { get; }
        
        abstract Task Initialize();
        void OnEnableChange(bool newMode);
    }
}