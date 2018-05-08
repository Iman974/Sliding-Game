using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New color animation", menuName = "Game/Custom Animations/Color")]
public class ColorAnimation : CustomAnimation {

    [SerializeField] private AnimationCurve lerpCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private float lerpSpeed = 1f;
    //[SerializeField] private float cycleOffset;

    public override Type AnimatedComponent {
        get {
            return typeof(Graphic);
        }
    }

    public override IEnumerator GetAnimation(Component image) {
        return AnimationUtility.LerpColor((Graphic)image, targetColor, lerpCurve, lerpSpeed);
    }
}
