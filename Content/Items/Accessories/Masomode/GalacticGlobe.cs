using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Content.Buffs.Minions;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Masomode
{
    public class GalacticGlobe : SoulsItem
    {
        public override bool Eternity => true;

        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.defense = 10;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 8);
        }
        public override void UpdateInventory(Player player)
        {
            player.AddEffect<ChalicePotionEffect>(Item);
        }
        public override void UpdateVanity(Player player)
        {
            player.AddEffect<ChalicePotionEffect>(Item);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[ModContent.BuffType<FlippedBuff>()] = true;
            player.buffImmune[ModContent.BuffType<HallowIlluminatedBuff>()] = true;
            player.buffImmune[ModContent.BuffType<NullificationCurseBuff>()] = true;
            player.buffImmune[ModContent.BuffType<UnstableBuff>()] = true;
            player.buffImmune[ModContent.BuffType<CurseoftheMoonBuff>()] = true;
            //player.buffImmune[BuffID.ChaosState] = true;

            player.AddEffect<ChalicePotionEffect>(Item);


            player.FargoSouls().GravityGlobeEXItem = Item;
            player.FargoSouls().WingTimeModifier += 1f;
        }
    }
    public class ChalicePotionEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<HeartHeader>();
        public override int ToggleItemType => ModContent.ItemType<GalacticGlobe>();
        public static List<int> ChaliceBuffs =
        [
            BuffID.Ironskin,
            BuffID.Regeneration,
            BuffID.Swiftness,
            BuffID.ManaRegeneration,
            BuffID.MagicPower,
            BuffID.AmmoReservation,
            BuffID.Archery,
            BuffID.Builder,
            BuffID.Crate,
            BuffID.Endurance,
            BuffID.Fishing,
            BuffID.Gills,
            BuffID.Lucky,
            BuffID.Heartreach,
            BuffID.Lifeforce,
            BuffID.Mining,
            BuffID.ObsidianSkin,
            BuffID.Rage,
            BuffID.Wrath,
            BuffID.Sonar,
            BuffID.Summoning,
            BuffID.Thorns,
            BuffID.Titan,
            BuffID.Warmth,
            BuffID.WaterWalking
        ];
        public override void PostUpdateEquips(Player player)
        {
            foreach (int buff in ChaliceBuffs)
            {
                int duration = buff == BuffID.Lucky ? 60 * 60 * 15 : 2;
                player.AddBuff(buff, duration);
            }
        }
    }
}