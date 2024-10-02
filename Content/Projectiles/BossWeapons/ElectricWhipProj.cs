using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Linq;
using FargowiltasSouls.Content.Projectiles.Deathrays;

namespace FargowiltasSouls.Content.Projectiles.BossWeapons
{
    public class ElectricWhipProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.RangeMultiplier = 1f;
            Projectile.rotation += Projectile.ai[0];
        }
        int[] Cooldowns = new int[Main.maxProjectiles];
        public ref float LightningCounter => ref Projectile.ai[2];
        public override bool PreAI()
        {
            if (LightningCounter < 4)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Cooldowns[i] > 0)
                        Cooldowns[i]--;
                    Projectile p = Main.projectile[i];
                    if (p.Alive() && p.minion && p.owner == Projectile.owner && Cooldowns[p.whoAmI] <= 0 && Projectile.Colliding(Projectile.Hitbox, p.Hitbox))
                    {
                        NPC target = Projectile.FindTargetWithinRange(150, true);
                        //Projectile otherMinion = Main.projectile.Where(p2 => p2.Alive() && p2.whoAmI != p.whoAmI && p2.minion && Cooldowns[p2.whoAmI] <= 0 && p2.owner == Projectile.owner).OrderBy(x => x.Distance(Projectile.Center)).FirstOrDefault();
                        if (target != null)
                        {
                            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);
                            if (FargoSoulsUtil.HostCheck)
                                Projectile.NewProjectile(Projectile.InheritSource(Projectile), p.Center, p.DirectionTo(target.Center), ModContent.ProjectileType<ElectricWhipLightning>(), Projectile.originalDamage / 3, Projectile.knockBack, Projectile.owner, ai2: p.Distance(target.Center));
                            Cooldowns[p.whoAmI] = /*Cooldowns[otherMinion.whoAmI] = */60;
                            LightningCounter++;
                        }
                    }
                }
            }
            return base.PreAI();
        }
        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.Red);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = [];
            Projectile.FillWhipControlPoints(Projectile, list);

            DrawLine(list);


            Main.DrawWhip_CoolWhip(Projectile, list);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[Type].Value;

            return false;
        }
    }
}
