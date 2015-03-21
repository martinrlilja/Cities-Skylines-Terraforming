using System;
using UnityEngine;

namespace Terraforming {
	public static class Tools {
		public static TerrainTool GetTerrainTool () {
			TerrainTool tool = ToolsModifierControl.toolController.gameObject.GetComponent<TerrainTool> ();
			if (tool != null) {
				return tool;
			}

			tool = ToolsModifierControl.toolController.gameObject.AddComponent<TerrainTool> ();
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
			tool.Load ();
			tool.m_capacity = 0.1f;
			return tool;
		}

		public static void LoadTools () {
			Tools.GetTerrainTool ();
			Tools.GetWaterTool ();
		}
	}
}

