using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Content.Projectiles.Souls;
using FargowiltasSouls.Content.UI.Elements;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls.Core.Toggler.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    public class MeteorEnchant : BaseEnchant
    {
        public static readonly Color NameColor = new(95, 71, 82);
        public override Color nameColor => NameColor;


        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = 100000;
        }

        public const int METEOR_ADDED_DURATION = 450;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AddEffects(player, Item);
        }
        public static void AddEffects(Player player, Item item)
        {
            player.AddEffect<MeteorMomentumEffect>(item);
            //player.AddEffect<MeteorTrailEffect>(item);
            player.AddEffect<MeteorEffect>(item);
        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.MeteorHelmet)
            .AddIngredient(ItemID.MeteorSuit)
            .AddIngredient(ItemID.MeteorLeggings)
            .AddIngredient(ItemID.StarCannon)
            .AddIngredient(ItemID.Magiluminescence)
            .AddIngredient(ItemID.PlaceAbovetheClouds)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
    public class MeteorMomentumEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<CosmoHeader>();
        public override int ToggleItemType => ModContent.ItemType<MeteorEnchant>();
        public override void PostUpdateEquips(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (!player.FargoSouls().NoMomentum && !player.mount.Active) //overriden by nomomentum
                {
                    player.runAcceleration *= 1.3f;
                    player.runSlowdown *= 1.3f;

                }
                player.hasMagiluminescence = true;
                /*
                if (player.HasEffect<MeteorTrailEffect>())
                {
                    const int SparkDelay = 2;
                    int Timer = (int)(Main.GlobalTimeWrappedHourly * 60) % 60;
                    if (player.velocity != Vector2.Zero && Timer % SparkDelay == 0)
                    {
                        for (int i = -1; i < 2; i += 2)
                        {
                            Vector2 vel = (-player.velocity).RotatedBy(i * MathHelper.Pi / 7).RotatedByRandom(MathHelper.Pi / 12);
                            int damage = 22;
                            Vector2 pos = player.Center;
                            Vector2 offset = Vector2.Normalize(player.velocity).RotatedBy(MathHelper.PiOver2 * -i) * (player.width / 2);
                            Projectile.NewProjectile(GetSource_EffectItem(player), pos + offset, vel, ModContent.ProjectileType<MeteorFlame>(), FargoSoulsUtil.HighestDamageTypeScaling(player, damage), 0.5f, player.whoAmI);
                        }
                    }
                }
                */
            }
        }
    }
    /*
    public class MeteorTrailEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<CosmoHeader>();
        public override int ToggleItemType => ModContent.ItemType<MeteorEnchant>();
        public override bool ExtraAttackEffect => true;
    }
    */
    public class MeteorEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<CosmoHeader>();
        public override int ToggleItemType => ModContent.ItemType<MeteorEnchant>();
        public override bool ExtraAttackEffect => true;
        public const int Cooldown = 60 * 10;
        public override void PostUpdateEquips(Player player)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (player.whoAmI == Main.myPlayer)
            {
                if (modPlayer.WeaponUseTimer > 0)
                {
                    modPlayer.MeteorCD--;
                }
                float denominator = 20f;
                if (modPlayer.ForceEffect<MeteorEnchant>())
                    denominator = 12f;
                modPlayer.MeteorCD -= player.velocity.Length() / denominator;
            }
        }
        public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile projectile, Item item)
        {
            if (player.whoAmI != Main.myPlayer)
                return;
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (modPlayer.MeteorCD > 0)
                return;

            bool forceEffect = modPlayer.ForceEffect<MeteorEnchant>();
            int damage = forceEffect ? 400 : 90;
            modPlayer.MeteorCD = Cooldown;
            CooldownBarManager.Activate("MeteorEnchantCooldown", ModContent.Request<Texture2D>("FargowiltasSouls/Content/Items/Accessories/Enchantments/MeteorEnchant").Value, Color.Lerp(MeteorEnchant.NameColor, Color.OrangeRed, 0.75f), () => 1f - Main.LocalPlayer.FargoSouls().MeteorCD / (float)Cooldown); 

            Vector2 pos = new(player.Center.X + Main.rand.NextFloat(-1000, 1000), player.Center.Y - 1000);
            Vector2 vel = new(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(8, 12));

            pos.X = target.Center.X + Main.rand.NextFloat(-16, 16);

            //can retarget better at them, but dont aim meteors upwards
            Vector2 predictive = Main.rand.NextFloat(10f, 30f) * target.velocity;
            pos.X += predictive.X;
            Vector2 targetPos = target.Center + predictive;
            if (pos.Y < targetPos.Y)
            {
                Vector2 accurateVel = vel.Length() * pos.SafeDirectionTo(targetPos);
                vel = Vector2.Lerp(vel, accurateVel, 0.9f);
            }
            SoundEngine.PlaySound(new SoundStyle("FargowiltasSouls/Assets/Sounds/ThrowShort"), pos);

            int i = Projectile.NewProjectile(GetSource_EffectItem(player), pos, vel, ModContent.ProjectileType<MeteorEnchantMeatball>(), FargoSoulsUtil.HighestDamageTypeScaling(player, damage), 0.5f, player.whoAmI, 0, 0.5f + (float)Main.rand.NextDouble() * 0.3f);
            if (forceEffect && i.IsWithinBounds(Main.maxProjectiles))
            {
                Projectile p = Main.projectile[i];
                p.position = p.Center;
                p.scale *= 2;
                p.width *= 2;
                p.height *= 2;
                p.Center = p.position;
            }
        }
    }
}
