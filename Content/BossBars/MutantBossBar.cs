using FargowiltasSouls.Content.Bosses.MutantBoss;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.BossBars
{
    public class MutantBossBar : ModBossBar
    {
        private int bossHeadIndex = -1;
        public override Asset<Texture2D> GetIconTexture(ref Microsoft.Xna.Framework.Rectangle? iconFrame)
        {   
           if (bossHeadIndex != -1)
           {
              return TextureAssets.NpcHeadBoss[bossHeadIndex];
           }
           return null;
        } 
        public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams)
        {
            return true;
        }

        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
        {
            NPC target = Main.npc[info.npcIndexToAimAt];
            if (target.townNPC || !target.active)
                return false;

            life = target.life;
            lifeMax = target.lifeMax;
            

            bossHeadIndex = target.GetBossHeadTextureIndex();
            return true;
        }
    }
}
