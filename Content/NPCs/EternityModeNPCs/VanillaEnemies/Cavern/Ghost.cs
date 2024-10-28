using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.Cavern
{
    public class Ghost : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.Ghost);
        public override void SetDefaults(NPC npc)
        {
            npc.knockBackResist = 0f;
            if (Main.hardMode)
                npc.lifeMax = (int)(npc.lifeMax * 1.25f);
        }
        public override void OnFirstTick(NPC npc)
        {
            base.OnFirstTick(npc);

            if (Main.rand.NextBool(5) && npc.FargoSouls().CanHordeSplit)
                EModeGlobalNPC.Horde(npc, 3);
        }

        public override void AI(NPC npc)
        {
            base.AI(npc);

            EModeGlobalNPC.Aura(npc, 100, BuffID.Cursed, false, 20);
            npc.dontTakeDamage = false;
            if (npc.HasPlayerTarget && Main.player[npc.target].Alive() && Main.player[npc.target].direction != Main.player[npc.target].HorizontalDirectionTo(npc.Center))
            {
                npc.dontTakeDamage = true;
                npc.velocity *= 0f;
                npc.Opacity = MathHelper.Lerp(npc.Opacity, 0.4f, 0.05f);
            }
            else
            {
                npc.Opacity = MathHelper.Lerp(npc.Opacity, 1f, 0.05f);
                npc.position += npc.velocity * 1f;
            }
        }
        public override void ModifyHitByAnything(NPC npc, Player player, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool(3))
                modifiers.SetMaxDamage(1);
        }
    }
}
