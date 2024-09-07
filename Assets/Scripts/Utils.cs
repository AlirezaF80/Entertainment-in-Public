using UnityEngine;

public static class Utils {
    public static bool IsInCameraBounds(this Collider2D coll) {
        Vector3 leftColliderPoint =
            new Vector3(coll.bounds.min.x, coll.bounds.center.y, coll.bounds.center.z);
        Vector3 rightColliderPoint =
            new Vector3(coll.bounds.max.x, coll.bounds.center.y, coll.bounds.center.z);
        var camera = Camera.main;
        Vector3 leftViewportPoint = camera.WorldToViewportPoint(leftColliderPoint);
        Vector3 rightViewportPoint = camera.WorldToViewportPoint(rightColliderPoint);
        var isInCameraBounds = leftViewportPoint.x > 0 && rightViewportPoint.x < 1 &&
                               leftViewportPoint.y > 0 && rightViewportPoint.y < 1;
        return isInCameraBounds;
    }
}