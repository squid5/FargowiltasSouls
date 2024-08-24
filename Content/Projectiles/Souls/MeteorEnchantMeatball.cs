using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Projectiles.Souls
{
    public class MeteorEnchantMeatball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.timeLeft = 60 * 4;
            Projectile.tileCollide = false;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.penetrate = 1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 120;
            Projectile.FargoSouls().noInteractionWithNPCImmunityFrames = true;
            Projectile.FargoSouls().CanSplit = false;
            Projectile.extraUpdates = 2;
            Projectile.scale = 2;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 60 * 2.5f)
                Projectile.tileCollide = true;
            if (++Projectile.frameCounter > 4)
            {
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }
            if (++Projectile.localAI[0] % 2 == 1)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y);
                Main.dust[d].noGravity = true;
            }

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            => Projectile.Distance(FargoSoulsUtil.ClosestPointInHitbox(targetHitbox, Projectile.Center)) < projHitbox.Width / 2;
        public override void OnKill(int timeLeft)
        {
            ScreenShakeSystem.StartShake(2f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (FargoSoulsUtil.HostCheck)
            {
                int i = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<ObsidianExplosion>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
                float scale = Projectile.scale / 2f;
                if (i.IsWithinBounds(Main.maxProjectiles))
                {
                    Projectile p = Main.projectile[i];
                    p.position = p.Center;
                    p.scale *= scale;
                    p.width = (int)(p.width * scale);
                    p.height = (int)(p.height * scale);
                    p.Center = p.position;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * 5);
            target.AddBuff(BuffID.OnFire3, 60 * 5);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //draw projectile
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            int num156 = texture2D13.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Vector2 drawOffset = Projectile.rotation.ToRotationVector2() * (texture2D13.Height - Projectile.height) / 2;

            Color color26 = lightColor;
            color26 = Projectile.GetAlpha(color26);

            SpriteEffects effects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            Main.EntitySpriteDraw(texture2D13, Projectile.Center + drawOffset - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);

            return false;
        }
    }
}
