using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Terraforming {
	public class EditorPropsPanel : GeneratedScrollPanel {
		public string[] m_editorCategories;

		public override ItemClass.Service service {
			get {
				return ItemClass.Service.None;
			}
		}

		protected override void OnButtonClicked (UIComponent comp) {
			object objectUserData = comp.objectUserData;
			PropInfo propInfo = objectUserData as PropInfo;
			if (propInfo != null)
			{
				PropTool propTool = ToolsModifierControl.SetTool<PropTool> ();
				if (propTool != null)
				{
					propTool.m_prefab = propInfo;
				}
			}
		}

		public override void RefreshPanel () {
			base.RefreshPanel ();

			List<PrefabInfo> list = new List<PrefabInfo> ();
			foreach (PrefabInfo info in Resources.FindObjectsOfTypeAll<PrefabInfo> ()) {
				if (Array.Exists (this.m_editorCategories, c => c == info.editorCategory)) {
					list.Add (info);
				}
			}
			list.Sort (new Comparison<PrefabInfo> (this.ItemsGenericCategorySort));

			foreach (PrefabInfo info in list) {
				this.CreateAssetItem (info);
			}
		}

		private void CreateAssetItem (PrefabInfo info) {
			string localizedTooltip = info.GetLocalizedTooltip ();
			int hashCode = TooltipHelper.GetHashCode (localizedTooltip);
			UIComponent tooltipBox = GeneratedPanel.GetTooltipBox (hashCode);
			this.SpawnEntry (info.name, localizedTooltip, info.m_Thumbnail, info.m_Atlas, tooltipBox, ToolsModifierControl.IsUnlocked (info.GetUnlockMilestone ())).objectUserData = info;
		}

		protected int ItemsGenericCategorySort (PrefabInfo a, PrefabInfo b) {
			if (a.editorCategory == b.editorCategory) {
				return a.m_UIPriority.CompareTo (b.m_UIPriority);
			} else {
				int aID = Array.FindIndex (this.m_editorCategories, c => c == a.editorCategory);
				int bID = Array.FindIndex (this.m_editorCategories, c => c == b.editorCategory);
				return aID - bID;
			}
		}
	}
}

