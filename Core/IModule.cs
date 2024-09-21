using System;
using System.Threading.Tasks;

namespace LegedaryTools.Mothership
{
    public interface IModule : IDisposable
    {
        bool Enabled { get; set; }
        Task Initialize();
    }
}