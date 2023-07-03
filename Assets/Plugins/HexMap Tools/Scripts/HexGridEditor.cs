#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace HexMapTools
{
    public partial class HexGrid
    {

        private partial class HexGridEditor
        {
            private HexGrid hexGrid;

            private bool showScaleGenerator = false;
            private static Vector2 hexSizeInPixels = Vector2.zero;
            private static Vector2 hexOverlappingPixels = Vector2.zero;
            private static Vector2 hexDistPixels = Vector2.zero;


            private bool isBrushEnabled = false;
            private Dictionary<HexCoordinates, GameObject> cells;
            //private GameObject prefab;
            private int selectedIndex = 0;
            private int brushSize = 1;

            private string nameToFind = "";
            private GameObject replaceWith = null;

            private HexCoordinates mouseCoords;

            private void OnEnable()
            {
                hexGrid = (HexGrid)target;
                Undo.undoRedoPerformed += OnUndoRedo;

                
            }

            private void OnDisable()
            {
                Undo.undoRedoPerformed -= OnUndoRedo;
            }

            public override void OnInspectorGUI()
            {
                GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                HexScale hexScale = hexGrid.HexScale;

                EditorGUILayout.LabelField("Objects in grid: " + hexGrid.transform.childCount);

                // ---- Settings ----
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Settings", headerStyle);
                EditorGUILayout.Separator();

                hexGrid.enableSnap = EditorGUILayout.Toggle("Enable snap", hexGrid.enableSnap);
                hexGrid.snapImmediately = EditorGUILayout.Toggle("Snap immediately", hexGrid.snapImmediately);
                hexGrid.createPrefabConnection = EditorGUILayout.Toggle(new GUIContent("Create prefab connection",
                    "If selected, brush creates prefab connections, many connections can strongly slow down editor and undo operation." +
                    "\nIt is best to keep it false for more than 200 objects in grid."),
                    hexGrid.createPrefabConnection);


                EditorGUILayout.Separator();


                EditorGUI.BeginChangeCheck();

                hexScale = ShowHexConfiguration(hexScale);

                if (EditorGUI.EndChangeCheck())
                {
                    hexGrid.HexScale = hexScale;
                }



                EditorGUILayout.Separator();

                if (GUILayout.Button("Snap children"))
                {
                    Undo.RegisterFullObjectHierarchyUndo(hexGrid.gameObject, "Snapped objects to the hex grid");
                    hexGrid.SnapAll();
                }

                if (GUILayout.Button("Remove duplicates"))
                {
                    //Undo.RegisterFullObjectHierarchyUndo(hexGrid.gameObject, "Hex grid, removed duplicates");
                    RemoveDuplicates();
                }

                // ---- Brush ----
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Brush", headerStyle);
                EditorGUILayout.Separator();

                EditorGUI.BeginChangeCheck();
                isBrushEnabled = GUILayout.Toggle(isBrushEnabled, "Enable brush", GUI.skin.FindStyle("Button"));
                if (EditorGUI.EndChangeCheck() && isBrushEnabled)
                {
                    FillCells();
                }

                

                brushSize = EditorGUILayout.IntField("Brush size", brushSize);
                if (brushSize <= 0)
                    brushSize = 1;
                else if (brushSize > 10)
                    brushSize = 10;
          

                // ---- Palette ----
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Palette", headerStyle);

                if (hexGrid.palette == null || hexGrid.palette.Count == 0)
                {
                    hexGrid.palette = new List<GameObject>();
                    hexGrid.palette.Add(null);
                }

                var palette = hexGrid.palette;


                for (int i = 0; i < palette.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();

                    bool isSelected = GUILayout.Toggle(selectedIndex == i, "Slot " + (i+1), GUI.skin.FindStyle("Button"));
                    if(isSelected)
                    {
                        selectedIndex = i;
                    }

                    palette[i] = (GameObject)EditorGUILayout.ObjectField(palette[i], typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Remove"))
                {
                    if (palette.Count > 1)
                        palette.RemoveAt(palette.Count - 1);

                    if (selectedIndex >= palette.Count)
                        selectedIndex = palette.Count - 1;
                }
                if(GUILayout.Button("Add"))
                {
                    palette.Add(null);
                }
                EditorGUILayout.EndHorizontal();


                // ---- Replace ----
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Replace", headerStyle);
                EditorGUILayout.Separator();

                nameToFind = EditorGUILayout.TextField("Name to find", nameToFind);
                replaceWith = (GameObject)EditorGUILayout.ObjectField("Replace with", replaceWith, typeof(GameObject), true);

                if(GUILayout.Button("Replace"))
                {
                    Transform[] transforms = hexGrid.transform.Cast<Transform>().ToArray();

                    Debug.Log("Replace: " + transforms.Length);

                    foreach (Transform t in transforms)
                    {
                        if (t == null || t.name != nameToFind)
                            continue;

                        HexCoordinates coords = hexGrid.HexCalculator.HexFromLocalPosition(t.localPosition);

                        //DestroyImmediate(t.gameObject);
                        Undo.DestroyObjectImmediate(t.gameObject);

                        if (replaceWith != null)
                        {
                            CreateObject(replaceWith, coords);
                        }
                    }
                }


                // ---- Mouse coordinates ----
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Mouse hex coordinates", headerStyle);
                EditorGUILayout.Separator();

                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(string.Format("Axial: X={0}, Y={1}, Z={2}", mouseCoords.X, mouseCoords.Y, mouseCoords.Z));
                EditorGUILayout.LabelField(string.Format("Offset: Col={0}, Row={1}", mouseCoords.Col, mouseCoords.Row));
                EditorGUI.indentLevel--;

            }

            private void OnSceneGUI()
            {
                Event e = Event.current;


                /* -----------------------------------------
                 * Code by: Redwraith
                 * Thanks for fixing brush to work in 3d view
                 */
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                var plane = new Plane(hexGrid.transform.rotation * Vector3.back, hexGrid.transform.position);
                float distance;
                plane.Raycast(ray, out distance);
                Vector3 pos = ray.GetPoint(distance);
                /*----------------------------------------- */


                HexCoordinates mouseCoords = hexGrid.HexCalculator.HexFromPosition(pos);
                this.mouseCoords = mouseCoords;

                Repaint();

                if (!isBrushEnabled)
                    return;


                Tools.current = Tool.None;

                if (Event.current.type == EventType.Layout)
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                if (e.type == EventType.KeyDown)
                {
                    int key = (int)e.keyCode - (int)KeyCode.Alpha1;
                    
                    if (key >= 0 && key < 9 && key < hexGrid.palette.Count)
                    {
                        selectedIndex = key;
                        e.Use();
                    }
                    Repaint();
                }


                var region = HexUtility.GetInRange(mouseCoords, brushSize - 1);

                foreach(var coords in region)
                {
                    hexGrid.DrawHexOutline(coords);
                }

                

                GameObject selectedPrefab = null;

                if (hexGrid.palette != null && selectedIndex >= 0 && selectedIndex < hexGrid.palette.Count)
                {
                    selectedPrefab = hexGrid.palette[selectedIndex];
                }

                if (e.isMouse && e.button == 0 && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown))
                {
                    if (selectedPrefab == null)
                    {
                        foreach (var coords in region)
                        {
                            if (cells.ContainsKey(coords))
                            {
                                GameObject obj = cells[coords];

                                //DestroyImmediate(obj);
                                Undo.DestroyObjectImmediate(obj);

                                cells.Remove(coords);
                            }
                        }
                    }
                    else
                    {
                        foreach (var coords in region)
                        {
                            if(cells.ContainsKey(coords))
                            {
                                if (cells[coords].name == selectedPrefab.name)
                                {
                                    continue;
                                }
                                GameObject prev = cells[coords];
                                //DestroyImmediate(prev);
                                Undo.DestroyObjectImmediate(prev);
                            }

                            cells[coords] = CreateObject(selectedPrefab, coords);
                        }
                    }
                }

                
            }

            GameObject CreateObject(GameObject prefab, HexCoordinates coords)
            {
                GameObject obj;
                if (hexGrid.createPrefabConnection)
                    obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                else
                    obj = Instantiate(prefab);

                obj.name = prefab.name;

                obj.transform.parent = hexGrid.transform;

                obj.transform.localRotation = prefab.transform.localRotation;
                obj.transform.localScale = prefab.transform.localScale;
                obj.transform.localPosition = hexGrid.HexCalculator.HexToLocalPosition(coords);

                Undo.RegisterCreatedObjectUndo(obj, "Hex created");

                return obj;
            }



            public HexScale ShowHexConfiguration(HexScale hexScale)
            {

                Vector2 scale = EditorGUILayout.Vector2Field("Hex scale", hexScale.Scale);
                hexScale = new HexScale(scale);

                Vector2 size = EditorGUILayout.Vector2Field("Hex size", hexScale.Size);
                hexScale = HexScale.FromHexSize(size);

                Vector2 dist = EditorGUILayout.Vector2Field("Hex dist", hexScale.Distance);
                hexScale = HexScale.FromHexDistance(dist);

                EditorGUILayout.Separator();
                showScaleGenerator = EditorGUILayout.Foldout(showScaleGenerator, "Genarate hex scale from pixels", true);

                if (showScaleGenerator)
                {
                    hexScale = ScaleGenerator(hexScale);
                }

                return hexScale;
            }

            private HexScale ScaleGenerator(HexScale hexScale)
            {

                EditorGUI.indentLevel++;

                hexSizeInPixels = EditorGUILayout.Vector2Field(
                    new GUIContent("Hex size", "Hex size in pixels."), hexSizeInPixels);


                hexOverlappingPixels = EditorGUILayout.Vector2Field(
                    new GUIContent("Overlapping pixels", "Overlapping/Common pixels in neighbouring hexes."), hexOverlappingPixels);
                hexDistPixels = new Vector2(hexSizeInPixels.x - hexOverlappingPixels.x,
                    (3f / 4f) * hexSizeInPixels.y - hexOverlappingPixels.y);


                hexDistPixels = EditorGUILayout.Vector2Field(
                    new GUIContent("Distance", "Distance between neighbours from the centre."), hexDistPixels);
                hexOverlappingPixels = new Vector2(hexSizeInPixels.x - hexDistPixels.x,
                        (3f / 4f) * hexSizeInPixels.y - hexDistPixels.y);


                HexScale newHexScale = HexScale.FromPixels(hexSizeInPixels, hexOverlappingPixels);

                EditorGUILayout.Separator();

                GUI.enabled = false;
                EditorGUILayout.Vector2Field("Hex size", newHexScale.Size);
                GUI.enabled = true;


                if (hexSizeInPixels.x <= 0 || hexSizeInPixels.y <= 0)
                    GUI.enabled = false;

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 20);
                if (GUILayout.Button("Generate"))
                {
                    hexScale = newHexScale;
                }
                GUILayout.EndHorizontal();

                GUI.enabled = true;

                EditorGUI.indentLevel--;

                return hexScale;
            }

            private void FillCells()
            {
                cells = new Dictionary<HexCoordinates, GameObject>();

                foreach (Transform t in hexGrid.transform)
                {
                    HexCoordinates coords = hexGrid.HexCalculator.HexFromLocalPosition(t.localPosition);

                    cells[coords] = t.gameObject;
                }
            }

            private void OnUndoRedo()
            {
                if(hexGrid.gameObject.activeSelf)
                    hexGrid.gameObject.SetActive(true);

                if (isBrushEnabled)
                {
                    FillCells();
                }

                
            }

            private void RemoveDuplicates()
            {
                HashSet<HexCoordinates> cells = new HashSet<HexCoordinates>();

                List<GameObject> toRemove = new List<GameObject>();

                foreach (Transform t in hexGrid.transform)
                {
                    HexCoordinates coords = hexGrid.HexCalculator.HexFromLocalPosition(t.localPosition);

                    if (cells.Contains(coords))
                    {
                        toRemove.Add(t.gameObject);
                    }
                    else
                    {
                        cells.Add(coords);
                    }
                }

                foreach (GameObject obj in toRemove)
                {
                    Undo.DestroyObjectImmediate(obj);
                }
            }
        }
    }

}


#endif