using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartCountdown : MonoBehaviour {

    [SerializeField] private int countDownStart = 3;
    [SerializeField] private Text countDownText;

    private void OnEnable() {
        StartCoroutine(ShowCountDown());
    }

    private IEnumerator ShowCountDown() {
        for (int i = countDownStart; i > 0; i--) {
            countDownText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        gameObject.SetActive(false);
        Game.Instance.enabled = true;
    }
}
