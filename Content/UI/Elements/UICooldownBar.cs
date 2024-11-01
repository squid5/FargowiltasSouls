using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.Elements
{
    public class UICooldownBar : UIPanel
    {
        public bool Active = true;

        public string NameKey;
        public Texture2D BarTexture;
        public Texture2D FillTexture;
        public Texture2D ItemTexture;
        public Color FillColor;
        public Func<float> FillRatio;
        public bool DisplayAtFull;
        public int FadeDelay;
        public Func<bool> ActiveFunction;
        public int AnimationFrames;

        public float Opacity = 0f;
        public int FadeTimer = 0;
        private int AnimationTimer;

        public UICooldownBar(string nameKey, Texture2D barTexture, Texture2D fillTexture, Texture2D itemTexture, Color fillColor, Func<float> fillRatio, bool displayAtFull, int fadeDelay, Func<bool> activeFunction, int animationFrames)
        {
            Active = true;
            NameKey = nameKey;
            BarTexture = barTexture;
            FillTexture = fillTexture;
            ItemTexture = itemTexture;
            FillColor = fillColor;
            FillRatio = fillRatio;
            DisplayAtFull = displayAtFull;
            FadeDelay = fadeDelay;
            ActiveFunction = activeFunction;
            AnimationFrames = animationFrames;

            Width.Set(CooldownBarManager.BarWidth, 0);
            Height.Set(CooldownBarManager.BarHeight, 0);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float fillRatio = FillRatio.Invoke();
            fillRatio = MathHelper.Clamp(fillRatio, 0f, 1f);
            if (!Active || fillRatio <= 0 || (!DisplayAtFull && fillRatio >= 1) || !ActiveFunction.Invoke())
            {
                if (++FadeTimer > FadeDelay || !ActiveFunction.Invoke())
                    Opacity -= 0.1f;
                if (FadeTimer > FadeDelay)
                    FadeTimer = FadeDelay;
            }
            else
            {
                FadeTimer = 0;
                if (Opacity < 0)
                    Opacity = 0;
                Opacity += 0.1f;
            }
            Opacity = MathHelper.Clamp(Opacity, -1, 1f);
            if (Opacity < -0.5f)
                Active = false;
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
            Rectangle itemFrame = ItemTexture.Bounds;
            if (AnimationFrames > 1)
            {
                AnimationTimer++;
                AnimationTimer %= 5 * AnimationFrames;
                int frame = AnimationTimer / 5;
                int frameHeight = (ItemTexture.Height / AnimationFrames);
                itemFrame = new(0, frame * frameHeight, ItemTexture.Width, frameHeight);
            }
            spriteBatch.Draw(ItemTexture, position - Vector2.UnitX * style.Width / 4 - Vector2.UnitY * style.Height / 2, itemFrame, Color.White * opacity, 0f, Vector2.Zero, itemScale, SpriteEffects.None, 0);
        }
    }
}
