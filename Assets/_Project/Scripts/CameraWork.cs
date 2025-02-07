using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class CameraWork : MonoBehaviour
    {

#region Private Fields

        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 7.0f;

        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float height = 3.0f;

        [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;

        [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool followOnStart = false;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float smoothSpeed = 0.125f;

        // cached transform of the target
        private Transform cameraTransform;

        // maintain a flag internally to reconnect if target is lost or camera is switched
        private bool isFollowing;

        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;

#endregion

#region Monobehaviour Callbacks

        private void Start()
        {
            if( followOnStart )
            {
                OnStartFollowing();
            }
        }

        private void LateUpdate()
        {
            if ( cameraTransform == null && isFollowing )
            {
                OnStartFollowing();
            }
            if( isFollowing )
            {
                Follow();
            }
        }

#endregion

#region Public Methods

        public void OnStartFollowing()
        {
            cameraTransform = Camera.main!.transform;
            isFollowing = true;
            Cut();
        }

#endregion

#region Private Methods

        private void Follow()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                this.transform.position + this.transform.TransformVector(cameraOffset),
                smoothSpeed * Time.deltaTime
            );

            cameraTransform.LookAt(this.transform.position + centerOffset);
        }

        private void Cut()
        {

            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);
            cameraTransform.LookAt(this.transform.position + centerOffset);
        }

#endregion

    }
}