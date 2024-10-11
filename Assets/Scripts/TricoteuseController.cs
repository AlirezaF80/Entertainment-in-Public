using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class TricoteuseController : MonoBehaviour {
        [SerializeField] private Animator animator;
        [SerializeField] private string talkCountName = "talkCount";
        [SerializeField] private Vector2 talkCountRange;
        [SerializeField] private Vector2 talkAnimDelayRange;
        [SerializeField] private float talkAnimDuration;

        private void Start() {
            animator.SetInteger(talkCountName, Random.Range((int)talkCountRange.x, (int)talkCountRange.y));
            StartCoroutine(Talk());
        }
        
        private IEnumerator Talk() {
            while (true) {
                yield return new WaitForSeconds(talkAnimDuration);
                animator.SetInteger(talkCountName, animator.GetInteger(talkCountName) - 1);
                if (animator.GetInteger(talkCountName) > 0) continue;
                yield return new WaitForSeconds(Random.Range(talkAnimDelayRange.x, talkAnimDelayRange.y));
                animator.SetInteger(talkCountName, Random.Range((int)talkCountRange.x, (int)talkCountRange.y));
            }
        }
    }
}