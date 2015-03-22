using System;
using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace Terraforming {
	public class TerraformingMain : ThreadingExtensionBase {
		public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
			if (Input.GetKey (KeyCode.PageUp)) {
				TerrainTool terrain = Tools.GetTerrainTool ();
				if (terrain != null && terrain.enabled) {
					float newSize = terrain.m_brushSize + realTimeDelta * terrain.m_brushSize;
					terrain.m_brushSize = Math.Min (2000, newSize);
				}

				WaterTool water = Tools.GetWaterTool ();
				if (water != null && water.enabled) {
					float newSize = water.m_capacity + realTimeDelta * water.m_capacity;
					water.m_capacity = Math.Min (1, newSize);
				}
			}

			if (Input.GetKey (KeyCode.PageDown)) {
				TerrainTool terrain = Tools.GetTerrainTool ();
				if (terrain != null && terrain.enabled) {
					float newSize = terrain.m_brushSize - realTimeDelta * terrain.m_brushSize;
					terrain.m_brushSize = Math.Max (1, newSize);
				}

				WaterTool water = Tools.GetWaterTool ();
				if (water != null && water.enabled) {
					float newSize = water.m_capacity - realTimeDelta * water.m_capacity;
					water.m_capacity = Math.Max (0.001f, newSize);
				}
			}
		}
	}
}

