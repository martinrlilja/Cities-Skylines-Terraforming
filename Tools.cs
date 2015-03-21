using System;

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
	}
}

