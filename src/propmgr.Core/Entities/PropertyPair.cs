using System;

namespace propmgr.Core.Entities
{
    public class PropertyPair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsComment() => Key == "#" || Key == "!";

        public bool IsEmptyLine() => Key == "" && Value == "";
    }
}
