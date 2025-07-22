using System.Linq;
using DialogueSystem;
using UnityEngine;

namespace NPC
{
    public class DialogueNpc : BaseNPC
    {
        [SerializeField] private DialogueData[] _dialogues;
        private int _actualDialogueIndex;

        public override bool Interact()
        {
            var actualContext = new DialogueContext()
            {
                actualDialogueIndex = _actualDialogueIndex,
                dialogueIndexCount = _dialogues.Length,
                npcID = _npcID,
                requestedDialogueIndex = _actualDialogueIndex
            };

            for (var i = 0; i < _dialogues.Length; i++)
            {
                if (_dialogues[i].hasBeenCompleted) continue;

                if (_dialogues[i].preConditions.All(x => x.CheckPrecondition(actualContext)))
                {
                    DialogueManager.QueueDialogue(_dialogues[i]);

                    if (i < _dialogues.Length - 1 &&
                        _dialogues[i + 1].preConditions.All(x => x.CheckPrecondition(actualContext)))
                    {
                        _dialogues[i].hasBeenCompleted = true;
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        public override void OnRange()
        {
            _material.color = Color.green;
        }

        public override void OffRange()
        {
            _material.color = Color.black;
        }
    }
}