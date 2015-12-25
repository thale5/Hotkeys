using ColossalFramework.UI;
using System.Text;

namespace Hotkeys
{
    /// <summary>
    /// Represents an UI component in the component hierarchy that can be found later using this information.
    /// </summary>
    public class Click
    {
        static string DELIM = " | ";

        public readonly string type;        // UIComponent type
        public readonly string desc;        // derived from component name or tooltip
        public readonly int nesting;        // component nesting depth within the hierarchy
        public readonly int orderNumber;    // component order number within its siblings
        public UIComponent component;       // if known

        public Click(UIComponent c, int nesting, int orderNumber)
        {
            this.type = c.GetType().Name;
            this.desc = c.GetDesc();
            this.nesting = nesting;
            this.orderNumber = orderNumber;
            this.component = c;
        }

        public override string ToString()
        {
            return new StringBuilder(64).Append(type).Append(DELIM).Append(desc).Append(DELIM).Append(nesting).Append(DELIM).Append(orderNumber).ToString();
        }
    }
}
