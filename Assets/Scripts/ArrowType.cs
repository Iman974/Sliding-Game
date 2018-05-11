using UnityEngine;

[CreateAssetMenu(fileName = "New arrow", menuName = "Game/Arrow")]
public class ArrowType : ScriptableObject {

    [SerializeField] private Sprite sprite;
    public Sprite Sprite {
        get { return sprite; }
    }

    [SerializeField] private float stayDuration = 1.25f;
    public float StayDuration {
        get { return stayDuration; }
    }

    [SerializeField] private float nextDelay = 0.5f;
    public float NextDelay {
        get { return nextDelay; }
    }

    [Header("Colors")]
    [SerializeField] private Color baseColor = Color.white;
    public Color BaseColor {
        get { return baseColor; }
    }

    [SerializeField] private Color failColor = Color.red;
    public Color FailColor {
        get { return failColor; }
    }

    [SerializeField] private Color successColor = Color.green;
    public Color SuccessColor {
        get { return successColor; }
    }

    [SerializeField] private Color skipColor = Color.red;
    public Color SkipColor {
        get { return skipColor; }
    }

    [SerializeField] private int scoreValue = 50;
    public int ScoreValue {
        get { return scoreValue; }
    }

    [Header("Animations")]
    [SerializeField] private CustomAnimation[] appearAnimation;
    public CustomAnimation[] AppearAnimations {
        get { return appearAnimation; }
    }

    [SerializeField] private CustomAnimation[] successAnimations;
    [SerializeField] private CustomAnimation[] failAnimations;

    [SerializeField] private CustomAnimation[] skipAnimations;
    public CustomAnimation[] SkipAnimations {
        get { return skipAnimations; }
    }

    [SerializeField] private float[] successAnimationsDelays = new float[0];
    [SerializeField] private float[] failAnimationsDelays = new float[0];

    public CustomAnimation[] GetValidationAnimations(bool isValidated) {
        return isValidated ? successAnimations : failAnimations;
    }

    public float[] GetValidationAnimationsDelays(bool isValidated) {
        return isValidated ? successAnimationsDelays : failAnimationsDelays;
    }

    private void OnEnable() {
        int delaysArraySize = successAnimationsDelays.Length;

        if (delaysArraySize > 0 && delaysArraySize != successAnimations.Length) {
            throw new System.IndexOutOfRangeException("Delays array size is inconsistent on " + name);
        }
    }
}
