using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Terraforming {
	public class EditorPropsPanel : GeneratedScrollPanel {
		public string m_editorCategory;

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
			DebugOutputPanel.AddMessage (ColossalFramework.Plugins.PluginManager.MessageType.Message, this.m_editorCategory);
			foreach (PrefabInfo info in Resources.FindObjectsOfTypeAll<PrefabInfo> ()) {
				if (info.editorCategory == this.m_editorCategory) {
					list.Add (info);
					DebugOutputPanel.AddMessage (ColossalFramework.Plugins.PluginManager.MessageType.Message, info.name);
				}
			}
			list.Sort (new Comparison<PrefabInfo> (base.ItemsGenericSort));

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
	}
}

