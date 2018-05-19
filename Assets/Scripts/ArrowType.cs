using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New arrow", menuName = "Game/Arrow")]
public class ArrowType : ScriptableObject {

    [Serializable]
    public class AnimationsHolder {

        [SerializeField] private CustomAnimation[] animations = new CustomAnimation[0];

        public CustomAnimation[] Animations {
            get { return animations; }
        }
        public int Length {
            get { return animations.Length; }
        }

        [SerializeField] private List<float> delays = new List<float>();
        public List<float> Delays {
            get { return delays; }
        }

    }

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

    [SerializeField] private SerializableDictionary_SlideDirection directionBinder;
    public SerializableDictionary_SlideDirection DirectionBinder {
        get { return directionBinder; }
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

    [SerializeField] private AnimationsHolder successAnimations = new AnimationsHolder();
    [SerializeField] private AnimationsHolder failAnimations = new AnimationsHolder();

    [SerializeField] private CustomAnimation[] skipAnimations;
    public CustomAnimation[] SkipAnimations {
        get { return skipAnimations; }
    }

    public AnimationsHolder GetValidationAnimations(bool isValidated) {
        return isValidated ? successAnimations : failAnimations;
    }

    //public float[] GetValidationAnimationsDelays(bool isValidated) {
    //    return isValidated ? successAnimationsDelays : failAnimationsDelays;
    //}

    private void OnEnable() {
        int animationsArraySize = successAnimations.Animations.Length;

        if (animationsArraySize != successAnimations.Delays.Count) {
            ResizeList(successAnimations.Delays, animationsArraySize);
        }

        animationsArraySize = failAnimations.Length;
        if (animationsArraySize != failAnimations.Delays.Count) {
            ResizeList(failAnimations.Delays, animationsArraySize);
        }

        //successAnimations.Delays.Sort();
        //failAnimations.Delays.Sort();
    }

    /// <summary>
    /// Resizes the list until to the wanted size. The empty elements are filled with default values.
    /// </summary>
    /// <param name="size">
    /// The size to be match.
    /// </param>
    private void ResizeList<T>(List<T> listToResize, int size) {
        int countDifference = -listToResize.Count + size;

        if (countDifference > 0) {
            for (int i = 0; i < countDifference; i++) {
                listToResize.Add(default(T));
            }
        } else {
            countDifference = -countDifference;

            for (int i = 0; i < countDifference; i++) {
                listToResize.RemoveAt(listToResize.Count - 1);
            }
        }
    }

    //private void SortListTogether<T, U>(List<T> list1, List<U> list2) where T: IComparable<T> where U : IComparer<U> {
    //    for (int i = 1; i < list1.Count; i++) {
    //        if (list1[i].CompareTo(list1[i - 1]) < 0) {
    //            T temp = list1[i];
    //            list1[i] = list1[i - 1];
    //            list1[i - 1] = temp;
    //        }
    //    }
    //}
}
