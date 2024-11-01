using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Content.Projectiles
{
    public class EffectVisual : ModProjectile
    {
        public override string Texture => FargoSoulsUtil.EmptyTexture;

        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public ref float Effect => ref Projectile.ai[0];
        public override void AI()
        {
            Projectile.velocity = Vector2.UnitY * -0.75f;

            Projectile.scale += 2 / 60f;
            Projectile.Opacity -= 2 / 60f;
            if (Projectile.Opacity <= 0.01f)
                Projectile.Kill();
        }
        public enum Effects
        {
            MythrilEnchant,
            MythrilSword,
            MythrilHalberd
        }
        public string EffectTexture()
        {
            return ((Effects)Effect) switch
            {
                Effects.MythrilEnchant => "FargowiltasSouls/Content/Items/Accessories/Enchantments/MythrilEnchant",
                Effects.MythrilSword => FargoSoulsUtil.VanillaTextureItem(ItemID.MythrilSword),
                Effects.MythrilHalberd => FargoSoulsUtil.VanillaTextureItem(ItemID.MythrilHalberd),
                _ => FargoSoulsUtil.EmptyTexture
            };
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(EffectTexture());
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = frame.Size() / 2f;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame), Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
