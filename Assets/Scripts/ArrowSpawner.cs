using UnityEngine;

public class ArrowSpawner : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;

    public Arrow[] Arrows => arrows;

    public static ArrowSpawner Instance { get; private set; }

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        #endregion
    }

    void Start() {
        Arrow[] arrowsCopy = new Arrow[arrows.Length];

        // This loop will sort the copy so the indexes match the arrow type 
        // defined by the enum. It also instantiates the arrow in the scene.
        for (int i = 0; i < arrows.Length; i++) {
            int id = (int)System.Enum.Parse(typeof(ArrowId),
                arrows[i].Instance.name);
            arrowsCopy[id] = arrows[i];
            arrowsCopy[id].InstantiateArrow();
        }
        arrows = arrowsCopy;
    }

    public GameObject GetArrow(ArrowId id) {
        return arrows[(int)id].Instance;
    }
}
