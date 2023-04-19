using System.Collections.Generic;
using UnityEngine;

namespace UILogicView
{

    public class UIView: MonoBehaviour
    {
        public string viewName;
    }
    
    public static class UIModule
    {
        private static Dictionary<string, UIView> viewDict;

        public static void Init()
        {
            viewDict = new Dictionary<string, UIView>();
        }
        public static bool AddNewView(UIView view)
        {
            if (viewDict.ContainsKey(view.viewName))
                viewDict[view.viewName] = view;
            else viewDict.Add(view.viewName, view);
            
            return true;
        }

        public static void DestroyPanel(string viewName)
        {
            if (viewDict.TryGetValue(viewName, out var view))
            {
                Object.DestroyImmediate(view.gameObject);
            }
        }
    }
}