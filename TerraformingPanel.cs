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
				this.SpawnEntry ("Terrain", TerraformingPanel.kTools [i].enumName, i);
			}
			this.SpawnEntry ("Water", "PlaceWater", TerraformingPanel.kTools.Length);
		}

		private UIButton SpawnEntry (string type, string name, int index) {
			string tooltip = TooltipHelper.Format (new string[] {
				"title",
				Locale.Get (type.ToUpper () + "_TITLE", name),
				"sprite",
				name,
				"text",
				Locale.Get (type.ToUpper () + "_DESC", name)
			});

			string baseIconName = type + name;
			UITextureAtlas atlas = ResourceUtils.CreateAtlas (new string[] {
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

			if (zOrder == kTools.Length) {
				WaterTool tool = Tools.GetWaterTool ();
				tool.m_mode = WaterTool.Mode.WaterSource;

				ToolsModifierControl.toolController.CurrentTool = tool;
			} else {
				TerrainTool tool = Tools.GetTerrainTool ();
				tool.m_mode = kTools [zOrder].enumValue;

				ToolsModifierControl.toolController.CurrentTool = tool;
			}
		}
		
		protected override void Start () {
			base.Start ();
			base.component.eventVisibilityChanged += delegate (UIComponent sender, bool visible) { };
		}
	}
}

