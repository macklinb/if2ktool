using System.Linq;
using System.Xml.Linq;

namespace if2ktool
{
    // A few helper methods for parsing plists (using .NET XElement etc)
    public static class PlistHelper
    {
        // Checks if a key is present under the XElement dict
        // Note that this doesn't check existance of a value for that key!
        public static bool IsKeyInDict(XElement dict, string keyName)
        {
            if (dict == null || dict.IsEmpty)
                return false;

            return dict.Descendants("key").FirstOrDefault(x => x.Value == keyName) != null;
        }

        // Assuming plist structure, returns the value of the key in the dict following <key>keyName</key>
        public static string GetValueOfKeyInDict(XElement dict, string keyName)
        {
            if (dict == null || dict.IsEmpty)
                return null;

            var key = dict.Descendants("key").FirstOrDefault(x => x.Value == keyName);
            return GetValueOfKey(key);
        }

        // Assuming plist structure, returns the value of the element immediately following the key
        // For example <key>Some Key</key><integer>1234</integer> will return 1234
        public static string GetValueOfKey(XElement key)
        {
            if (key != null && key.NextNode != null)
            {
                var xelement = (XElement)key.NextNode;
                return string.IsNullOrEmpty(xelement.Value) ? xelement.Name.LocalName : xelement.Value;
            }
            else
                return null;
        }
    }
}
