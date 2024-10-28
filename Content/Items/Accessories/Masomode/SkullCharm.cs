using FargowiltasSouls.Content.Buffs.Minions;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Masomode
{
    public class SkullCharm : SoulsItem
    {
        public override bool Eternity => true;

        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 42;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Dazed] = true;
            player.GetDamage(DamageClass.Generic) += 0.15f;
            player.endurance -= 0.1f;
            player.aggro -= 400;
            player.FargoSouls().SkullCharm = true;
            player.AddEffect<PungentMinion>(Item);

        }
    }
    public class PungentMinion : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<LumpofFleshHeader>();
        public override int ToggleItemType => ModContent.ItemType<SkullCharm>();
        public override bool MinionEffect => true;
        public override void PostUpdateEquips(Player player)
        {
            if (!player.FargoSouls().LumpOfFlesh && !player.HasBuff<SouloftheMasochistBuff>())
                player.AddBuff(ModContent.BuffType<Buffs.Minions.CrystalSkullBuff>(), 5);
        }
    }
}