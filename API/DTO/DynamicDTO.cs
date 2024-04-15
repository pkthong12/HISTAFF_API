using API.Main;
using System.Dynamic;

namespace API.DTO
{
    public class DynamicDTO : DynamicObject
    {
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        // Allow getting the value of a dynamic property
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string propertyName = binder.Name;
            return properties.TryGetValue(propertyName, out result);
        }

        // Allow setting the value of a dynamic property
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string propertyName = binder.Name;
            properties[propertyName] = value;
            return true;
        }

        // Indexer property
        public object this[string propertyName]
        {
            get => properties[propertyName];
            set => properties[propertyName] = value;
        }
    }
}
