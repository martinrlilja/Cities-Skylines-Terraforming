using System;
using UnityEngine;

namespace Terraforming {
	public static class Tools {
		public static TerrainTool GetTerrainTool () {
			TerraformingTool tool = ToolsModifierControl.toolController.gameObject.GetComponent<TerraformingTool> ();
			if (tool != null) {
				return tool;
			}

			tool = ToolsModifierControl.toolController.gameObject.AddComponent<TerraformingTool> ();
			tool.m_brushSize = 100f;
			tool.m_strength = 0.01f;
			tool.m_brush = ToolsModifierControl.toolController.m_brushes [0];
			return tool;
		}

		public static WaterTool GetWaterTool () {
			WaterToolLoad tool = ToolsModifierControl.toolController.gameObject.GetComponent<WaterToolLoad> ();
			if (tool != null) {
				return tool;
			}

			tool = ToolsModifierControl.toolController.gameObject.AddComponent<WaterToolLoad> ();
			tool.m_capacity = 0.01f;
			return tool;
		}

		public static void Load () {
			ToolBase tool = ToolsModifierControl.toolController.CurrentTool;
			Tools.GetTerrainTool ();
			Tools.GetWaterTool ();
			ToolsModifierControl.toolController.CurrentTool = tool;
		}
	}
}

