using FargowiltasSouls.Content.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Projectiles.Souls
{
    public class SpectreSpirit : ModProjectile
    {
        public override string Texture => FargoSoulsUtil.VanillaTextureNPC(NPCID.DungeonSpirit);

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bone);
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 900;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.light = 1f;
            Projectile.DamageType = DamageClass.Magic;
        }
        int TargetNPC = -1;
        bool FoundTarget = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            if (FoundTarget && !TargetNPC.IsWithinBounds(Main.maxProjectiles))
            {
                Projectile.Kill();
                return;
            }
            if (FoundTarget && TargetNPC != -1)
            {
                NPC npc = Main.npc[TargetNPC];
                if (npc.Alive() && npc.CanBeChasedBy()) //target is still valid
                {
                    Vector2 v = npc.Center;
                    float inertia = 15f;
                    float deadzone = 25f;
                    Vector2 vectorToIdlePosition = v - Projectile.Center;
                    float num = vectorToIdlePosition.Length();
                    if (num > deadzone)
                    {
                        vectorToIdlePosition.Normalize();
                        vectorToIdlePosition *= 22f;
                        Projectile.velocity = (Projectile.velocity * (inertia - 1f) + vectorToIdlePosition) / inertia;
                    }
                    else if (Projectile.velocity == Vector2.Zero)
                    {
                        Projectile.velocity.X = -0.15f;
                        Projectile.velocity.Y = -0.05f;
                    }
                }
                else
                {
                    TargetNPC = -1;
                    Projectile.Kill();
                    return;
                }

                return;
            }
            bool slowdown = true;
            if (++Projectile.localAI[2] > 90f)
            {
                int p = player.whoAmI;
                if (p != -1 && p != Main.maxPlayers && Main.player[p].active && !Main.player[p].dead && !Main.player[p].ghost)
                {
                    if (Main.player[p].Distance(Projectile.Center) < 16 * 5)
                    {
                        slowdown = false;
                        Projectile.velocity = Projectile.SafeDirectionTo(Main.player[p].Center) * 9f;
                        Projectile.timeLeft++;

                        if (Projectile.Colliding(Projectile.Hitbox, Main.player[p].Hitbox))
                        {
                            player.FargoSouls().HealPlayer(20);
                            TargetNPC = Projectile.FindTargetWithLineOfSight(1500);
                            FoundTarget = true;
                            FargoGlobalItem.OnRetrievePickup(player);
                            Projectile.friendly = true;
                            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
                            return;
                        }
                    }
                }
            }

            if (slowdown)
                Projectile.velocity *= 0.95f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
            {
                Projectile.velocity.X = oldVelocity.X * -0.9f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
            {
                Projectile.velocity.Y = oldVelocity.Y * -0.9f;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnKill(int timeLeft)
        {
            const int max = 16;
            for (int i = 0; i < max; i++)
            {
                Vector2 vector6 = Vector2.UnitY * 5f;
                vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + Projectile.Center;
                Vector2 vector7 = vector6 - Projectile.Center;
                int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.SpectreStaff, 0f, 0f, 0, default, 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = vector7;
            }
        }
    }
}