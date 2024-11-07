using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SoftKitty.LiquidContainer
{
    [CustomEditor(typeof(LiquidControl))]
    [CanEditMultipleObjects]
    public class LiquidControlEditor : Editor
    {
        SerializedProperty ShowOpenningHelper;
        SerializedProperty ContainerMouth;
        SerializedProperty OpenningRadius;
        SerializedProperty WaterLine;
        SerializedProperty WaterLineOffset;
        SerializedProperty CorkModel;
        SerializedProperty FlowHitMask;
        Texture banner;
        Texture volumnHelp;
        Texture check;
        LiquidControl _script;
        GameObject scriptObject;
        float oldWaterline = 0F;
        Vector2 oldOffset = Vector2.zero;
        bool showHelp = false;
        Transform AddNewFollowObj;
        Rigidbody AddNewRigi;

        void OnEnable()
        {
            _script = (LiquidControl)target;
            scriptObject = _script.gameObject;
            var script = MonoScript.FromScriptableObject(this);
            var path = AssetDatabase.GetAssetPath(script);
            ShowOpenningHelper = serializedObject.FindProperty("ShowOpenningHelper");
            ContainerMouth = serializedObject.FindProperty("ContainerMouth");
            OpenningRadius = serializedObject.FindProperty("OpenningRadius");
            WaterLine = serializedObject.FindProperty("WaterLine");
            WaterLineOffset = serializedObject.FindProperty("WaterLineOffset");
            CorkModel = serializedObject.FindProperty("CorkModel");
            FlowHitMask = serializedObject.FindProperty("FlowHitMask");
            banner = (Texture)AssetDatabase.LoadAssetAtPath(path.Replace("LiquidControlEditor.cs", "Banner.png"), typeof(Texture));
            volumnHelp = (Texture)AssetDatabase.LoadAssetAtPath(path.Replace("LiquidControlEditor.cs", "volumn.png"), typeof(Texture));
            check = (Texture)AssetDatabase.LoadAssetAtPath(path.Replace("LiquidControlEditor.cs", "check.png"), typeof(Texture));
        }

        public void SetScriptDirty()
        {
            if (Application.isPlaying) return;
            UnityEditor.EditorUtility.SetDirty(scriptObject);
            UnityEditor.EditorUtility.SetDirty(_script);
        }

        public override void OnInspectorGUI()
        {
            GUIStyle headStyle = new GUIStyle();
            headStyle.fontSize = 16;
            headStyle.normal.textColor = new Color(0.4F, 0.7F, 0.8F, 1F);
            serializedObject.Update();

            GUILayout.Box(banner);

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("- Liquid settings -", headStyle);

            if (_script.LiquidMeshFilter == null)
            {
                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                if (_script.LiquidMeshFilter != null)
                {
                    GUILayout.Label("Make sure assign SoftKitty/Liquid shader to your liquid mesh first.");
                }
                GUI.color = Color.red;
                GUILayout.Label("Assign the liquid mesh:");
                if (GUILayout.Button("Auto find the liquid mesh"))
                {
                    MeshRenderer[] _renderers = scriptObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer obj in _renderers)
                    {
                        if (obj.sharedMaterial != null)
                        {
                            if (obj.sharedMaterial.shader.name.Contains("SoftKitty/Liquid"))
                            {
                                string path = AssetDatabase.GetAssetPath(obj.GetComponent<MeshFilter>().sharedMesh);
                                ModelImporter A = (ModelImporter)AssetImporter.GetAtPath(path);
                                A.isReadable = true;
                                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                                _script.LiquidMeshFilter = obj.GetComponent<MeshFilter>();
                                _script.LiquidMeshFilter.GetComponent<MeshRenderer>().sharedMaterial.SetInt("_Stencil", 2);
                                _script.LiquidMaskMesh = Instantiate(_script.LiquidMeshFilter.gameObject, scriptObject.transform) as GameObject;
                                _script.LiquidMaskMesh.name = "LiquidMask(DoNotModify)";
                                _script.LiquidMaskMesh.GetComponent<MeshRenderer>().sharedMaterial = Resources.Load<Material>("LiquidContainer/LiquidMask");
                                _script.LiquidMeshFilter.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Size", _script.CalSize());
                                SetScriptDirty();
                            }
                        }
                    }
                }
                GUI.color = Color.white;

            }
            else
            {
                if (oldWaterline != _script.WaterLine)
                {
                    oldWaterline = _script.WaterLine;
                    _script.LiquidMeshFilter.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_WaterLine", oldWaterline);
                }

                GUILayout.BeginHorizontal();
                GUILayout.Box(check);
                GUI.color = new Color(0F, 1F, 0.2F, 1F);
                GUILayout.Label("Liquid Mesh:" + _script.LiquidMeshFilter.gameObject.name);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUI.color = _script.isMobile? Color.white: new Color(0F, 1F, 0.2F, 1F);
                if (GUILayout.Button("Tessellation Shader"))
                {
                    _script.LiquidMeshFilter.GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("SoftKitty/Liquid");
                    _script.isMobile = false;
                    SetScriptDirty();
                }
                GUI.color = _script.isMobile ? new Color(0F, 1F, 0.2F, 1F) : Color.white;
                if (GUILayout.Button("Mobile/VR Shader"))
                {
                    _script.LiquidMeshFilter.GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("SoftKitty/LiquidMobile");
                    _script.isMobile = true;
                    SetScriptDirty();
                }
                GUILayout.EndHorizontal();

                foreach (ParticleSystemRenderer obj in scriptObject.GetComponentsInChildren<ParticleSystemRenderer>(true))
                {
                    if (obj.sharedMaterial.shader.name.Contains("LiquidParticle"))
                    {
                        if (obj.sharedMaterial.GetInt("_Stencil") != 2)
                            obj.sharedMaterial.SetInt("_Stencil", 2);
                    }
                }
                GUI.color = Color.white;

                GUILayout.BeginHorizontal();
                GUILayout.Label("Liquid fill amount:", GUILayout.Width(140));
                WaterLine.floatValue = GUILayout.HorizontalSlider(WaterLine.floatValue, 0F, 1F);
                GUILayout.Label((WaterLine.floatValue * 100F).ToString("0.0") + "% ", GUILayout.Width(60));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                GUILayout.Label("Volumn of cube:", GUILayout.Width(140));
                _script.VolumnOfCube = GUILayout.HorizontalSlider(_script.VolumnOfCube, 0F, 1F);
                GUILayout.Label((_script.VolumnOfCube * 100F).ToString("0") + "% ", GUILayout.Width(60));
                GUILayout.EndHorizontal();

                if (GUILayout.Button("What is this?")) showHelp = !showHelp;
                GUI.color = Color.white;
                if (showHelp) GUILayout.Box(volumnHelp);


                Vector2 _tempOffset = WaterLineOffset.vector2Value;

                GUILayout.BeginHorizontal();
                GUILayout.Label("Empty height offset:", GUILayout.Width(140));
                _tempOffset.x = GUILayout.HorizontalSlider(_tempOffset.x, -0.25F, 0.25F);
                GUILayout.Label( "", GUILayout.Width(60));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Full height offset:", GUILayout.Width(140));
                _tempOffset.y = GUILayout.HorizontalSlider(_tempOffset.y, -0.25F, 0.25F);
                GUILayout.Label("", GUILayout.Width(60));
                GUILayout.EndHorizontal();
              
                WaterLineOffset.vector2Value = _tempOffset;

                if (oldOffset != WaterLineOffset.vector2Value && _script.LiquidMeshFilter != null)
                {
                    _script.LiquidMeshFilter.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Size", _script.CalSize());
                    oldOffset = WaterLineOffset.vector2Value;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label(" ");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                GUILayout.Label("Objects floating on the water surface(Y+ up):", GUILayout.Width(250));
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("___________________________________________", GUILayout.Width(300));
                GUILayout.EndHorizontal();

                for (int i=0;i < _script.FollowSurfaceObjs.Count;i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        _script.FollowSurfaceObjs.RemoveAt(i);
                        SetScriptDirty();
                    }
                    GUI.color = new Color(0.4F, 0.7F, 1F, 1F);
                    GUILayout.Label("[Transform]"+_script.FollowSurfaceObjs[i].name);
                    GUI.color = Color.white;

                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                AddNewFollowObj = (Transform)EditorGUILayout.ObjectField("Select a transform", AddNewFollowObj, typeof(Transform), true);
                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                if (GUILayout.Button("Add"))
                {
                    _script.FollowSurfaceObjs.Add(AddNewFollowObj);
                    SetScriptDirty();
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("___________________________________________", GUILayout.Width(300));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Following Speed:", GUILayout.Width(140));
                _script.FollowSpeed = GUILayout.HorizontalSlider(_script.FollowSpeed, 0.1F, 10F);
                GUILayout.Label((_script.FollowSpeed*10F).ToString("00")+"%", GUILayout.Width(60));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Waving Intensity:", GUILayout.Width(140));
                _script.FollowWave = GUILayout.HorizontalSlider(_script.FollowWave, 0F, 2F);
                GUILayout.Label((_script.FollowWave * 50F).ToString("00") + "%", GUILayout.Width(60));
                GUILayout.EndHorizontal();


                ///=======================
                GUILayout.BeginHorizontal();
                GUILayout.Label(" ");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                GUILayout.Label("Rigibodies floating on the water surface(Y+ up):", GUILayout.Width(250));
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("___________________________________________", GUILayout.Width(300));
                GUILayout.EndHorizontal();

                for (int i = 0; i < _script.FloatingRigibodies.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        _script.FloatingRigibodies.RemoveAt(i);
                        SetScriptDirty();
                    }
                    GUI.color = new Color(0.4F, 1F, 0.2F, 1F);
                    GUILayout.Label("[Rigibody]" + _script.FloatingRigibodies[i].name);
                    GUI.color = Color.white;

                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                AddNewRigi = (Rigidbody)EditorGUILayout.ObjectField("Select a rigibody", AddNewRigi, typeof(Rigidbody), true);
                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                if (GUILayout.Button("Add"))
                {
                    _script.FloatingRigibodies.Add(AddNewRigi);
                    AddNewRigi.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    AddNewRigi.useGravity = true;
                    AddNewRigi.isKinematic = false;
                    AddNewRigi.drag = 15F;
                    AddNewRigi.angularDrag = 5F;
                    SetScriptDirty();
                }
               
                GUILayout.EndHorizontal();

                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Make sure add Rigibody with isKinematic on and a MeshCollider to the container.", GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUI.color = Color.white;

                GUILayout.BeginHorizontal();
                GUILayout.Label("___________________________________________", GUILayout.Width(300));
                GUILayout.EndHorizontal();
            }


            GUILayout.Label(" ");
            GUILayout.EndVertical();

            GUI.color = new Color(1F, 0.7F, 0F, 1F);
            _script.Opened = GUILayout.Toggle(_script.Opened, "Should the liquid be able to flow outside or take liquid from the other containers.");
            GUI.color = Color.white;

            if (_script.Opened)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("- Container mouth settings -", headStyle);


                if (_script.ContainerMouth == null)
                {
                    GUI.color = Color.red;
                    GUILayout.Label("Create a transform for the position of the container mouth:");
                    if (GUILayout.Button("Create container mouth transform"))
                    {
                        GameObject newObj = new GameObject();
                        newObj.name = "ContainerMouth";
                        newObj.transform.SetParent(scriptObject.transform);
                        newObj.transform.localPosition = Vector3.zero;
                        newObj.transform.position += Vector3.up * 0.1F;
                        newObj.transform.localEulerAngles = Vector3.zero;
                        newObj.transform.localScale = Vector3.one;
                        newObj.layer = 4;
                        SphereCollider sc = newObj.AddComponent<SphereCollider>();
                        sc.isTrigger = true;
                        sc.radius = 0.1F;
                        sc.enabled = false;
                        _script.ContainerMouth = newObj.transform;
                        SetScriptDirty();
                    }
                    GUI.color = Color.white;
                }
                else
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Box(check);
                    GUI.color = new Color(0F, 1F, 0.2F, 1F);
                    GUILayout.Label("Container mouth:" + _script.ContainerMouth.gameObject.name);
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();

                    if (ShowOpenningHelper.boolValue)
                    {
                        if (GUILayout.Button("Hide Helper for the container mouth")) ShowOpenningHelper.boolValue = false;
                    }
                    else
                    {
                        if (GUILayout.Button("Show Helper for the container mouth")) ShowOpenningHelper.boolValue = true;

                    }
                    GUILayout.Label(" ");

                    GUI.color = new Color(1F, 0.7F, 0F, 1F);
                    GUILayout.Label("Please maunally adjust the position of the ContainerMouth transform.");
                    GUI.color = Color.white;

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Size:", GUILayout.Width(140));
                    OpenningRadius.floatValue = GUILayout.HorizontalSlider(OpenningRadius.floatValue, 0.005F, 0.1F);
                    GUILayout.Label(OpenningRadius.floatValue.ToString("0.00"), GUILayout.Width(60));
                    GUILayout.EndHorizontal();

                    if (_script.ContainerMouth.GetComponent<SphereCollider>()) _script.ContainerMouth.GetComponent<SphereCollider>().radius = _script.OpenningRadius;

                }

                GUILayout.Label(" ");
                GUILayout.EndVertical();






                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("- Liquid flow settings -", headStyle);



                _script.canRecive = GUILayout.Toggle(_script.canRecive, "Be able to recive liquid from other containers.");

                GUILayout.BeginHorizontal();
                GUILayout.Label("Maxium flow length:", GUILayout.Width(140));
                _script.FlowLengthLimit = GUILayout.HorizontalSlider(_script.FlowLengthLimit, 0.05F, 3F);
                GUILayout.Label(_script.FlowLengthLimit.ToString("0.00") + "m ", GUILayout.Width(60));
                GUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(FlowHitMask);

                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                GUILayout.Label("Flow will be stopped when hit the colliders of the above layermask.");
                GUILayout.Label("Please exclude the [Water Layer](4)");
                GUILayout.Label("Make sure add mesh collider to the containers as well.");
                GUI.color = Color.white;

                GUI.color = (_script.CorkModel == null ? new Color(1F, 0.7F, 0F, 1F) : new Color(0F, 1F, 0.2F, 1F));
                EditorGUILayout.PropertyField(CorkModel);
                GUI.color = Color.white;

                GUI.color = new Color(1F, 0.7F, 0F, 1F);
                GUILayout.Label("When you disactive or move away the cork, liquid will be able to leak out.");
                GUI.color = Color.white;

                _script.hasFlowPonding = GUILayout.Toggle(_script.hasFlowPonding, "Trigger ponding effect when flow hits colliders with the layermask.");

                if (_script.hasFlowPonding)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Ponding size:", GUILayout.Width(140));
                    _script.PondingSize = GUILayout.HorizontalSlider(_script.PondingSize, 0.5F, 5F);
                    GUILayout.Label(_script.PondingSize.ToString("0.0") + "m ", GUILayout.Width(60));
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Liquid flow out speed:", GUILayout.Width(140));
                _script.FlowOutSpeed = GUILayout.HorizontalSlider(_script.FlowOutSpeed, 0F, 2F);
                GUILayout.Label("", GUILayout.Width(60));
                GUILayout.EndHorizontal();
                GUILayout.Label(" ");
                GUILayout.EndVertical();
            }





            serializedObject.ApplyModifiedProperties();
        }
    }
}