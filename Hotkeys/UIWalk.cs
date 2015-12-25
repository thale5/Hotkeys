using ColossalFramework.UI;
using System.Collections.Generic;
using System.IO;

namespace Hotkeys
{
    /// <summary>
    /// A full recursive walk of the UIView component hierarchy for research purposes.
    /// </summary>
    public class UIWalk
    {
        static StreamWriter writer;

        // A recursive walk of all UIViews.
        public static void Walk()
        {
            string s = System.DateTime.Now.ToString().Replace(" ", "_").Replace("/", "-").Replace(":", "-");
            writer = new StreamWriter("UIWalk-" + s + ".txt");

            foreach (UIView view in UISupport.GetUIViews())
                Walk(view);

            writer.Close();
            writer = null;
        }

        static void Walk(UIView view)
        {
            Write(view);
            IList<UIComponent> childs = view.GetUIComponents();

            foreach (UIComponent comp in childs)
                Walk(comp, "  ", childs);
        }

        static void Walk(UIComponent comp, string indent, IList<UIComponent> siblings)
        {
            Write(comp, indent, siblings);

            if (!(comp == null) && indent.Length < 40)
            {
                indent += "  ";
                IList<UIComponent> childs = comp.components;

                if (childs != null)
                    foreach (UIComponent c in childs)
                        Walk(c, indent, childs);
            }
        }

        static void Write(UIView view)
        {
            writer.WriteLine();
            writer.WriteLine("UIView " + view.uiTag);
        }

        static void Write(UIComponent comp, string indent, IList<UIComponent> siblings)
        {
            string s = indent;

            if (comp == null)
                s += "null UIComponent";
            else
            {
                s = Append(s, comp.GetType());
                s = Append(s, comp.GetDesc());
                s = Append(s, comp.isEnabled);
                s = Append(s, comp.isVisible);
                s = Append(s, comp.GetOrderNumber(siblings));
            }

            writer.WriteLine(s);
        }

        static string Append(string s, object o)
        {
            if (o == null)
                s += "null ";
            else
                s += o.ToString() + " ";

            return s;
        }
    }
}
