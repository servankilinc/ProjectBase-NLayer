using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Core.Utils.CriticalData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreMapAttribute : Attribute
    {
    }

    /// <summary>
    /// Json Serilaze Ignore Properties for logs ...
    /// </summary>
    public class IgnoreMapResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);

            return props.Where(p =>
            {
                if (string.IsNullOrEmpty(p.PropertyName)) return false;

                PropertyInfo? propertyInfo = type.GetProperty(p.PropertyName);
                if (propertyInfo == null) return true;

                return !Attribute.IsDefined(propertyInfo, typeof(IgnoreMapAttribute));
            }).ToList();
        }
    }
}
