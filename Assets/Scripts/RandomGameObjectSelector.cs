using UnityEngine;
using Random = UnityEngine.Random;

public class RandomGameObjectSelector : MonoBehaviour {
    [SerializeField] private GameObject[] choices;

    private void Awake() {
        var randomChoice = Random.Range(0, choices.Length);
        for (var i = 0; i < choices.Length; i++) {
            var choice = choices[i];
            choice.SetActive(i == randomChoice);
        }
    }
}
