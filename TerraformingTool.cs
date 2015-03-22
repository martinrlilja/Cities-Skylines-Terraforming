using System;
using System.Reflection;
using UnityEngine;
using ColossalFramework;
using System.Threading;

namespace Terraforming {
	public class TerraformingTool : TerrainTool {
		private static FieldInfo  f_strokeEnded      = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_strokeEnded");
		private static FieldInfo  f_strokeInProgress = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_strokeInProgress");
		private static FieldInfo  f_undoRequest      = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_undoRequest");
		private static FieldInfo  f_mouseRay         = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_mouseRay");
		private static FieldInfo  f_mouseRayLength   = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_mouseRayLength");
		private static FieldInfo  f_mouseRayValid    = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_mouseRayValid");
		private static FieldInfo  f_mousePosition    = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_mousePosition");
		private static FieldInfo  f_mouseLeftDown    = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_mouseLeftDown");
		private static FieldInfo  f_mouseRightDown   = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_mouseRightDown");
		private static FieldInfo  f_strokeXmin       = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_strokeXmin");
		private static FieldInfo  f_strokeXmax       = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_strokeXmax");
		private static FieldInfo  f_strokeZmin       = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_strokeZmin");
		private static FieldInfo  f_strokeZmax       = ReflectionUtils.GetInstanceField (typeof (TerrainTool), "m_strokeZmax");

		private static MethodInfo m_ApplyBrush       = ReflectionUtils.GetInstanceMethod (typeof (TerrainTool), "ApplyBrush");
		private static MethodInfo m_EndStroke        = ReflectionUtils.GetInstanceMethod (typeof (TerrainTool), "EndStroke");

		public int m_costMultiplier = 2000;

		private ushort[] m_buffer;

		public int StrokeXMin {
			get {
				return (int)f_strokeXmin.GetValue (this);
			}
			set {
				f_strokeXmin.SetValue (this, value);
			}
		}
		
		public int StrokeXMax {
			get {
				return (int)f_strokeXmax.GetValue (this);
			}
			set {
				f_strokeXmax.SetValue (this, value);
			}
		}

		public int StrokeZMin {
			get {
				return (int)f_strokeZmin.GetValue (this);
			}
			set {
				f_strokeZmin.SetValue (this, value);
			}
		}

		public int StrokeZMax {
			get {
				return (int)f_strokeZmax.GetValue (this);
			}
			set {
				f_strokeZmax.SetValue (this, value);
			}
		}

		public bool StrokeEnded {
			get {
				return (bool)f_strokeEnded.GetValue (this);
			}
			set {
				f_strokeEnded.SetValue (this, value);
			}
		}

		public bool StrokeInProgress {
			get {
				return (bool)f_strokeInProgress.GetValue (this);
			}
			set {
				f_strokeInProgress.SetValue (this, value);
			}
		}

		public bool UndoRequest {
			get {
				return (bool)f_undoRequest.GetValue (this);
			}
			set {
				f_undoRequest.SetValue (this, value);
			}
		}

		public bool MouseRayValid {
			get {
				return (bool)f_mouseRayValid.GetValue (this);
			}
			set {
				f_mouseRayValid.SetValue (this, value);
			}
		}

		public bool MouseLeftDown {
			get {
				return (bool)f_mouseLeftDown.GetValue (this);
			}
			set {
				f_mouseLeftDown.SetValue (this, value);
			}
		}
		
		public bool MouseRightDown {
			get {
				return (bool)f_mouseRightDown.GetValue (this);
			}
			set {
				f_mouseRightDown.SetValue (this, value);
			}
		}

		public Ray MouseRay {
			get {
				return (Ray)f_mouseRay.GetValue (this);
			}
			set {
				f_mouseRay.SetValue (this, value);
			}
		}

		public float MouseRayLength {
			get {
				return (float)f_mouseRayLength.GetValue (this);
			}
			set {
				f_mouseRayLength.SetValue (this, value);
			}
		}

		public Vector3 MousePosition {
			get {
				return (Vector3)f_mousePosition.GetValue (this);
			}
			set {
				f_mousePosition.SetValue (this, value);
			}
		}

		public void ApplyBrush () {
			ReflectionUtils.InvokeInstanceMethod (m_ApplyBrush, this);
		}

		public void EndStroke () {
			ReflectionUtils.InvokeInstanceMethod (m_EndStroke, this);
		}

		public override void SimulationStep () {
			ToolBase.RaycastInput input = new ToolBase.RaycastInput (this.MouseRay, this.MouseRayLength);
			if (this.UndoRequest && !this.StrokeInProgress) {
				base.ApplyUndo ();
				this.UndoRequest = false;
			} else {
				if (this.StrokeEnded) {
					this.EndStroke ();
					this.StrokeEnded = false;
					this.StrokeInProgress = false;
				} else {
					ToolBase.RaycastOutput raycastOutput;
					if (this.MouseRayValid && ToolBase.RayCast (input, out raycastOutput)) {
						this.MousePosition = raycastOutput.m_hitPos;
						if (this.MouseLeftDown != this.MouseRightDown) {
							if (!this.StrokeInProgress) {
								this.ResetBuffer ();
							} else {
								this.UpdateBuffer ();
							}
							
							this.StrokeInProgress = true;
							this.ApplyBrush ();
							int cost = this.DiffBuffer () * this.m_costMultiplier;
							if (cost != Singleton<EconomyManager>.instance.PeekResource (EconomyManager.Resource.Construction, cost)) {
								this.RevertToBuffer ();
							} else {
								Singleton<EconomyManager>.instance.FetchResource (EconomyManager.Resource.Construction, cost,
								                                                  ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
							}
						}
					}
				}
			}
		}

		private void ResetBuffer () {
			ushort[] heights = Singleton<TerrainManager>.instance.RawHeights;
			if (this.m_buffer == null) {
				this.m_buffer = new ushort[heights.Length];
			}
			Array.Copy (heights, this.m_buffer, this.m_buffer.Length);
		}

		private void UpdateBuffer () {
			ushort[] heights = Singleton<TerrainManager>.instance.RawHeights;
			for (int i = this.StrokeZMin; i < this.StrokeZMax; ++i) {
				for (int j = this.StrokeXMin; j < this.StrokeXMax; ++j) {
					int index = i * 1081 + j;
					this.m_buffer [index] = heights [index];
				}
			}
		}

		private int DiffBuffer () {
			int diff = 0;
			ushort[] heights = Singleton<TerrainManager>.instance.RawHeights;
			for (int i = this.StrokeZMin; i < this.StrokeZMax; ++i) {
				for (int j = this.StrokeXMin; j < this.StrokeXMax; ++j) {
					int index = i * 1081 + j;
					diff += Math.Abs (this.m_buffer [index] - heights [index]);
				}
			}
			return diff;
		}

		private void RevertToBuffer () {
			ushort[] heights = Singleton<TerrainManager>.instance.RawHeights;
			for (int i = this.StrokeZMin; i < this.StrokeZMax; ++i) {
				for (int j = this.StrokeXMin; j < this.StrokeXMax; ++j) {
					int index = i * 1081 + j;
					heights [index] = this.m_buffer [index];
				}
			}
			TerrainModify.UpdateArea (this.StrokeXMin, this.StrokeZMin, this.StrokeXMax, this.StrokeZMax, true, false, false);
		}
	}
}

