using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HexMapTools
{

    [ExecuteInEditMode]
    public partial class HexGrid : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private HexCalculator hexCalculator;

        [SerializeField, HideInInspector]
        private bool enableSnap = true;

        [SerializeField, HideInInspector]
        private bool snapImmediately;

        [SerializeField, HideInInspector]
        private bool createPrefabConnection;


        public HexCalculator HexCalculator
        {
            get { return hexCalculator; }
            private set
            {
                hexCalculator = value;
            }
        }

        public HexScale HexScale
        {
            get { return HexCalculator.HexScale; }
            private set
            {
                HexCalculator = new HexCalculator(value, transform);
            }
        }


        private void Start()
        {
            if (hexCalculator == null)
                hexCalculator = new HexCalculator(new HexScale(), transform);
        }

        public void SnapAll()
        {
            foreach (Transform child in transform)
            {
                SnapObject(child);
            }
        }


        public Vector3 SnapPosition(Vector3 pos)
        {
            HexCoordinates coords = hexCalculator.HexFromLocalPosition(pos);

            Vector3 newPos = hexCalculator.HexToLocalPosition(coords);
            newPos.z = pos.z;

            return newPos;
        }

        public void SnapObject(Transform target)
        {
            Vector3 pos = target.localPosition;
            HexCoordinates coords = hexCalculator.HexFromLocalPosition(pos);

            Vector3 newPos = hexCalculator.HexToLocalPosition(coords);
            newPos.z = pos.z;

            target.localPosition = newPos;

        }


        //Snapping in editor
#if UNITY_EDITOR

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }


        //Draw hex outline
        private void DrawHexOutline(Vector3 localPosition)
        {
            HexCoordinates coords = hexCalculator.HexFromLocalPosition(localPosition);

            DrawHexOutline(coords, localPosition.z);
        }

        private void DrawHexOutline(HexCoordinates coords, float z = 0)
        {
            Vector3 newPos = hexCalculator.HexToLocalPosition(coords);
            newPos.z = z;

            Vector2 hexSize = HexScale.Size;

            Vector3[] points = new Vector3[7];
            points[0] = transform.TransformPoint(newPos + new Vector3(0, hexSize.y / 2f, 0));
            points[1] = transform.TransformPoint(newPos + new Vector3(hexSize.x / 2f, hexSize.y / 4f, 0));
            points[2] = transform.TransformPoint(newPos + new Vector3(hexSize.x / 2f, -hexSize.y / 4f, 0));
            points[3] = transform.TransformPoint(newPos + new Vector3(0, -hexSize.y / 2f, 0));
            points[4] = transform.TransformPoint(newPos + new Vector3(-hexSize.x / 2f, -hexSize.y / 4f, 0));
            points[5] = transform.TransformPoint(newPos + new Vector3(-hexSize.x / 2f, hexSize.y / 4f, 0));
            points[6] = transform.TransformPoint(newPos + new Vector3(0, hexSize.y / 2f, 0));

            Handles.DrawPolyLine(points);
        }

        //Draw outlines for hexes, snap on mouse up
        private void OnSceneGUI(SceneView sceneView)
        {
            if (!enableSnap || snapImmediately)
                return;

            if (Event.current.type == EventType.MouseUp)
            {
                foreach (var selected in Selection.transforms)
                {
                    if (selected.parent != transform)
                        continue;

                    SnapObject(selected);
                }

            }



            foreach (var selected in Selection.transforms)
            {
                if (selected.parent != transform)
                    continue;


                DrawHexOutline(selected.localPosition);

            }

            HandleUtility.Repaint();
        }


        //Snap immediately
        private void Update()
        {

            if (!enableSnap || !snapImmediately)
                return;


            foreach (var selected in Selection.transforms)
            {
                if (selected.parent != transform)
                    continue;

                SnapObject(selected);
            }
        }



        //Custom inspector serialized variables
        [SerializeField]
        private List<GameObject> palette;

#endif






        //Custom inspector
#if UNITY_EDITOR

        [CustomEditor(typeof(HexGrid))]
        private partial class HexGridEditor : Editor
        {

        }


#endif


    }



}