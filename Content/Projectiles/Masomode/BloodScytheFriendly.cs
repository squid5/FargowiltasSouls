using FargowiltasSouls.Core.Systems;
using FargowiltasSouls.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Luminance.Core.Graphics;
using FargowiltasSouls.Assets.ExtraTextures;
using Terraria.Audio;

namespace FargowiltasSouls.Content.Projectiles.Masomode
{
    public class BloodScytheFriendly : ModProjectile//, IPixelatedPrimitiveRenderer
    {
        public override string Texture => "FargowiltasSouls/Content/Projectiles/Masomode/BloodScytheVanilla1";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 1;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Red * Projectile.Opacity;
        }
        /*public float WidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.scale * Projectile.width * 1.3f;
            return MathHelper.SmoothStep(baseWidth, 3.5f, completionRatio);
        }

        public static Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(Color.Red, Color.Transparent, completionRatio) * 0.7f;
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader shader = ShaderManager.GetShader("FargowiltasSouls.BlobTrail");
            FargoSoulsUtil.SetTexture1(FargosTextureRegistry.FadedStreak.Value);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(WidthFunction, ColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: shader), 25);
            Texture2D glowTexture = ModContent.Request<Texture2D>("FargowiltasSouls/Content/Projectiles/GlowRing").Value;

            Vector2 glowDrawPosition = Projectile.Center;

            Main.EntitySpriteDraw(glowTexture, glowDrawPosition, null, Color.Teal, Projectile.rotation, glowTexture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None, 0);
        }
        */
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DemonScythe);
            AIType = ProjectileID.DemonScythe;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.FargoSouls().CanSplit = false;
            Projectile.FargoSouls().noInteractionWithNPCImmunityFrames = true;

            FargowiltasSouls.MutantMod.Call("LowRenderProj", Projectile);

            Projectile.Opacity = 0.4f;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * 0.8f;
            Projectile.ai[0] += 1f;
            if (!(Projectile.ai[0] < 30f))
            {
                if (Projectile.ai[0] < 100f)
                {
                    Projectile.velocity *= 1.06f;
                }
                else
                {
                    Projectile.ai[0] = 200f;
                }
            }

            Vector2 offset = new Vector2(0, -20).RotatedBy(Projectile.rotation);
            offset = offset.RotatedByRandom(MathHelper.Pi / 6);
            int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.BloodWater, 0f, 0f, 150);
            Main.dust[d].position += offset;
            float velrando = Main.rand.Next(20, 31) / 10;
            Main.dust[d].velocity = Projectile.velocity / velrando;
            Main.dust[d].noGravity = true;
            Main.dust[d].scale = 1.2f;
        }
    }
}
