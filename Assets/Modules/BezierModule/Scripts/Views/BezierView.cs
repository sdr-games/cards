using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SDRGames.Whist.BezierModule.Views
{
    public class BezierView : MonoBehaviour
    {
        public const float BEZIER_SEGMENT_COUNT = 25;

        [SerializeField] private Transform[] _controlPoints;

        private int CurveCount => _controlPoints.Length / 3;

        public Vector2 CalculatePosition(float t, Vector2 firstPoint, Vector2 secondPoint, Vector2 thirdPoint, Vector2 fourthPoint)
        {
            Vector2 firstPointCalculated = Mathf.Pow(1 - t, 3) * firstPoint;
            Vector2 secondPointCalculated = 3 * Mathf.Pow(1 - t, 2) * t * secondPoint;
            Vector2 thirdPointCalculated = 3 * (1 - t) * Mathf.Pow(t, 2) * thirdPoint;
            Vector2 fourthPointCalculated = Mathf.Pow(t, 3) * fourthPoint;

            return firstPointCalculated + secondPointCalculated + thirdPointCalculated + fourthPointCalculated;
        }

        public Vector2 GetControlPointsPosition(float t = 0)
        {
            if (_controlPoints.Length != 1 && _controlPoints.Length % 3 != 1)
            {
                return Vector2.zero;
            }

            for (int i = 0; i < CurveCount; i++)
            {
                int nodeIndex = i * 3;
                return CalculatePosition(t, _controlPoints[nodeIndex].position, _controlPoints[nodeIndex + 1].position, _controlPoints[nodeIndex + 2].position, _controlPoints[nodeIndex + 3].position);
            }
            return Vector2.zero;
        }

        public Vector2[] GetControlPointsPositions()
        {
            if (_controlPoints.Length != 1 && _controlPoints.Length % 3 != 1)
            {
                return null;
            }

            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < CurveCount; i++)
            {
                for (int j = 0; j <= BEZIER_SEGMENT_COUNT; j++)
                {
                    float t = j / BEZIER_SEGMENT_COUNT;
                    int nodeIndex = i * 3;
                    Vector2 position = CalculatePosition(t, _controlPoints[nodeIndex].position, _controlPoints[nodeIndex + 1].position, _controlPoints[nodeIndex + 2].position, _controlPoints[nodeIndex + 3].position);
                    positions.Add(position);
                }
            }
            return positions.ToArray();
        }

        //public void Move

        private void OnDrawGizmos()
        {
            if (_controlPoints.Length != 1 && _controlPoints.Length % 3 != 1)
            {
                return;
            }

            for(int i = 0; i < _controlPoints.Length; i++)
            {
                Gizmos.DrawSphere(_controlPoints[i].position, 5f);
            }

            Vector2[] controlPointsPositions = GetControlPointsPositions();
            for(int i = 0; i < controlPointsPositions.Length; i++)
            {
                Vector2 gizmosPosition = controlPointsPositions[i];
                Gizmos.DrawSphere(gizmosPosition, 2.5f);
            }

            for (int i = 0; i < CurveCount; i++)
            {
                int nodeIndex = i * 3;
                if (nodeIndex % 3 == 0)
                {
                    Gizmos.DrawLine(_controlPoints[i].position, _controlPoints[i + 1].position);
                    Gizmos.DrawLine(_controlPoints[i + 1].position, _controlPoints[i + 2].position);
                    Gizmos.DrawLine(_controlPoints[i + 2].position, _controlPoints[i + 3].position);
                }
            }
        }

        
    }
}
