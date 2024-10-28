using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Buffs.Souls
{
    public class MushroomPowerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            float stats = 0.2f;
            if (player.FargoSouls().ForceEffect<ShroomiteEnchant>())
                stats = 0.3f;
            player.GetDamage(DamageClass.Generic) += stats;
            player.GetCritChance(DamageClass.Generic) += stats * 100;
        }
    }
}
