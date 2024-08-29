using FargowiltasSouls.Content.Bosses.Champions.Cosmos;
using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Content.Projectiles.Souls;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    public class VortexEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Vortex Enchantment");
            /* Tooltip.SetDefault(
@"Double tap down to toggle stealth, reducing chance for enemies to target you but slowing movement
When entering stealth, spawn a vortex that draws in enemies and projectiles
While in stealth, your own projectiles will not be sucked in
'Tear into reality'"); */
            //             DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "星旋魔石");
            //             Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, 
            // @"双击'下'键切换至隐形模式，减少敌人以你为目标的几率，但大幅降低移动速度
            // 进入隐形状态时生成一个会吸引敌人和弹幕的旋涡
            // 处于隐形状态时你的弹幕不会被旋涡吸引
            // '撕裂现实'");
        }

        public override Color nameColor => new(0, 242, 170);


        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Purple;
            Item.value = 400000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AddEffects(player, Item);
        }
        public static void AddEffects(Player player, Item item)
        {
            player.AddEffect<VortexEffect>(item);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.VortexHelmet)
            .AddIngredient(ItemID.VortexBreastplate)
            .AddIngredient(ItemID.VortexLeggings)
            //vortex wings
            .AddIngredient(ItemID.VortexBeater)
            .AddIngredient(ItemID.Phantasm)
            //chain gun
            //electrosphere launcher
            .AddIngredient(ItemID.SDMG)

            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
    public class VortexProjGravity : AccessoryEffect
    {
        public override Header ToggleHeader => null;
        
        public override void PostUpdateEquips(Player player)
        {
            foreach (Projectile toProj in Main.projectile.Where(p => p != null && p.active && p.friendly && p.owner == player.whoAmI))
            {
                foreach (Projectile fromProj in Main.projectile.Where(p => p != null && p.active && p.friendly && p.whoAmI != toProj.whoAmI && p.owner == player.whoAmI && !TungstenEffect.TungstenAlwaysAffectProj(p) && p.FargoSouls().CanSplit && FargoSoulsUtil.CanDeleteProjectile(p, 0)))
                {
                    // if (fromProj.aiStyle != 1)
                    //    continue;
                    Vector2 dif = toProj.Center - fromProj.Center;
                    int distSq = (int)dif.LengthSquared();
                    if (distSq < 1 || float.IsNaN(distSq))
                        continue;
                    int rSquared = distSq;
                    rSquared += 100;
                    Vector2 force = Utils.SafeNormalize(dif, Vector2.UnitY);
                    const float gravityConstant = 9000f; // tweak
                    force *= gravityConstant /* (toProj.Size.Length() * fromProj.Size.Length())*/ / rSquared;
                    fromProj.velocity += force;
                }
            }
        }
    }
    public class VortexEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<CosmoHeader>();
        public override int ToggleItemType => ModContent.ItemType<VortexEnchant>();
        public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile projectile, Item item)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (modPlayer.VortexCD <= 0 && player.Distance(target.Center) > 1000)
            {
                bool force = modPlayer.ForceEffect<VortexEnchant>();
                int dmg = 10000;
                if (force)
                    dmg = 16500;
                Vector2 velocity = player.DirectionTo(target.Center);
                int damage = FargoSoulsUtil.HighestDamageTypeScaling(modPlayer.Player, dmg);
                FargoSoulsUtil.NewProjectileDirectSafe(modPlayer.Player.GetSource_ItemUse(modPlayer.Player.HeldItem), player.Center, velocity, ModContent.ProjectileType<VortexLaser>(), damage, 0f, modPlayer.Player.whoAmI, 1f);
                float cd = force ? 6f : 8f;
                modPlayer.VortexCD = LumUtils.SecondsToFrames(cd);
            }
        }
    }
}
