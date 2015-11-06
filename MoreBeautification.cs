using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace MoreBeautification
{
    public class MoreBeautification : IUserMod
    {
        public string Name
        {
            get { return "More Beautification"; }
        }

        public string Description
        {
            get { return "Enables props placement in game."; }
        }
    }
}
