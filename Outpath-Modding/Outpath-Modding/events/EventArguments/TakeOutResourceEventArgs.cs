using System;

namespace Outpath_Modding.Events.EventArguments
{
    public class TakeOutResourceEventArgs : EventArgs
    {
        public TakeOutResourceEventArgs(TakeOutResource resource, float damage)
        {
            Rresource = resource;
            Damage = damage;
        }

        public TakeOutResource Rresource { get; }
        public float Damage { get; set; }
        public bool IsAllowed { get; set; } = true;
    }
}
