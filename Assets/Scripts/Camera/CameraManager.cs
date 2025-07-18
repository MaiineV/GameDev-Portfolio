using System.Collections;
using System.Collections.Generic;

namespace Camera
{
    public enum CameraID
    {
        None,
        MainCamera,
        FirstDoorCamera
    }

    public static class CameraManager
    {
        private static readonly Dictionary<CameraID, VCameraController> Cameras = new();
        public static CameraID CurrentCamera { get; private set; } = CameraID.None;

        public static void RegisterCamera(CameraID cameraID, VCameraController cameraController)
        {
            if (cameraID == CameraID.None) return;

            Cameras.TryAdd(cameraID, cameraController);
        }

        public static void UnregisterCamera(CameraID cameraID)
        {
            if (cameraID == CameraID.None) return;

            if (Cameras.ContainsKey(cameraID)) Cameras.Remove(cameraID);
        }

        public static void SwapCamera(CameraID newCameraID)
        {
            if (!Cameras.ContainsKey(newCameraID)) return;
            
            if (Cameras.ContainsKey(CurrentCamera))
            {
                Cameras[CurrentCamera].ChangePriority(0);
            }
            
            CurrentCamera = newCameraID;
            Cameras[CurrentCamera].ChangePriority(1);
        }
    }
}