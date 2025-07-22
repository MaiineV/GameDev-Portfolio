using UnityEngine;

namespace NPC
{
    public abstract class BaseNPC : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected NpcID _npcID;
        [SerializeField] protected Material _material;
        public abstract bool Interact();
        public abstract void OnRange();
        public abstract void OffRange();
    }
}