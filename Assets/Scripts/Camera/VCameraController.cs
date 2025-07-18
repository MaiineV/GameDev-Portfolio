using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class VCameraController : MonoBehaviour
    {
        private int _priority;
        private CinemachineCamera _camera;
        
        [SerializeField] private CameraID _cameraID;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
            CameraManager.RegisterCamera(_cameraID, this);
        }

        public void ChangePriority(int priority)
        {
            _priority = priority;
            _camera.Priority = priority;
        }

        private void OnDestroy()
        {
            CameraManager.UnregisterCamera(_cameraID);
        }
    }
}