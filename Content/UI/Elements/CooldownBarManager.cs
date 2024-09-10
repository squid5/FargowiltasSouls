using FargowiltasSouls.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.Elements
{
    public class CooldownBarManager : UIState
    {
        public static CooldownBarManager Instance;
        //public static List<UICooldownBar> CooldownBars;
        const int BarWidth = 100;
        const int BarHeight = 20;
        const int SpaceBetweenBars = 20;
        public static void Activate(string nameKey, Texture2D itemTexture, Color fillColor, Func<float> fillRatio)
        {
            if (!SoulConfig.Instance.CooldownBars)
                return;
            if (Instance.Children.Any(b => b is UICooldownBar cdBar && cdBar.NameKey == nameKey))
                return;

            float x = SoulConfig.Instance.CooldownBarsX;
            float y = SoulConfig.Instance.CooldownBarsY;
            int i = Instance.Children.Count();

            // If there's a free unused bar, transform it into the new bar
            if (Instance.Children.FirstOrDefault(b => b is UICooldownBar cdBar && cdBar.Opacity <= -0.5f) is UICooldownBar freeBar)
            {
                freeBar.NameKey = nameKey;
                freeBar.ItemTexture = itemTexture;
                freeBar.FillColor = fillColor;
                freeBar.FillRatio = fillRatio;
            }
            else
            {
                UICooldownBar newBar = new(nameKey, FargoUIManager.CooldownBarTexture.Value, FargoUIManager.CooldownBarFillTexture.Value, itemTexture, fillColor, fillRatio);
                //int nX = i / 3;
                //int nY = i % 3;
                int nY = i;
                //newBar.Left.Set(x + nX * (SpaceBetweenBars + BarWidth), 0);
                newBar.Left.Set(x, 0);
                newBar.Top.Set(y + nY * (SpaceBetweenBars + BarHeight), 0);
                Instance.Append(newBar);
            }
        }
        public override void OnActivate()
        {
            Instance = this;
            base.OnActivate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!SoulConfig.Instance.CooldownBars)
                return;
            base.Draw(spriteBatch);
            /*
            foreach (UICooldownBar cooldownBar in CooldownBars)
            {
                cooldownBar.Draw(spriteBatch);
            }*/
        }
    }
}
