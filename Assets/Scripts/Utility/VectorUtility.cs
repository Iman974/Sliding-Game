using UnityEngine;
using System;

public static class VectorUtility {

    [Serializable]
    public struct Vector4Bool {
        [SerializeField] private bool x;
        [SerializeField] private bool y;
        [SerializeField] private bool z;
        [SerializeField] private bool w;

        //public bool X {
        //    get { return x; }
        //    set { x = value; }
        //}
        //public bool Y {
        //    get { return y; }
        //    set { y = value; }
        //}
        //public bool Z {
        //    get { return z; }
        //    set { z = value; }
        //}
        //public bool W {
        //    get { return w; }
        //    set { w = value; }
        //}

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

    [Serializable]
    public struct Vector3Bool {
        [SerializeField] private bool x;
        [SerializeField] private bool y;
        [SerializeField] private bool z;

        //public bool X {
        //    get { return x; }
        //    set { x = value; }
        //}
        //public bool Y {
        //    get { return y; }
        //    set { y = value; }
        //}
        //public bool Z {
        //    get { return z; }
        //    set { z = value; }
        //}
        //public bool W {
        //    get { return w; }
        //    set { w = value; }
        //}

        public bool this[int index] {
            get {
                switch (index) {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException("No such component exists.");
                }
            }
        }
    }
}
