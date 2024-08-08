using FargowiltasSouls.Content.Projectiles.BossWeapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Weapons.SwarmDrops
{
    public class SlimeSlingingSlasher : SoulsItem
    {
        int Timer = 0;
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 335;
            Item.DamageType = DamageClass.Melee;
            Item.width = 48;
            Item.height = 64;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 10);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
            Item.scale = 1.5f;
        }

        public override void HoldItem(Player player) //fancy momentum swing, this should be generalized and applied to other swords imo
        {
            if (player.itemAnimation == 0)
            {
                Timer = 0;
                return;
            }

            if (player.itemAnimation == player.itemAnimationMax)
            {
                Timer = player.itemAnimationMax;
            }
            if (player.itemAnimation > 0)
            {
                Timer--;
            }
            if (Timer == player.itemAnimationMax / 2)
            {
                SoundEngine.PlaySound(SoundID.Item1, player.Center);
                //SoundEngine.PlaySound(SoundID.Item39, player.Center);
            }
            if (Timer > 2 * player.itemAnimationMax / 3)
            {
                player.itemAnimation = player.itemAnimationMax;
                Item.noMelee = true;
                player.direction = (int)player.HorizontalDirectionTo(Main.MouseWorld);
            }
            else
            {
                Item.noMelee = false;
                float prog = (float)Timer / (2 * player.itemAnimationMax / 3);
                player.itemAnimation = (int)(player.itemAnimationMax * Math.Pow(MomentumProgress(prog), 2));
            }

        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.onFire || target.onFire2 || target.onFire3)
            {
                modifiers.FinalDamage *= 1.2f;
            }

        }
        public static float MomentumProgress(float x)
        {
            return (x * x * 3) - (x * x * x * 2);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 240);
            SoundEngine.PlaySound(SoundID.Item17);
            Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<Slimesplosion>(), damageDone, 1f, Item.whoAmI, 1, 1, 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(null, "SlimeKingsSlasher")
            .AddIngredient(ModContent.Find<ModItem>("Fargowiltas", "EnergizerSlime"))
            .AddIngredient(ItemID.LunarBar, 10)

            .AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"))

            .Register();
        }
    }
}