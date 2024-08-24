using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
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
        public override void OnKill(int timeLeft)
        {
            ScreenShakeSystem.StartShake(2f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (FargoSoulsUtil.HostCheck)
            {
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<ObsidianExplosion>(), Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * 5);
            target.AddBuff(BuffID.OnFire3, 60 * 5);
        }
    }
}
