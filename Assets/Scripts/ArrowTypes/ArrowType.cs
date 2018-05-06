using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class ArrowType : ScriptableObject {

    [SerializeField] private Sprite sprite;
    public Sprite Sprite {
        get { return sprite; }
    }

    [SerializeField] private float stayDuration;
    public float StayDuration {
        get { return stayDuration; }
    }

    [SerializeField] private float nextDelay;
    public float NextDelay {
        get { return nextDelay; }
    }

    [SerializeField] private Color color;
    public Color Color {
        get { return color; }
    }

    [SerializeField] private int scoreValue;
    public int ScoreValue {
        get { return scoreValue; }
    }

    public abstract IEnumerator Show(Image arrowImg);

    public abstract IEnumerator Hide(Image arrowImg);
}
