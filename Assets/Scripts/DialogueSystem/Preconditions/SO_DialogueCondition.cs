using UnityEngine;

namespace DialogueSystem.Preconditions
{
    [CreateAssetMenu(fileName = "SO_DialogueCondition", menuName = "Scriptable Objects/SO_DialogueCondition")]
    public abstract class SO_DialogueCondition : ScriptableObject
    {
        public abstract bool CheckPrecondition(DialogueContext context);
    }
}