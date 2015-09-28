using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using UnityEngine;

namespace MoreBeautification
{
    public class EditorPropsPanel : GeneratedScrollPanel
    {
        private static MethodInfo m_CreateAssetItem = ReflectionUtils.GetInstanceMethod(typeof(GeneratedScrollPanel), "CreateAssetItem");

        public string[] m_editorCategories;

        public void CreateAssetItem(PrefabInfo info)
        {
            ReflectionUtils.InvokeInstanceMethod(m_CreateAssetItem, this, info);
        }

        public override ItemClass.Service service
        {
            get
            {
                return ItemClass.Service.None;
            }
        }

        protected override void OnButtonClicked(UIComponent comp)
        {
            object objectUserData = comp.objectUserData;
            PropInfo propInfo = objectUserData as PropInfo;
            if (propInfo != null)
            {
                PropTool propTool = ToolsModifierControl.SetTool<PropTool>();
                if (propTool != null)
                {
                    propTool.m_prefab = propInfo;
                }
            }
        }

        public override void RefreshPanel()
        {
            base.RefreshPanel();

            var list = Resources.FindObjectsOfTypeAll<PropInfo>().Where(info => info.m_mesh != null).
                Where(info => Array.Exists(this.m_editorCategories, c =>
            {
                if (c != "PropsCommonStreets" && c != PrefabInfo.kDefaultCategory && c != PrefabInfo.kSameAsGameCategory)
                    return c == info.editorCategory;
                switch (category)
                {
                    case "PropsLights":
                        return info.m_effects != null && info.m_effects.Any(effect => effect.m_effect is LightEffect);
                    case "PropsCommon":
                    case "PropsUnsorted":
                        if (info.m_effects != null)
                        {
                            if (info.m_effects.Any(effect => effect.m_effect is LightEffect))
                            {
                                return false;
                            }
                        }
                        break;
                }
                return c == info.editorCategory;
            })).ToList();
            list.Sort(new Comparison<PropInfo>(this.ItemsGenericCategorySort));

            foreach (PropInfo info in list)
            {
                this.CreateAssetItem(info);
            }
        }

        protected int ItemsGenericCategorySort(PrefabInfo a, PrefabInfo b)
        {
            if (a.editorCategory == b.editorCategory)
            {
                return a.m_UIPriority.CompareTo(b.m_UIPriority);
            }
            else
            {
                int aID = Array.FindIndex(this.m_editorCategories, c => c == a.editorCategory);
                int bID = Array.FindIndex(this.m_editorCategories, c => c == b.editorCategory);
                return aID - bID;
            }
        }
    }
}

