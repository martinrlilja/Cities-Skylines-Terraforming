using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace Terraforming {
	public class TerraformingMod : IUserMod {
		public string Name {
			get { return "Terraforming"; }
		}

		public string Description {
			get { return "Enables the terrain tools in the game."; }
		}
	}
}
