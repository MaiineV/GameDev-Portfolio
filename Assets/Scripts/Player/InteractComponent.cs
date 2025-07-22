using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public interface IInteractable
{
    public bool Interact();
    public void OnRange();
    public void OffRange();
}

namespace Player
{
    public class InteractComponent : MonoBehaviour
    {
        [Header("Interact Variables")]
        [SerializeField] private float radius;
        [SerializeField] private LayerMask interactableMask;

        private Collider _actualInteractable;
        private IInteractable _interactableComponent;

        private void Update()
        {
            CheckInteract();
        }

        internal bool TryInteract()
        {
            if (_interactableComponent == null) return false;
            
            _interactableComponent.Interact();
            return true;
        }

        private void CheckInteract()
        {
            var interactables = 
                Physics.OverlapSphere(transform.position, radius, interactableMask);

            if (interactables.Length <= 0)
            {
                _interactableComponent?.OffRange();
                
                _actualInteractable = null;
                _interactableComponent = null;
                
                return;
            }
            
            interactables = 
                interactables.OrderBy(x=>Vector3.Distance(transform.position, x.transform.position)).ToArray();

            if (interactables[0] == _actualInteractable) return;

            var interactable = interactables[0].GetComponent<IInteractable>();

            if (interactable == null) return;
            
            _interactableComponent?.OffRange();
            _interactableComponent = interactable;
            _interactableComponent.OnRange();
            
            _actualInteractable = interactables[0];
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}