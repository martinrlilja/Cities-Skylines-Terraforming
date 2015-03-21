using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace Terraforming {
	public class TerraformingGroupPanel : GeneratedGroupPanel {
		protected override bool CustomRefreshPanel () {
			this.AddButton (true);
			return true;
		}

		protected override bool IsServiceValid (PrefabInfo info) {
			return true;
		}

		private UIButton AddButton (bool enabled) {
			Type type = typeof (TerraformingPanel);

			GameObject subbarButtonTemplate = UITemplateManager.GetAsGameObject ("SubbarButtonTemplate");
			GameObject subbarPanelTemplate = UITemplateManager.GetAsGameObject ("SubbarPanelTemplate");
			UIButton button = (UIButton)this.m_Strip.AddTab ("TerrainDefault", subbarButtonTemplate, subbarPanelTemplate, type);
			button.isEnabled = enabled;

			GeneratedScrollPanel generatedScrollPanel = (GeneratedScrollPanel)this.m_Strip.GetComponentInContainer (button, type);
			if (generatedScrollPanel != null) {
				generatedScrollPanel.component.isInteractive = true;
				generatedScrollPanel.m_OptionsBar = ToolsModifierControl.mainToolbar.m_OptionsBar;
				generatedScrollPanel.m_DefaultInfoTooltipAtlas = ToolsModifierControl.mainToolbar.m_DefaultInfoTooltipAtlas;

				if (enabled) {
					generatedScrollPanel.RefreshPanel ();
				}
			}

			string text = "SubBarTerrainDefault";
			button.normalFgSprite = text;
			button.focusedFgSprite = text + "Focused";
			button.hoveredFgSprite = text + "Hovered";
			button.pressedFgSprite = text + "Pressed";
			button.disabledFgSprite = text + "Disabled";

			return button;
		}
	}
}
