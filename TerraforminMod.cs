using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace Terraforming {
	public class TerraformingMod : IUserMod {
		public string Name {
			get { return "WIP More Beautificatio"; }
		}

		public string Description {
			get { return "Enables terraforming, water source placement and park decoration placement in game."; }
		}
	}
}
