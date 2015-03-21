using System;
using System.Runtime.CompilerServices;
using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace Terraforming {
	public class TerraformingPanel : GeneratedScrollPanel {
		private static readonly PositionData<TerrainTool.Mode>[] kTools = Utils.GetOrderedEnumData<TerrainTool.Mode> ();

		public override ItemClass.Service service {
			get {
				return ItemClass.Service.None;
			}
		}

		public override void RefreshPanel () {
			base.RefreshPanel ();
			for (int i = 0; i < TerraformingPanel.kTools.Length; i++) {
				this.SpawnEntry (TerraformingPanel.kTools [i].enumName, i);
			}
		}

		private UIButton SpawnEntry (string name, int index) {
			string tooltip = TooltipHelper.Format (new string[] {
				"title",
				Locale.Get ("TERRAIN_TITLE", name),
				"sprite",
				name,
				"text",
				Locale.Get ("TERRAIN_DESC", name)
			});

			string baseIconName = "Terrain" + name;
			UITextureAtlas atlas = AtlasCreator.CreateAtlas (new string[] {
				baseIconName,
				baseIconName + "Focused",
				baseIconName + "Hovered",
				baseIconName + "Pressed",
				baseIconName + "Disabled"
			});

			return base.CreateButton (name, tooltip, baseIconName, index, atlas, GeneratedPanel.tooltipBox, true);
		}

		protected override void OnButtonClicked (UIComponent comp) {
			int zOrder = comp.zOrder;

			TerrainTool tool = TerraformingTool.GetTerrainTool ();
			tool.m_mode = kTools [zOrder].enumValue;

			ToolsModifierControl.toolController.CurrentTool = tool;
		}
		
		protected override void Start () {
			base.Start ();
			base.component.eventVisibilityChanged += delegate (UIComponent sender, bool visible) { };
		}
	}
}

