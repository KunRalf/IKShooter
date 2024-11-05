using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class SceneCameras : MonoBehaviour
    {
        [field:SerializeField] public CinemachineFreeLook PlayerCamera { get; private set; }
    }
}