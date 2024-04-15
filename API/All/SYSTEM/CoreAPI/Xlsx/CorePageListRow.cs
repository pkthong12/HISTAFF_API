using System.Dynamic;

namespace API.All.SYSTEM.CoreAPI.Xlsx
{
    public class CorePageListRow: DynamicObject
    {
        private readonly Dictionary<string, object?> _rowInformation;

        public CorePageListRow() {
            _rowInformation = new Dictionary<string, object?>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            var key = binder.Name;
            return _rowInformation.TryGetValue(key, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder,  object? value)
        {
            var key = binder.Name;
            _rowInformation[key] = value;

            return true;
        }

    }
}
