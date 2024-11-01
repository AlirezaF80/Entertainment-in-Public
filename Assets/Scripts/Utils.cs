using System.Collections.Generic;
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
        // check if any part of object is in camera bounds, even if half of it is in bounds
        var isInCameraBounds = leftViewportPoint.x is < 1 and > 0 || rightViewportPoint.x is > 0 and < 1;
        return isInCameraBounds;
    }

    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }
    
    public static void SetActive(this IEnumerable<GameObject> gameObjects, bool active) {
        foreach (var go in gameObjects) {
            go.SetActive(active);
        }
    }
}