using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Content.Items.Accessories.Souls;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Forces
{
    [AutoloadEquip(EquipType.Wings)]
    public class LifeForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            Enchants[Type] =
            [
                ModContent.ItemType<PumpkinEnchant>(),
                ModContent.ItemType<BeeEnchant>(),
                ModContent.ItemType<SpiderEnchant>(),
                ModContent.ItemType<TurtleEnchant>(),
                ModContent.ItemType<BeetleEnchant>()
            ];
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new Terraria.DataStructures.WingStats(1000);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            SetActive(player);
            player.AddEffect<LifeForceEffect>(Item);
            modPlayer.LifeForceActive = true;
            // Pumpkin Enchant
            if (!player.HasEffect<LifeForceEffect>())
                player.AddEffect<PumpkinEffect>(Item);
            // Bee Enchant
            player.AddEffect<BeeEffect>(Item);
            player.strongBees = true;
            // Spider Enchant
            player.AddEffect<SpiderEffect>(Item);
            // Turtle Enchant
            if (player.HasEffect<LifeForceEffect>())
            {
                player.turtleThorns = true;
                player.thorns = 5f;
                modPlayer.CactusImmune = true;
            }
            else
            {
                TurtleEnchant.AddEffects(player, Item);
            }
            // Beetle Enchant

            if (player.HasEffect<LifeForceEffect>())
            {
                player.AddEffect<BeetleEffect>(Item);
                if (modPlayer.LifeBeetleDuration > 0 && player.HasEffect<BeetleEffect>())
                {
                    player.AddBuff(BuffID.BeetleMight3, modPlayer.LifeBeetleDuration);
                    player.AddBuff(BuffID.BeetleEndurance3, modPlayer.LifeBeetleDuration);
                    player.beetleOffense = true;
                    player.beetleDefense = true;
                    player.GetDamage(DamageClass.Generic) += 0.3f;
                    player.GetDamage(DamageClass.Melee) -= 0.3f; //offset the actual vanilla beetle buff
                }
            }
            else
            {

                BeetleEnchant.AddEffects(player, Item);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants[Type])
                recipe.AddIngredient(ench);
            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            player.wingsLogic = ArmorIDs.Wing.LongTrailRainbowWings;
            ascentWhenFalling = 1f;
            ascentWhenRising = 0.25f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 1.75f;
            constantAscend = 0.135f;
            if (player.controlUp)
            {
                ascentWhenFalling *= 6f;
                ascentWhenRising *= 6f;
                constantAscend *= 6f;
            }
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 18f;
            acceleration = 0.75f;
        }
    }
    public class LifeForceEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<LifeHeader>();
        public override int ToggleItemType => ModContent.ItemType<LifeForce>();
    }
}
