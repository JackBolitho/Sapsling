using UnityEngine;

//created by Ribhu Hooja

// Helps create the trajectory when you pull back a sling
public class TrajectoryHelper : MonoBehaviour {
    [SerializeField] private GameObject trajectoryDotPrefab;
    [SerializeField] private float dotSpacingMultiplier;

    [SerializeField] private int numDots;
    private GameObject[] trajectoryDots;

    void Awake() {
        trajectoryDots = new GameObject[numDots];
        for (int i = 0; i < numDots; i++) {
            trajectoryDots[i] = Instantiate(trajectoryDotPrefab, transform);
        }
    }

    // The way this works is that it computes the trajectory taken by a projectile when
    // launched from the tree (exactly like the seed is), and finds out what place the 
    // projectile will be at several discrete timesteps. This is easy, because the motion
    // is simple uniformly accelerated motion: x(t) = x(0) + ut + (1/2)at^2
    // we just find this for multiple t's, and place a dot there
    public void RecomputeTrajectory(Vector3 position, Vector3 velocity) {
        transform.position = position;

        for (int i = 0; i < trajectoryDots.Length; i++) {
            GameObject trajDot = trajectoryDots[i];
            float t = dotSpacingMultiplier * i;
            trajDot.transform.localPosition = t * (velocity + (Vector3)(0.5f * t * Physics2D.gravity));
        }
    }
}
