using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AEA
{
    public class ParallaxBehavior : MonoBehaviour
    {
        [SerializeField] private Vector2 _parallaxEffectMultiplier;

        [SerializeField] private Transform _cameraTransform; // Expose in the Unity Editor

        private Vector3 _lastCameraPosition;

        void Start()
        {
            if (_cameraTransform == null)
            {
                // Attempt to find the camera if not assigned in the Inspector
                _cameraTransform = Camera.main?.transform;
            }

            if (_cameraTransform == null)
            {
                Debug.LogError("Camera not found. Make sure the camera is tagged as 'MainCamera' or assign it manually.");
            }

            _lastCameraPosition = _cameraTransform.position;
        }

        private void LateUpdate()
        {
            ParallaxEffect();
        }

        private void ParallaxEffect()
        {
            if (_cameraTransform != null)
            {
                float parallaxFactorX = _parallaxEffectMultiplier.x;
                float parallaxFactorY = _parallaxEffectMultiplier.y;

                Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
                Vector3 parallaxEffect = new Vector3(deltaMovement.x * parallaxFactorX, deltaMovement.y * parallaxFactorY, 0f);

                transform.position += parallaxEffect;
                _lastCameraPosition = _cameraTransform.position;
            }
        }
    }
}
