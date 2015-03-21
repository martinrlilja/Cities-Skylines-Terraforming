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
				if (terrain != null) {
					float newSize = terrain.m_brushSize + realTimeDelta * terrain.m_brushSize;
					terrain.m_brushSize = Math.Min (2000, newSize);
				}

				WaterTool water = Tools.GetWaterTool ();
				if (water != null) {
					float newSize = water.m_capacity + realTimeDelta * water.m_capacity;
					water.m_capacity = Math.Min (100, newSize);
				}
			}

			if (Input.GetKey (KeyCode.PageDown)) {
				TerrainTool terrain = Tools.GetTerrainTool ();
				if (terrain != null) {
					float newSize = terrain.m_brushSize - realTimeDelta * terrain.m_brushSize;
					terrain.m_brushSize = Math.Max (1, newSize);
				}

				WaterTool water = Tools.GetWaterTool ();
				if (water != null) {
					float newSize = water.m_capacity - realTimeDelta * water.m_capacity;
					water.m_capacity = Math.Max (0.01f, newSize);
				}
			}

			/*if (Input.GetKeyDown (KeyCode.T)) {
				try {
					WaterTool tool = Tools.GetWaterTool ();
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, "m_levelMaterial: " + tool.m_levelMaterial.name);
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, "m_levelMaterial.shader: " + tool.m_levelMaterial.shader.name);
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, "m_sourceMaterial: " + tool.m_sourceMaterial.name);
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, "m_sourceMaterial.shader: " + tool.m_sourceMaterial.shader.name);
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, "m_sourceMesh: " + tool.m_sourceMesh.name);

					// Try to load
					Shader shader = ResourceUtils.Load<Shader> (tool.m_sourceMaterial.shader.name);
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, shader.ToString ());

					Mesh mesh = ResourceUtils.Load<Mesh> (tool.m_sourceMesh.name);
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, mesh.ToString ());
				} catch (Exception e) {
					DebugOutputPanel.AddMessage (PluginManager.MessageType.Message, e.ToString ());
				}
			}*/
		}
	}
}

