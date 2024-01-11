﻿using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Forces
{
	public class TimberForce : BaseForce
    {
        public static int[] Enchants => new int[]
        {
            ModContent.ItemType<WoodEnchant>(),
            ModContent.ItemType<BorealWoodEnchant>(),
            ModContent.ItemType<RichMahoganyEnchant>(),
            ModContent.ItemType<EbonwoodEnchant>(),
            ModContent.ItemType<ShadewoodEnchant>(),
            ModContent.ItemType<PalmWoodEnchant>(),
            ModContent.ItemType<PearlwoodEnchant>()
        };
        public static bool[] ContainsEnchant;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();


            // DisplayName.SetDefault("Force of Timber");

            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "森林之力");

            /* Tooltip.SetDefault(
$"[i:{ModContent.ItemType<WoodEnchant>()}] You gain a shop discount based on bestiary completion\n" +
$"[i:{ModContent.ItemType<BorealWoodEnchant>()}] Attacks will periodically be accompanied by several snowballs\n" +
$"[i:{ModContent.ItemType<RichMahoganyEnchant>()}] All grappling hooks shoot, pull, and retract 2.5x as fast\n" +
$"[i:{ModContent.ItemType<EbonwoodEnchant>()}][i:{ModContent.ItemType<ShadewoodEnchant>()}] You have an aura of Corruption and Bleeding\n" +
$"[i:{ModContent.ItemType<PalmWoodEnchant>()}] Double tap down to spawn up to 3 palm tree sentries\n" +
$"[i:{ModContent.ItemType<PearlwoodEnchant>()}] Projectiles may spawn a star when they hit something\n" +
"'Extremely rigid'"); */

            // TODO: localization
            //             string tooltip_ch =
            // @"[i:{0}] 商店价格会依据你的图鉴解锁度降低
            // [i:{1}] 攻击时定期释放几个雪球
            // [i:{2}] 所有钩爪的抛出速度×2，牵引速度×2.5，回收速度×3
            // [i:{3}] 暗影焰、诅咒焰和鲜血光环环绕着你
            // [i:{4}] 双击“下”键召唤一个会向敌人扔橡实的棕榈树哨兵
            // [i:{5}] 弹幕击中敌人或物块时有几率生成一颗星星
            // “很硬”";
            //             Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, string.Format(tooltip_ch, Enchants[0], Enchants[1], Enchants[2], Enchants[3], Enchants[5], Enchants[6]));
            //
            //             string tooltip_pt =
            // @"[i:{0}] Você recebe um disconto nas lojas com base na conclusão do bestiário
            // [i:{1}] Ataques serão periodicamente acompanhados por várias bolas de neve
            // [i:{2}] Todos os ganchos disparam, puxam e retraem 2,5x mais rápido
            // [i:{3}] [i:{4}] Você tem uma aura de Corrupção e Sangrando
            // [i:{5}] Toque duas vezes para baixo para invocar até 3 sentinelas de palmeira
            // [i:{6}] Ataques podem invocar uma estrela quando eles atingem algo
            // 'Extremamente rígido'";
            //             Tooltip.AddTranslation((int)GameCulture.CultureName.Portuguese, string.Format(tooltip_pt, Enchants[0], Enchants[1], Enchants[2], Enchants[3], Enchants[4], Enchants[5], Enchants[6]));
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            modPlayer.timberForce = true;
            WoodEnchant.WoodEffect(player, Item);
            player.AddEffect<BorealEffect>(Item);
            player.AddEffect<MahoganyEffect>(Item);
            player.AddEffect<EbonwoodEffect>(Item);
            player.AddEffect<ShadewoodEffect>(Item);
            player.AddEffect<PalmwoodEffect>(Item);
            player.AddEffect<PearlwoodEffect>(Item);
        }

        public override void UpdateVanity(Player player)
        {
            player.FargoSouls().WoodEnchantDiscount = true;
        }

        public override void UpdateInventory(Player player)
        {
            player.FargoSouls().WoodEnchantDiscount = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants)
                recipe.AddIngredient(ench);
            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
}
