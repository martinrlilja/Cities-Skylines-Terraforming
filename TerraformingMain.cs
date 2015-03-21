using System;
using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace Terraforming {
	public class TerraformingMain : ThreadingExtensionBase {
		public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
			if (Input.GetKey (KeyCode.PageUp)) {
				TerrainTool tool = TerraformingTool.GetTerrainTool ();
				if (tool != null) {
					float newSize = tool.m_brushSize + realTimeDelta * tool.m_brushSize;
					tool.m_brushSize = Math.Min (2000, newSize);
				}
			}

			if (Input.GetKey (KeyCode.PageDown)) {
				TerrainTool tool = TerraformingTool.GetTerrainTool ();
				if (tool != null) {
					float newSize = tool.m_brushSize - realTimeDelta * tool.m_brushSize;
					tool.m_brushSize = Math.Max (1, newSize);
				}
			}
		}
	}
}

