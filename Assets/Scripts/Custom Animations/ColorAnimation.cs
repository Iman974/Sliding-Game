using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New color animation", menuName = "Game/Custom Animations/Color")]
public class ColorAnimation : CustomAnimation {


    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private VectorUtility.Vector4Bool leaveUnchanged;
    [SerializeField] private float cycleOffset;

    private Color unchangedColor;

    public override Type AnimatedComponent {
        get {
            return typeof(Graphic);
        }
    }

    private void OnEnable() {
        unchangedColor = targetColor;
    }

    public override IEnumerator GetAnimation(Component graphic) {
        for (int i = 0; i < 4; i++) { // Great error probability here if the wrong component count is set (in this case > 4)
            if (leaveUnchanged[i]) {
                unchangedColor[i] = ((Graphic)graphic).color[i]; // we need the graphic component, that's why this loop is there
            }
        }

        return AnimationUtility.LerpColor((Graphic)graphic, unchangedColor, animationCurve, animationSpeed, cycleOffset);
    }
}
