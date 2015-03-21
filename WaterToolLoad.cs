using System;
using UnityEngine;

namespace Terraforming {
	public class WaterToolLoad : WaterTool {
		public void Load () {
			this.m_levelMaterial = new Material (Shader.Find ("Custom/Overlay/WaterLevel"));
			this.m_sourceMaterial = new Material (Shader.Find ("Custom/Tools/WaterSource"));
			this.m_sourceMesh = ResourceUtils.Load<Mesh> ("Cylinder01");

			this.Awake ();
		}
	}
}

