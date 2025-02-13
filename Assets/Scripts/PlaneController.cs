﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlaneController : MonoBehaviour {
    [SerializeField] private float flyHeight = 5;
    [SerializeField] private float flyWidth = 1;
    [SerializeField] private float flySpeed = 5;
    [SerializeField] private float delayBeforeFly = 0.25f;
    [SerializeField] private AnimationCurve flyCurve;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.GetComponent<PlayerController>()) return;
        StartCoroutine(Fly());
    }

    private IEnumerator Fly() {
        // pick a random position based on direction facing, and height, and fly to it
        yield return new WaitForSeconds(delayBeforeFly);
        var targetPosition = GetFinalFlyPosition();
        var flyTime = Vector2.Distance(transform.position, targetPosition) / flySpeed;
        var timeElapsed = 0f;
        var startPosition = transform.position;
        while (timeElapsed < flyTime) {
            timeElapsed += Time.deltaTime;
            var t_norm = timeElapsed / flyTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, flyCurve.Evaluate(t_norm));
            yield return null;
        }
    }

    private Vector3 GetFinalFlyPosition() {
        return transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * flyWidth, flyHeight, 0);
    }

    private void OnDrawGizmosSelected() {
        // show to final possible position based on current direction
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetFinalFlyPosition(), 0.5f);
    }
}