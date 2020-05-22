namespace KSM.Utility
{
    using UnityEngine.AI;
    using UnityEngine;

    public static class NavMeshUtil
    {
        public static float GetShortestDistance(Vector3 sourcePosition, Vector3 targetPosition, int areaMask)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(sourcePosition, targetPosition, areaMask, path);

            int len = path.corners.Length;

            float dist = 0;

            for (int i = 0; i < len - 1; ++i)
            {
                dist += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 2);
            }

            return dist;
        }

        public static float GetShortestDistanceForWaypoints(Vector3[] waypoints, int areaMask)
        {
            int len = waypoints.Length;

            float dist = 0;

            for (int i = 0; i < len - 1; ++i)
            {
                dist += GetShortestDistance(waypoints[i], waypoints[i + 1], areaMask);
            }

            return dist;
        }
    }
}