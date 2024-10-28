using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Masomode
{
    public class NymphsPerfume : SoulsItem
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
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 4);
        }

        public override void UpdateInventory(Player player)
        {
            FargoSoulsPlayer fargoPlayer = player.FargoSouls();
            fargoPlayer.NymphsPerfumeRespawn = true;
        }

        public override void UpdateVanity(Player player)
        {
            FargoSoulsPlayer fargoPlayer = player.FargoSouls();
            fargoPlayer.NymphsPerfumeRespawn = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Lovestruck] = true;
            player.buffImmune[ModContent.BuffType<LovestruckBuff>()] = true;
            player.buffImmune[ModContent.BuffType<HexedBuff>()] = true;
            player.buffImmune[BuffID.Stinky] = true;
            FargoSoulsPlayer fargoPlayer = player.FargoSouls();
            fargoPlayer.NymphsPerfumeRespawn = true;
            player.AddEffect<NymphPerfumeEffect>(Item);
        }
    }
    public class NymphPerfumeEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<BionomicHeader>();
        public override int ToggleItemType => ModContent.ItemType<NymphsPerfume>();
        public override bool ExtraAttackEffect => true;
        public override void PostUpdateEquips(Player player)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            modPlayer.NymphsPerfume = true;
            if (modPlayer.NymphsPerfumeCD > 0)
                modPlayer.NymphsPerfumeCD -= modPlayer.MasochistSoul ? 10 : 1;
        }
    }
}