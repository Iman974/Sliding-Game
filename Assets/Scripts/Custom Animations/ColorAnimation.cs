using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New color animation", menuName = "Game/Custom Animations/Color")]
public class ColorAnimation : CustomAnimation {

    [Serializable]
    private struct Vector4Bool {
        [SerializeField] private bool x;
        [SerializeField] private bool y;
        [SerializeField] private bool z;
        [SerializeField] private bool w;

        public bool X {
            get { return x; }
            set { x = value; }
        }
        public bool Y {
            get { return y; }
            set { y = value; }
        }
        public bool Z {
            get { return z; }
            set { z = value; }
        }
        public bool W {
            get { return w; }
            set { w = value; }
        }

        public bool this[int index] {
            get {
                switch (index) {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("No such component exists.");
                }
            }
        }
    }

    [SerializeField] private AnimationCurve lerpCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private Vector4Bool leaveUnchanged;
    [SerializeField] private float lerpSpeed = 1f;
    //[SerializeField] private float cycleOffset;

    public override Type AnimatedComponent {
        get {
            return typeof(Graphic);
        }
    }

    public ColorAnimation() {
    }

    public override IEnumerator GetAnimation(Component graphic) {
        for (int i = 0; i < 4; i++) {
            if (leaveUnchanged[i]) {
                targetColor[i] = ((Graphic)graphic).color[i];
            }
        }

        return AnimationUtility.LerpColor((Graphic)graphic, targetColor, lerpCurve, lerpSpeed);
    }
}
