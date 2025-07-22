using NPC;

namespace DialogueSystem
{
    public struct DialogueContext
    {
        public NpcID npcID;
        public int actualDialogueIndex;
        public int dialogueIndexCount;
        public int requestedDialogueIndex;
    }
}