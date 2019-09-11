using UnityEngine;

public class ArrowPool : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;

    public Arrow[] Arrows => arrows;

    public static ArrowPool Instance { get; private set; }

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        #endregion
    }
}
