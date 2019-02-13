using Eclipse.Base.Struct;
using Eclipse.Components.Command;

namespace Eclipse.Base
{
    public class PluginManagerBase : ManagerBase
    {
        public virtual string GetManagerName() { return null; }
        public virtual CommandBase[] GetCommands() { return null; }
        public virtual ControlKeycodeBase.PluginControl[] GetPluginControl() { return null; }
    }
}
