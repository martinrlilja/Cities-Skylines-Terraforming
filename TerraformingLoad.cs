using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace Terraforming {
	public class TerraformingLoad : LoadingExtensionBase {
		public override void OnLevelLoaded (LoadMode mode) {
			base.OnLevelLoaded (mode);
			
			UITabstrip tabstrip = (UITabstrip)ToolsModifierControl.mainToolbar.component;
			UITabstrip beautificationTabstrip = this.BeautificationTabstrip (tabstrip);
			if (beautificationTabstrip != null) {
				this.AddButton (beautificationTabstrip, true);
			}
		}

		private UIButton[] AllButtons (UITabstrip s) {
			List<UIButton> buttons = new List<UIButton> ();
			foreach (UIComponent component in s.components) {
				if (component != null && component is UIButton) {
					UIButton button = (UIButton)component;
					buttons.Add (button);
				}
			}
			return buttons.ToArray ();
		}

		private UIButton FindButton (UITabstrip s, string name) {
			UIButton[] buttons = this.AllButtons (s);
			return Array.Find (buttons, b => b.name == name);
		}

		private bool IsCreated (UITabstrip strip) {
			UIButton button = this.FindButton (strip, "Terrain");
			return button != null;
		}

		private UITabstrip BeautificationTabstrip (UITabstrip s) {
			UIButton button = this.FindButton (s, "Beautification");
			if (button == null) {
				DebugOutputPanel.AddMessage (PluginManager.MessageType.Error, "Could not find button.");
				return null;
			}

			Type groupPanelType = typeof (BeautificationGroupPanel);
			GeneratedGroupPanel groupPanel = (GeneratedGroupPanel)s.GetComponentInContainer (button, groupPanelType);

			if (groupPanel == null) {
				DebugOutputPanel.AddMessage (PluginManager.MessageType.Error, "Could not find groupPanel.");
				return null;
			}

			UITabstrip strip = groupPanel.Find<UITabstrip> ("GroupToolstrip");
			if (strip == null) {
				DebugOutputPanel.AddMessage (PluginManager.MessageType.Error, "Could not find UITabstrip.");
				return null;
			}
			return strip;
		}

		private UIButton AddButton (UITabstrip strip, bool enabled) {
			Type type = typeof (TerraformingPanel);

			GameObject subbarButtonTemplate = UITemplateManager.GetAsGameObject ("SubbarButtonTemplate");
			GameObject subbarPanelTemplate = UITemplateManager.GetAsGameObject ("SubbarPanelTemplate");
			UIButton button = (UIButton)strip.AddTab ("TerrainDefault", subbarButtonTemplate, subbarPanelTemplate, type);
			button.isEnabled = enabled;

			GeneratedScrollPanel generatedScrollPanel = (GeneratedScrollPanel)strip.GetComponentInContainer (button, type);
			if (generatedScrollPanel != null) {
				generatedScrollPanel.component.isInteractive = true;
				generatedScrollPanel.m_OptionsBar = ToolsModifierControl.mainToolbar.m_OptionsBar;
				generatedScrollPanel.m_DefaultInfoTooltipAtlas = ToolsModifierControl.mainToolbar.m_DefaultInfoTooltipAtlas;

				if (enabled) {
					generatedScrollPanel.RefreshPanel ();
				}
			}

			this.SetButtonSprites (button, "ToolbarIconTerrain", "SubBarButtonBase");
			button.tooltip = "Terraforming";
			return button;
		}

		private void SetButtonSprites (UIButton button, string backgroundSpriteBase, string foregroundSpriteBase) {
			this.SetButtonSprites (button, foregroundSpriteBase, backgroundSpriteBase, "", "");
		}

		private void SetButtonSprites (UIButton button, string backgroundSpriteBase, string foregroundSpriteBase, string normalBgPostfix) {
			this.SetButtonSprites (button, foregroundSpriteBase, backgroundSpriteBase, normalBgPostfix, "");
		}

		private void SetButtonSprites (UIButton button, string backgroundSpriteBase, string foregroundSpriteBase, string normalBgPostfix, string normalFgPostfix) {
			button.atlas = AtlasCreator.CreateAtlas (new string[] {
				backgroundSpriteBase + normalBgPostfix,
				backgroundSpriteBase + "Focused",
				backgroundSpriteBase + "Hovered",
				backgroundSpriteBase + "Pressed",
				backgroundSpriteBase + "Disabled",
				foregroundSpriteBase + normalFgPostfix,
				foregroundSpriteBase + "Focused",
				foregroundSpriteBase + "Hovered",
				foregroundSpriteBase + "Pressed",
				foregroundSpriteBase + "Disabled"
			});

			button.normalBgSprite   = backgroundSpriteBase + normalBgPostfix;
			button.focusedBgSprite  = backgroundSpriteBase + "Focused";
			button.hoveredBgSprite  = backgroundSpriteBase + "Hovered";
			button.pressedBgSprite  = backgroundSpriteBase + "Pressed";
			button.disabledBgSprite = backgroundSpriteBase + "Disabled";

			button.normalFgSprite   = foregroundSpriteBase + normalFgPostfix;
			button.focusedFgSprite  = foregroundSpriteBase + "Focused";
			button.hoveredFgSprite  = foregroundSpriteBase + "Hovered";
			button.pressedFgSprite  = foregroundSpriteBase + "Pressed";
			button.disabledFgSprite = foregroundSpriteBase + "Disabled";
		}
	}
}

