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

			ResourceUtils.Dump<CursorInfo> ();
			Tools.Load ();
			
			UITabstrip tabstrip = (UITabstrip)ToolsModifierControl.mainToolbar.component;
			UITabstrip beautificationTabstrip = this.BeautificationTabstrip (tabstrip);

			if (beautificationTabstrip != null && !this.IsCreated (beautificationTabstrip)) {
				this.AddPropsPanels (beautificationTabstrip, new EditorProps[] {
					new EditorProps ("PropsBillboards", new string[] {
						"PropsBillboardsLogo",
						"PropsBillboardsSmallBillboard",
						"PropsBillboardsMediumBillboard",
						"PropsBillboardsLargeBillboard",
					}, "ToolbarIconPropsBillboards", "Billboards"),

					new EditorProps ("PropsSpecialBillboards", new string[] {
						"PropsBillboardsRandomLogo",
						"PropsSpecialBillboardsRandomSmallBillboard",
						"PropsSpecialBillboardsRandomMediumBillboard",
						"PropsSpecialBillboardsRandomLargeBillboard",
						"PropsSpecialBillboards3DBillboard",
						"PropsSpecialBillboardsAnimatedBillboard",
					}, "ToolbarIconPropsSpecialBillboards", "Special Billboards"),

					new EditorProps ("PropsIndustrial", new string[] {
						"PropsIndustrialContainers",
						"PropsIndustrialConstructionMaterials",
						"PropsIndustrialStructures",
					}, "ToolbarIconPropsIndustrial", "Industrial"),

					new EditorProps ("PropsParks", new string[] {
						"PropsParksPlaygrounds",
						"PropsParksFlowersAndPlants",
						"PropsParksParkEquipment",
						"PropsParksFountains",
					}, "ToolbarIconPropsParks", "Parks"),

					new EditorProps ("PropsCommon", new string[] {
						"PropsCommonAccessories",
						"PropsCommonGarbage",
						"PropsCommonCommunications",
						"PropsCommonStreets",
					}, "ToolbarIconPropsCommon", "Common"),

					new EditorProps ("PropsResidential", new string[] {
						"PropsResidentialHomeYard",
						"PropsResidentialGroundTiles",
						"PropsResidentialRooftopAccess",
						"PropsResidentialRandomRooftopAccess",
					}, "ToolbarIconPropsResidential", "Residential"),
				});

				UIButton button = this.AddButton (typeof (TerraformingPanel), beautificationTabstrip, "TerrainDefault", "Terraforming", true);
				this.SetButtonSprites (button, "ToolbarIconTerrain", "SubBarButtonBase");
			}
			// CreateGroupItem (new GeneratedGroupPanel.GroupInfo ("PropsParksParkEquipment", this.GetCategoryOrder (base.name), "Props"), "PROPS_CATEGORY");
		}

		private struct EditorProps {
			public string   m_category;
			public string[] m_categories;
			public string   m_icon;
			public string   m_tooltip;

			public EditorProps (string category, string[] categories, string icon, string tooltip) {
				this.m_category   = category;
				this.m_categories = categories;
				this.m_icon       = icon;
				this.m_tooltip    = tooltip;
			}
		}

		private void AddPropsPanels (UITabstrip tabstrip, EditorProps[] props) {
			foreach (EditorProps prop in props) {
				UIButton button = this.AddButton (typeof (EditorPropsPanel), tabstrip, prop.m_category, prop.m_categories, prop.m_tooltip, true);
				this.SetButtonSprites (button, prop.m_icon, "SubBarButtonBase");
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
			UIButton button = this.FindButton (strip, "TerrainDefault");
			return button != null;
		}

		private UITabstrip BeautificationTabstrip (UITabstrip s) {
			UIButton button = this.FindButton (s, "Beautification");
			if (button == null) {
				return null;
			}

			Type groupPanelType = typeof (BeautificationGroupPanel);
			GeneratedGroupPanel groupPanel = (GeneratedGroupPanel)s.GetComponentInContainer (button, groupPanelType);

			if (groupPanel == null) {
				return null;
			}

			UITabstrip strip = groupPanel.Find<UITabstrip> ("GroupToolstrip");
			if (strip == null) {
				return null;
			}
			return strip;
		}

		private UIButton AddButton (Type type, UITabstrip strip, string category, string tooltip, bool enabled) {
			return this.AddButton (type, strip, category, null, tooltip, enabled);
		}

		private UIButton AddButton (Type type, UITabstrip strip, string category, string[] editorCategories, string tooltip, bool enabled) {
			GameObject subbarButtonTemplate = UITemplateManager.GetAsGameObject ("SubbarButtonTemplate");
			GameObject subbarPanelTemplate = UITemplateManager.GetAsGameObject ("SubbarPanelTemplate");
			UIButton button = (UIButton)strip.AddTab (category, subbarButtonTemplate, subbarPanelTemplate, type);
			button.isEnabled = enabled;

			GeneratedScrollPanel generatedScrollPanel = (GeneratedScrollPanel)strip.GetComponentInContainer (button, type);
			if (generatedScrollPanel != null) {
				generatedScrollPanel.component.isInteractive = true;
				generatedScrollPanel.m_OptionsBar = ToolsModifierControl.mainToolbar.m_OptionsBar;
				generatedScrollPanel.m_DefaultInfoTooltipAtlas = ToolsModifierControl.mainToolbar.m_DefaultInfoTooltipAtlas;

				if (generatedScrollPanel is EditorPropsPanel) {
					((EditorPropsPanel)generatedScrollPanel).m_editorCategories = editorCategories;
				}

				if (enabled) {
					generatedScrollPanel.RefreshPanel ();
				}
			}

			button.tooltip = tooltip;
			return button;
		}

		private void SetButtonSprites (UIButton button, string backgroundSpriteBase, string foregroundSpriteBase) {
			this.SetButtonSprites (button, foregroundSpriteBase, backgroundSpriteBase, "", "");
		}

		private void SetButtonSprites (UIButton button, string backgroundSpriteBase, string foregroundSpriteBase, string normalBgPostfix) {
			this.SetButtonSprites (button, foregroundSpriteBase, backgroundSpriteBase, normalBgPostfix, "");
		}

		private void SetButtonSprites (UIButton button, string backgroundSpriteBase, string foregroundSpriteBase, string normalBgPostfix, string normalFgPostfix) {
			button.atlas = ResourceUtils.CreateAtlas (new string[] {
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

