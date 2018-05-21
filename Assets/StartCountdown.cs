using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartCountdown : MonoBehaviour {

    [SerializeField] private int countDownStart = 3;
    [SerializeField] private Text countDownText;
    [SerializeField] private GameObject arrowImg;

    private IEnumerator Start() {
        for (int i = countDownStart; i > 0; i--) {
            countDownText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        countDownText.enabled = false;
        enabled = false;

        arrowImg.SetActive(true);
        GameManager.Instance.enabled = true;
    }
}
