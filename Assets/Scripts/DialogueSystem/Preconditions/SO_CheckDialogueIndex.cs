namespace DialogueSystem.Preconditions
{
    public class SO_CheckDialogueIndex : SO_DialogueCondition
    {
        public override bool CheckPrecondition(DialogueContext context)
        {
            if(context.actualDialogueIndex >= context.dialogueIndexCount) return false;
            
            return context.actualDialogueIndex == context.requestedDialogueIndex;
        }
    }
}