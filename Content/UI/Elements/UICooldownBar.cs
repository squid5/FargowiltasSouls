using FargowiltasSouls.Core;
using FargowiltasSouls.Core.Systems;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.Elements
{
    public class UICooldownBar : UIPanel
    {
        public string NameKey;
        public Texture2D BarTexture;
        public Texture2D FillTexture;
        public Texture2D ItemTexture;
        public Color FillColor;
        public Func<float> FillRatio;
        public float Opacity = 0f;

        public UICooldownBar(string nameKey, Texture2D barTexture, Texture2D fillTexture, Texture2D itemTexture, Color fillColor, Func<float> fillRatio)
        {
            NameKey = nameKey;
            BarTexture = barTexture;
            FillTexture = fillTexture;
            ItemTexture = itemTexture;
            FillColor = fillColor;
            FillRatio = fillRatio;

            Width.Set(100, 0);
            Height.Set(20, 0);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float fillRatio = FillRatio.Invoke();
            fillRatio = MathHelper.Clamp(fillRatio, 0f, 1f);
            if (fillRatio <= 0 || fillRatio >= 1)
                Opacity -= 0.05f;
            else
                Opacity += 0.1f;
            Opacity = MathHelper.Clamp(Opacity, -1, 1f);
            float opacity = MathHelper.Clamp(Opacity, 0, 1f);
            //base.DrawSelf(spriteBatch);
            //float fillRatio = FillRatio.Invoke();
            fillRatio = MathHelper.Clamp(fillRatio, 0f, 1f);

            CalculatedStyle style = GetDimensions();

            // Drawing
            Vector2 position = style.Position();
            // bar
            spriteBatch.Draw(BarTexture, position, BarTexture.Bounds, Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

            // fill
            Rectangle fillRectangle = BarTexture.Bounds;
            float rectangleMin = (8f / 84f);
            float rectangleFill = rectangleMin + fillRatio * (1 - 2 * rectangleMin);
            fillRectangle.Width = (int)(fillRectangle.Width * rectangleFill);
            spriteBatch.Draw(FillTexture, position, fillRectangle, FillColor * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

            // item
            float itemScale = MathHelper.Lerp(1f, 34f / ItemTexture.Width, 0.75f);
            spriteBatch.Draw(ItemTexture, position - Vector2.UnitX * style.Width / 4 - Vector2.UnitY * style.Height / 2, ItemTexture.Bounds, Color.White * opacity, 0f, Vector2.Zero, itemScale, SpriteEffects.None, 0);
        }
    }
}
