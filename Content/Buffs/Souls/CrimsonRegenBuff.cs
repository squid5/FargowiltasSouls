﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using FargowiltasSouls.Core.ModPlayers;

namespace FargowiltasSouls.Content.Buffs.Souls
{
    public class CrimsonRegenBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crimson Regen");
            // Description.SetDefault("You are regenning your last wound");
            Main.buffNoSave[Type] = true;
            //Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();

            modPlayer.CrimsonRegenTime -= 1; //remove one tick from counter

            if (modPlayer.CrimsonRegenTime == 0)
            {
                player.Heal(modPlayer.CrimsonRegenAmount); //crimsonregenamount is set in CrimsonEnchant.cs

                if (modPlayer.CrimsonWizCharge)
                {
                    modPlayer.CrimsonRegenTime = 7 * 60;
                    modPlayer.CrimsonWizCharge = false;
                }
                else {
                    player.DelBuff(buffIndex); //remove buff if no wizard/wizard used
                }
            } 

            for (int i = 0; i < 3; i++)
            {
                int dustType = modPlayer.CrimsonWizCharge ? DustID.Crimson : DustID.Crimson; //change dust if its wizarded
                int num6 = Dust.NewDust(player.position, player.width, player.height, dustType, 0f, 0f, 175, default, 1.75f);
                Main.dust[num6].noGravity = true;
                Main.dust[num6].velocity *= 0.75f;
                int num7 = Main.rand.Next(-40, 41);
                int num8 = Main.rand.Next(-40, 41);
                Dust dust = Main.dust[num6];
                dust.position.X += num7;
                Dust dust2 = Main.dust[num6];
                dust2.position.Y += num8;
                Main.dust[num6].velocity.X = (float)-(float)num7 * 0.075f;
                Main.dust[num6].velocity.Y = (float)-(float)num8 * 0.075f;
            }

        }
    }
}