using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Hotkeys
{
    /// <summary>
    /// Static UI support methods.
    /// </summary>
    public static class UISupport
    {
        public const int MAX_NESTING = 20;

        static UIComponent hovered;
        static int hoveredNesting;

        public static UIView[] GetUIViews()
        {
            return Resources.FindObjectsOfTypeAll<UIView>();
        }

        /// <summary>
        /// Returns the UI components defined immediately under the UIView.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IList<UIComponent> GetUIComponents(this UIView view)
        {
            if (!(view.cachedTransform == null))
            {
                int n = view.cachedTransform.childCount;
                List<UIComponent> childs = new List<UIComponent>(n);

                for (int i = 0; i < n; i++)
                {
                    UIComponent comp = view.cachedTransform.GetChild(i).GetComponent<UIComponent>();

                    if (!(comp == null))
                        childs.Add(comp);
                }

                return childs;
            }
            else
                return new UIComponent[0];
        }

        /// <summary>
        /// Returns a string descriptor based on component name or tooltip, as available.
        /// </summary>
        public static string GetDesc(this UIComponent comp)
        {
            string ret;
            string name = comp.cachedName;

            if (name != null && name.Length > 0 && !comp.GetType().Name.Contains(name))
                ret = name;
            else
            {
                string tip = comp.tooltip;
                ret = tip != null && tip.Length > 0 ? tip : "";
            }

            if (ret.Length > 32)
                ret = ret.Substring(0, 32);

            return ret;
        }

        /// <summary>
        /// Returns the component order number within its siblings.
        /// </summary>
        public static int GetOrderNumber(this UIComponent comp, IList<UIComponent> siblings)
        {
            string type = comp.GetType().Name, desc = comp.GetDesc();
            int order = 0;

            for (int i = 0; i < siblings.Count; i++)
            {
                UIComponent s = siblings[i];

                if (object.ReferenceEquals(comp, s))
                    return order;

                if (type == s.GetType().Name && desc == s.GetDesc())
                    order++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the UI component over which the mouse is hovering.
        /// </summary>
        public static Click FindHovered()
        {
            hovered = null;

            foreach (UIView view in UISupport.GetUIViews())
            {
                IList<UIComponent> childs = view.GetUIComponents();

                foreach (UIComponent comp in childs)
                    if (comp.isVisible)
                        comp.FindHovered();

                if (!(hovered == null))
                {
                    int orderNumber;

                    if (hoveredNesting == 1)
                        orderNumber = hovered.GetOrderNumber(childs);
                    else
                    {
                        IList<UIComponent> siblings = hovered.parent.components;

                        if (siblings != null && siblings.Count > 0)
                            orderNumber = hovered.GetOrderNumber(siblings);
                        else
                            orderNumber = -1;
                    }

                    return new Click(hovered, hoveredNesting, orderNumber);
                }
            }

            return null;
        }

        static void FindHovered(this UIComponent comp, int nesting = 1)
        {
            if (nesting < MAX_NESTING && comp.isVisibleSelf)
            {
                if (comp.containsMouse)
                {
                    hovered = comp;
                    hoveredNesting = nesting;
                }

                IList<UIComponent> childs = comp.components;
                nesting++;

                if (childs != null)
                    for (int i = 0; i < childs.Count; i++)
                        childs[i].FindHovered(nesting);
            }
        }
    }
}
