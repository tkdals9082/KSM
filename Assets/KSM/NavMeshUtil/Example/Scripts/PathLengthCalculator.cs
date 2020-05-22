namespace KSM.Utility
{
    using System;
    using UnityEngine;
    using UnityEngine.AI;

    public class PathLengthCalculator : MonoBehaviour
    {
        [Tooltip("Ordered waypoints")]
        public Transform[] waypoints;

        [ContextMenu("Calc")]
        private void CalculateDistance()
        {
            float dist = NavMeshUtil.GetShortestDistanceForWaypoints(Array.ConvertAll(waypoints, (tf) => tf.position), 1 << NavMesh.GetAreaFromName("Walkable"));
            Debug.LogError(dist);
        }
    }
}