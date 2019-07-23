using System;

namespace propmgr.Core.Entities
{
    public class PropertyPair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsComment() => throw new NotImplementedException();

        public bool IsEmptyLine() => throw new NotImplementedException();
    }
}
