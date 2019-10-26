using UnityEngine;

public class SwipeEffectSpawner : StateMachineBehaviour {

    [SerializeField] ParticleSystem GFX = null;

    static float halfScreenWorldSizeX;
    static float halfScreenWorldSizeY;
    static System.Collections.Generic.Dictionary<Direction, Vector3> edgesMiddlePoint =
        new System.Collections.Generic.Dictionary<Direction, Vector3>(DirectionUtility.kDirectionCount);

    void OnEnable() {
        // If initialization has not been done yet
        if (edgesMiddlePoint.Count == 0) {
            Camera mainCamera = Camera.main;
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
            Vector3 bottomRight = mainCamera.ViewportToWorldPoint(Vector3.right);
            Vector3 topLeft = mainCamera.ViewportToWorldPoint(Vector3.up);
            halfScreenWorldSizeX = (bottomRight - bottomLeft).magnitude * 0.5f;
            halfScreenWorldSizeY = (topLeft - bottomLeft).magnitude * 0.5f;
            const float kEdgeOffset = 1f;
            edgesMiddlePoint[Direction.Right] = new Vector3(-(halfScreenWorldSizeX + kEdgeOffset), 0f);
            edgesMiddlePoint[Direction.Up] = new Vector3(0f, -(halfScreenWorldSizeY + kEdgeOffset));
            edgesMiddlePoint[Direction.Left] = new Vector3(halfScreenWorldSizeX + kEdgeOffset, 0f);
            edgesMiddlePoint[Direction.Down] = new Vector3(0f, halfScreenWorldSizeY + kEdgeOffset);
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Direction desiredDirection = ArrowManager.DesiredDirection;

        ParticleSystem particleSystem = Instantiate(GFX);
        particleSystem.transform.position = edgesMiddlePoint[desiredDirection];
        particleSystem.transform.up = DirectionUtility.DirectionToVector(desiredDirection);
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        ParticleSystem.MainModule main = particleSystem.main;
        if (desiredDirection == Direction.Down || desiredDirection == Direction.Up) {
            shape.radius = halfScreenWorldSizeX;
            main.startRotationMultiplier = 0f;
        } else {
            shape.radius = halfScreenWorldSizeY;
            main.startRotationMultiplier = Mathf.PI * 0.5f;
        }

    }
}
