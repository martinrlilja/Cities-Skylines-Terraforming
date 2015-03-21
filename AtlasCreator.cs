using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace Terraforming {
	public static class AtlasCreator {
		public static UITextureAtlas CreateAtlas (string[] names) {
			List<Texture2D> sprites = new List<Texture2D> ();

			foreach (Texture2D sprite in Resources.FindObjectsOfTypeAll<Texture2D> ()) {
				if (Array.Exists (names, n => n == sprite.name)) {
					sprites.Add (sprite);
				}
			}

			UITextureAtlas atlas = new UITextureAtlas ();
			atlas.material = new Material (AtlasCreator.GetUIAtlasShader ());

			Texture2D texture = new Texture2D (0, 0);
			Rect[] rects = texture.PackTextures (sprites.ToArray (), 0);

			for (int i = 0; i < rects.Length; ++i) {
				Texture2D sprite = sprites [i];
				Rect rect = rects [i];

				UITextureAtlas.SpriteInfo spriteInfo = new UITextureAtlas.SpriteInfo ();
				spriteInfo.name = sprite.name;
				spriteInfo.texture = sprite;
				spriteInfo.region = rect;
				spriteInfo.border = new RectOffset ();

				atlas.AddSprite (spriteInfo);
			}

			atlas.material.mainTexture = texture;
			return atlas;
		}

		private static Shader GetUIAtlasShader () {
			return UIView.GetAView ().defaultAtlas.material.shader;
		}
	}
}
