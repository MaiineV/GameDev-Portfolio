using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        private readonly Queue<DialogueData> _dialogueQueue = new();

        private bool _isProcessingDialogue = false;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start()
        {
            UI_DialogueManager.instance.OnEndDialogue += OnEndDialogue;
        }

        private bool OnEndDialogue()
        {
            if (!_dialogueQueue.Any())
            {
                EventManager.Trigger(EventName.OnPlayerResetMovement);
                ControllerManager.ChangeInputMap(ActionMapType.Player);
                _isProcessingDialogue = false;
                return true;
            }
            
            UI_DialogueManager.instance.InitDialogue(_dialogueQueue.Dequeue());
            return false;

        }

        public static void QueueDialogue(DialogueData dialogue)
        {
            _instance._dialogueQueue.Enqueue(dialogue);
            if (_instance._isProcessingDialogue) return;

            UI_DialogueManager.instance.InitDialogue(_instance._dialogueQueue.Dequeue());
            ControllerManager.ChangeInputMap(ActionMapType.UI);
            _instance._isProcessingDialogue = true;
        }
    }
}