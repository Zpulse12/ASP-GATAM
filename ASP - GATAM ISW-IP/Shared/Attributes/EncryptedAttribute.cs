using System;

namespace Gatam.Shared.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EncryptedAttribute : Attribute
    {
        public EncryptedAttribute()
        {
        }
    }
}
