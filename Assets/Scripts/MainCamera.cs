using UnityEngine;

public class MainCamera : MonoBehaviour {
    public static MainCamera Instance;
    [SerializeField] private Transform cameraTransform;

    private void Awake() {
        // Ensure only one instance of MainCamera exists
        if (Instance == null) {
            Instance = this;
        } else {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public Transform GetCameraTransform() {
        return cameraTransform.transform;
    }
}