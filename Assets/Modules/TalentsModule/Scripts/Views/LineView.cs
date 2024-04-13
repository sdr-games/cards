using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.TalentsModule
{
    public class LineView : Graphic
    {
        [SerializeField] private float _thickness = 10f;

        private Vector2 _endPoint;
        private Vector2 _startPoint;

        public void Initialize(Vector2 startPoint, Vector2 endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            float angle = GetAngle();

            DrawVerticesForPoint(_startPoint, vh, angle);
            DrawVerticesForPoint(_endPoint, vh, angle);

            vh.AddTriangle(0, 1, 3);
            vh.AddTriangle(3, 2, 0);
        }

        private float GetAngle()
        {
            return Mathf.Atan2(_endPoint.y - _startPoint.y, _endPoint.x - _startPoint.x) * (180 / Mathf.PI) + 90f;
        }

        private void DrawVerticesForPoint(Vector2 point, VertexHelper vh, float angle)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-_thickness / 2, 0);
            vertex.position += new Vector3(point.x, point.y);
            vh.AddVert(vertex);

            vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(_thickness / 2, 0);
            vertex.position += new Vector3(point.x, point.y);
            vh.AddVert(vertex);
        }
    }
}
