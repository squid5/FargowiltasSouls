using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Projectiles.ChallengerItems
{
    public class SpiritBow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults() 
        {
            Projectile.width = 30;
            Projectile.height = 68;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.FargoSouls().CanSplit = false;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
        }

        private const int chargeMax = 4;
        public ref float charge => ref Projectile.ai[0];
        public ref float soundTimer => ref Projectile.ai[1];

        public override void AI()
        {
            Projectile.frame = (int)charge;
            Player player = Main.player[Projectile.owner];
            Vector2 offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            float rot = (Main.MouseWorld - (player.Center + offset)).ToRotation();
            if (player.channel && !player.noItems && !player.CCed)
            {
                player.heldProj = Projectile.whoAmI;
                player.SetDummyItemTime(2);
                Projectile.timeLeft++;

                Projectile.position = player.Center - offset;
                Projectile.rotation = rot + MathHelper.Pi;
                
                Projectile.direction = Main.MouseWorld.DirectionTo(player.Center).X < 0 ? 1 : -1;
                player.ChangeDir(Projectile.direction);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.ThreeQuarters, rot - MathHelper.PiOver2);

                if (charge <= chargeMax) {
                    charge += 1 / 30f;
                }

                if (charge >= 3.8f)
                {
                    float shake = 0.8f;
                    Projectile.position += new Vector2(Main.rand.NextFloat(-shake, shake), Main.rand.NextFloat(-shake, shake));
                }

                if (soundTimer-- == 0)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath6 with {Pitch = 0.1f + ((charge - 1) / 3) }, player.Center);
                    soundTimer = 10;
                }
            }
            else
            {
                if (FargoSoulsUtil.HostCheck)
                {
                    Vector2 shootspeed = new Vector2(10f * charge, 0f);
                    SoundEngine.PlaySound(SoundID.Item102, player.Center);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, shootspeed.RotatedBy(rot), ModContent.ProjectileType<SpiritArrow>(), (int)(Projectile.damage * charge), 3f, Projectile.owner);
                }
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float rot = Projectile.rotation;
            Rectangle frame = new(0, Projectile.height * (int)charge, texture.Width, Projectile.height);
            SpriteEffects flip = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture, drawPos, frame, lightColor, rot, new Vector2(frame.Width / 2, frame.Height / 2), Projectile.scale, flip);
            return false;
        }
    }
}
