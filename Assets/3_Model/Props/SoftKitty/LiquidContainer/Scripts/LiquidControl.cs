using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoftKitty.LiquidContainer
{

    #region Liqud Data Class
    [System.Serializable]
    public class LiquidData
    {
        [ColorUsage(true, true)]
        public Color _colorTop;
        [ColorUsage(true, true)]
        public Color _colorBottom;
        public float _volumn = 0F;
    }
    #endregion

    public class LiquidControl : MonoBehaviour
    {
        #region Settings
        public bool isMobile = false;
        public bool ShowOpenningHelper = true;
        public bool Opened = false;
        public Vector2 WaterLineOffset = Vector2.zero;
        public MeshFilter LiquidMeshFilter;
        public float WaterLine = 1;
        public float FlowOutSpeed = 1F;
        public Transform ContainerMouth;
        public float OpenningRadius = 0.03F;
        public LayerMask FlowHitMask = 1;
        public float FlowLengthLimit = 1F;
        public bool hasFlowPonding = true;
        public float PondingSize = 2F;
        public bool canRecive = true;
        public float VolumnOfCube = 1F;
        public GameObject CorkModel;
        public List<Transform> FollowSurfaceObjs = new List<Transform>();
        public List<Rigidbody> FloatingRigibodies = new List<Rigidbody>();
        public float FollowSpeed = 0.5F;
        public float FollowWave = 1F;
        #endregion

        #region Internal Variables
        LineRenderer LiquidFlow;
        float size_X;
        float size_Y;
        float size_Z;
        LayerMask MouthMask;
        float flow_size = 0F;
        RaycastHit flow_hit;
        GameObject PondingObj;
        GameObject SprayObj;
        Vector3 prePos;
        Vector3 preRot;
        List<Vector3> _pos = new List<Vector3>();
        [ColorUsage(true, true)]
        Color colorTop;
        [ColorUsage(true, true)]
        Color colorBottom;
        #endregion

        #region Accessable Variables
        [HideInInspector]
        public float Volumn = 0F;
        [HideInInspector]
        public float size_realtime;
        [HideInInspector]
        public List<LiquidData> LiquidDatas = new List<LiquidData>();
        [HideInInspector]
        public GameObject LiquidMaskMesh;
        [HideInInspector]
        public float Velocity = 0F;
        #endregion

        /// ===============================Custom Functions===============================
        #region Custom Functions
        private void DoSomethingWhenHitWaterTrigger(GameObject target) //Add your custom code here, This will be triggered if the flow hit any collider set to Layer 4 (Water Layer) with "IsTrigger" checked.
        {
            //Target is the collider your flow hit.
        }
        #endregion
        /// ===============================Custom Functions===============================


        /// ===============================Useful Accessable Datas===============================
        #region Useful Accessable Datas
        public float GetCurrentTotalVolumn()//Return the total current volumn of the liquid
        {
            return Volumn * WaterLine;
        }

        public float GetCurrentVolumnByColor(Color _colorTop, Color _colorBottom)//Return the current volumn of the liquid with certain colors
        {
            float _result = 0F;
            foreach (LiquidData _data in LiquidDatas)
            {
                if (_data._colorTop == _colorTop && _data._colorBottom == _colorBottom)
                {
                    _result += _data._volumn;
                }
            }
            return _result;
        }
        public int GetTotalLiquidCount()//Return how many liquid has been mixed into this container
        {
            return LiquidDatas.Count;
        }

        public float GetLiquidSurfaceHeight()//Return the liquid surface Y axis position in world space,  This is useful if you want to make something floating on the surface of the liquid
        {
            return LiquidMeshFilter.transform.position.y + size_realtime * (WaterLine - 0.5F);
        }

        public Vector3 GetLiquidSurfaceCenter()//Return the liquid surface center position in world space,  This is useful if you want to make something floating on the surface of the liquid
        {
            Vector3 _up = LiquidMeshFilter.transform.up;
            _up.x = 0F;
            _up.z = 0F;
            _up.Normalize();
            Vector3 _abovePoint= LiquidMeshFilter.transform.position+ Vector3.up * (size_realtime * (WaterLine - 0.5F));
            Vector3 _extendPoint = _abovePoint;
            float _lerp = 1F-Vector3.Angle(LiquidMeshFilter.transform.up, _up) / 90F;
            if (_up.y>0F) {
                _extendPoint= LiquidMeshFilter.transform.position + LiquidMeshFilter.transform.up * (size_realtime * (WaterLine - 0.5F)) / Mathf.Cos(Vector3.Angle(LiquidMeshFilter.transform.up, _up) * Mathf.PI / 180F);
            }
            else
            {
                _extendPoint = LiquidMeshFilter.transform.position - LiquidMeshFilter.transform.up * (size_realtime * (WaterLine - 0.5F)) / Mathf.Cos(Vector3.Angle(LiquidMeshFilter.transform.up, _up) * Mathf.PI / 180F);
            }
            if (Vector3.Distance(_extendPoint, _abovePoint) > Mathf.Max(size_X, size_Z, size_Y))
                _extendPoint = _abovePoint + (_extendPoint - _abovePoint).normalized * Mathf.Max(size_X, size_Z, size_Y);
            return Vector3.Lerp(_abovePoint,_extendPoint, _lerp);
        }

        public bool isReachOpenning()//Return if the the surface of the liquid is above the bottom of the container mouth, Meaning should the liquid flow out of the mouth if the cork is not set.
        {
            return (GetLiquidSurfaceHeight() > ContainerMouth.position.y - OpenningRadius * (1F - ContainerMouth.up.y));
        }

        public bool isCorkSet() // Check if the cork is set to prevent liquid flow out
        {
            if (CorkModel == null) return false;
            return CorkModel.activeSelf && Vector3.Distance(CorkModel.transform.position, ContainerMouth.position) < OpenningRadius;
        }
        #endregion
        /// ===============================Useful Accessable Datas===============================

        /// ===============================Useful Public Funtions===============================
        #region Useful Accessable Datas
        public void SetFlowSpeed(float _speed) //Set the liquid flow out speed, default value is 1
        {
            FlowOutSpeed = Mathf.Max(_speed, 0F);
        }
        public void SetWaterLine(float _amount)//Directly change the percentage amount of the liquid (0-1)
        {
            AddLiquid(Mathf.Clamp(_amount - WaterLine, -WaterLine, 1F - WaterLine), colorTop, colorBottom);
            WaterLine = Mathf.Clamp01(_amount);
        }

        public void FillInLiquid(float _amount, Color _colorTop, Color _colorBottom) // Fill in x amount of liquid with certain colors
        {
            if (WaterLine < 1F)
            {
                WaterLine = Mathf.MoveTowards(WaterLine, 1F, _amount / Volumn);
                colorTop = Color.Lerp(colorTop, _colorTop, _amount / Volumn);
                AddLiquid(_amount, _colorTop, _colorBottom);
            }
        }

        public void AddLiquid(float _volumn, Color _colorTop, Color _colorBottom, bool _addAll = false) //_volumn can be nagetive value
        {
            bool _foundMatch = false;
            float _addedVolumn = 0F;
            foreach (LiquidData _data in LiquidDatas)
            {
                if (_addAll)
                {
                    _data._volumn += _volumn * (_data._volumn / (Volumn * WaterLine));
                    _data._volumn = Mathf.Clamp(_data._volumn, 0F, Volumn);
                }
                else if (_data._colorTop == _colorTop && _data._colorBottom == _colorBottom && !_foundMatch)
                {
                    _foundMatch = true;
                    float _oldVolumn = _data._volumn;
                    _data._volumn += _volumn;
                    _data._volumn = Mathf.Clamp(_data._volumn, 0F, Volumn);
                    _addedVolumn = _data._volumn - _oldVolumn;
                }
            }

            if (!_foundMatch)
            {
                _addedVolumn = Mathf.Clamp(_volumn, 0F, Volumn);
                LiquidDatas.Add(new LiquidData() { _colorTop = _colorTop, _colorBottom = _colorBottom, _volumn = _addedVolumn });
            }

            if (_addedVolumn != 0F)
            {
                Color _newColorTop = Color.black;
                Color _newColorBottom = Color.black;
                foreach (LiquidData _data in LiquidDatas)
                {
                    _newColorTop.r += _data._colorTop.r * _data._volumn / (Volumn * WaterLine);
                    _newColorTop.g += _data._colorTop.g * _data._volumn / (Volumn * WaterLine);
                    _newColorTop.b += _data._colorTop.b * _data._volumn / (Volumn * WaterLine);

                    _newColorBottom.r += _data._colorBottom.r * _data._volumn / (Volumn * WaterLine);
                    _newColorBottom.g += _data._colorBottom.g * _data._volumn / (Volumn * WaterLine);
                    _newColorBottom.b += _data._colorBottom.b * _data._volumn / (Volumn * WaterLine);
                }
                colorTop = _newColorTop;
                colorBottom = _newColorBottom;
                LiquidMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_TopColor", colorTop);
                LiquidMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", colorBottom);
            }
        }

        #endregion
        /// ===============================Useful Public Funtions===============================


        #region Mono Behavious
        void Start()
        {
            Initialized();
            MouthMask |= (1 << 4);
        }

        void Update()
        {
            CheckWaterLine();
            CheckOpen();
            CalVelocity();
            FollowWaterSurface();
        }

        private void FixedUpdate()
        {
            FollowWaterSurfaceRigi();
        }
        #endregion

        #region Initialize
        private void Initialized()
        {
            Volumn = CalVolumn();
            LiquidMeshFilter.GetComponent<MeshRenderer>().material.SetInt("_Stencil", 2);
            if (LiquidMaskMesh == null)
            {
                LiquidMaskMesh = Instantiate(LiquidMeshFilter.gameObject, transform) as GameObject;
                LiquidMaskMesh.name = "LiquidMask(runtime)";
                LiquidMaskMesh.GetComponent<MeshRenderer>().material = Resources.Load<Material>("LiquidContainer/LiquidMask");
            }
            colorTop = LiquidMeshFilter.GetComponent<MeshRenderer>().material.GetColor("_TopColor");
            colorBottom = LiquidMeshFilter.GetComponent<MeshRenderer>().material.GetColor("_BottomColor");
            foreach (ParticleSystemRenderer obj in GetComponentsInChildren<ParticleSystemRenderer>(true))
            {
                if (obj.material.shader.name.Contains("LiquidParticle")) obj.material.SetInt("_Stencil", 2);
            }
            LiquidDatas.Add(new LiquidData() { _colorTop = colorTop, _colorBottom = colorBottom, _volumn = Volumn * WaterLine });
            foreach (Transform obj in FollowSurfaceObjs)
            {
                obj.SetParent(null);
            }
        }
        #endregion

        #region Internal Calculation
        private Vector3 FindFlowHitPoint(Vector3 _pos2)
        {
            if (Physics.Raycast(_pos2, Vector3.down, out flow_hit, FlowLengthLimit, MouthMask, QueryTriggerInteraction.Collide))
            {
                if (flow_hit.collider.isTrigger && flow_hit.collider.GetComponentInParent<LiquidControl>() && WaterLine > 0F)
                {
                    flow_hit.collider.GetComponentInParent<LiquidControl>().FillInLiquid(Time.deltaTime * 0.1F * FlowOutSpeed * Volumn, colorBottom, colorTop);
                    Vector3 _planePos = flow_hit.collider.transform.position;
                    _planePos.y = flow_hit.point.y;
                    if (Vector3.Distance(flow_hit.point, _planePos) > flow_hit.collider.GetComponent<SphereCollider>().radius * 0.2F)
                    {
                        if (!SprayObj)
                        {
                            SprayObj = Instantiate(Resources.Load<GameObject>("LiquidContainer/Spray"), flow_hit.point, Quaternion.identity) as GameObject;
                        }
                        else
                        {
                            SprayObj.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color.Lerp(colorBottom, Color.white, 0.3F));
                            SprayObj.transform.position = flow_hit.point - Vector3.up * flow_hit.collider.GetComponent<SphereCollider>().radius * 0.7F;
                            SprayObj.transform.forward = flow_hit.normal;
                            if (!SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Play();
                        }

                        Vector3 _midPos = _pos2 - Vector3.up * (flow_hit.distance + 0.02F);
                        _pos.Add(_midPos);
                        _pos.Add(Vector3.Lerp(flow_hit.collider.GetComponentInParent<LiquidControl>().ContainerMouth.position, _midPos, 0.5F));
                        Vector3 _endPos = flow_hit.collider.GetComponentInParent<LiquidControl>().LiquidMeshFilter.transform.position + Vector3.up * (size_realtime-0.07F) * (flow_hit.collider.GetComponentInParent<LiquidControl>().WaterLine - 0.5F);
                        return new Vector3(Mathf.Lerp(_midPos.x, _endPos.x, 0.5F), _endPos.y, Mathf.Lerp(_midPos.z, _endPos.z, 0.5F));
                    }
                    else
                    {
                        if (SprayObj && SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Stop();
                        Vector3 _endPos = _pos2;
                        _endPos.y = flow_hit.collider.GetComponentInParent<LiquidControl>().LiquidMeshFilter.transform.position.y + size_realtime * (flow_hit.collider.GetComponentInParent<LiquidControl>().WaterLine - 0.5F);
                        return _endPos;
                    }
                }
                else
                {
                    DoSomethingWhenHitWaterTrigger(flow_hit.collider.gameObject);
                    if (!SprayObj)
                    {
                        SprayObj = Instantiate(Resources.Load<GameObject>("LiquidContainer/Spray"), flow_hit.point, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        SprayObj.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color.Lerp(colorBottom, Color.white, 0.3F));
                        SprayObj.transform.position = flow_hit.point;
                        SprayObj.transform.forward = flow_hit.normal;
                        if (!SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Play();
                    }
                    Vector3 _endPos = _pos2;
                    _endPos.y = flow_hit.point.y;
                    return _endPos;
                }
            }
            else if (Physics.Raycast(_pos2, Vector3.down, out flow_hit, FlowLengthLimit, FlowHitMask, QueryTriggerInteraction.Ignore) && ContainerMouth.transform.up.y < 0.9F)
            {
                if (!flow_hit.collider.isTrigger)
                {
                    if (hasFlowPonding && flow_hit.normal.y >= 0.9F
                        && Physics.Raycast(_pos2 + Vector3.forward * PondingSize * 0.02F, Vector3.down, flow_hit.distance + 0.2F, FlowHitMask, QueryTriggerInteraction.Ignore)
                        && Physics.Raycast(_pos2 - Vector3.forward * PondingSize * 0.02F, Vector3.down, flow_hit.distance + 0.2F, FlowHitMask, QueryTriggerInteraction.Ignore)
                        && Physics.Raycast(_pos2 + Vector3.right * PondingSize * 0.02F, Vector3.down, flow_hit.distance + 0.2F, FlowHitMask, QueryTriggerInteraction.Ignore)
                        && Physics.Raycast(_pos2 - Vector3.right * PondingSize * 0.02F, Vector3.down, flow_hit.distance + 0.2F, FlowHitMask, QueryTriggerInteraction.Ignore)
                        )
                    {
                        if (!PondingObj)
                        {
                            PondingObj = Instantiate(Resources.Load<GameObject>("LiquidContainer/"+ (isMobile? "PondingMobile" : "Ponding")), flow_hit.point, Quaternion.identity) as GameObject;
                            PondingObj.transform.up = flow_hit.normal;
                            PondingObj.GetComponent<MeshRenderer>().material.SetColor("_TopColor", colorTop);
                            PondingObj.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", colorBottom);
                            PondingObj.GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_Color", Color.Lerp(colorBottom, Color.white, 0.3F));

                        }
                        else
                        {
                            PondingObj.SetActive(true);
                            PondingObj.transform.position = Vector3.Lerp(PondingObj.transform.position, flow_hit.point, Time.deltaTime * 12F);
                            PondingObj.transform.up = flow_hit.normal;
                            PondingObj.transform.localScale = Vector3.Lerp(PondingObj.transform.localScale, Vector3.one * PondingSize, Time.deltaTime * 1F);
                            if (!PondingObj.GetComponentInChildren<ParticleSystem>().isPlaying) PondingObj.GetComponentInChildren<ParticleSystem>().Play();
                        }
                    }
                    else
                    {
                        if (PondingObj)
                        {
                            if (PondingObj.transform.localScale.x > 0F)
                                PondingObj.transform.localScale = Vector3.Lerp(PondingObj.transform.localScale, Vector3.zero, Time.deltaTime * 10F);
                            else
                                PondingObj.SetActive(false);

                            if (PondingObj.GetComponentInChildren<ParticleSystem>().isPlaying) PondingObj.GetComponentInChildren<ParticleSystem>().Stop();
                        }
                    }

                    if (!SprayObj)
                    {
                        SprayObj = Instantiate(Resources.Load<GameObject>("LiquidContainer/Spray"), flow_hit.point, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        SprayObj.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color.Lerp(colorBottom, Color.white, 0.3F));
                        SprayObj.transform.position = flow_hit.point;
                        SprayObj.transform.forward = flow_hit.normal;
                        if (!SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Play();
                    }
                }

                return _pos2 - Vector3.up * flow_hit.distance;
            }
            else
            {
                if (PondingObj)
                {
                    if (PondingObj.transform.localScale.x > 0F)
                        PondingObj.transform.localScale = Vector3.Lerp(PondingObj.transform.localScale, Vector3.zero, Time.deltaTime * 3F);
                    else
                        PondingObj.SetActive(false);

                    if (PondingObj.GetComponentInChildren<ParticleSystem>().isPlaying) PondingObj.GetComponentInChildren<ParticleSystem>().Stop();
                }
                if (SprayObj && SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Stop();
            }
            return _pos2 - Vector3.up * (FlowLengthLimit * 0.7F + 0.3F * FlowLengthLimit * flow_size);
        }
        public float CalSize()
        {
            Volumn = CalVolumn();
            return CalSizeRealTime();
        }
        private void CalVelocity()
        {
            float _velocity = Mathf.Clamp(Vector3.Distance(transform.position, prePos) / Time.deltaTime * 30F + Vector3.Angle(transform.up, preRot) / Time.deltaTime * 0.005F, 0F, 1F);
            if (Velocity < _velocity)
                Velocity = Mathf.Lerp(Velocity, _velocity, Time.deltaTime * 4F);
            else
                Velocity = Mathf.MoveTowards(Velocity, _velocity, Time.deltaTime * 0.6F);

            LiquidMeshFilter.GetComponent<MeshRenderer>().material.SetFloat("_WaveIntensity", Velocity);
            prePos = transform.position;
            preRot = transform.up;
        }
        private float CalVolumn()
        {
            Vector3 TopPoint = Vector3.zero;
            Vector3 BottomPoint = Vector3.zero;
            Vector3 LeftPoint = Vector3.zero;
            Vector3 RightPoint = Vector3.zero;
            Vector3 FrountPoint = Vector3.zero;
            Vector3 BackPoint = Vector3.zero;
            Matrix4x4 LocaltoWorld = LiquidMeshFilter.transform.localToWorldMatrix;
            foreach (Vector3 vertex in LiquidMeshFilter.sharedMesh.vertices)
            {
                if (vertex.y > TopPoint.y)
                {
                    TopPoint = vertex;
                }
                else if (vertex.y < BottomPoint.y)
                {
                    BottomPoint = vertex;
                }
                if (vertex.x > RightPoint.x)
                {
                    RightPoint = vertex;
                }
                else if (vertex.x < LeftPoint.x)
                {
                    LeftPoint = vertex;
                }
                if (vertex.z > FrountPoint.z)
                {
                    FrountPoint = vertex;
                }
                else if (vertex.z < BackPoint.z)
                {
                    BackPoint = vertex;
                }
            }
            size_X = Vector3.Distance(LocaltoWorld.MultiplyPoint3x4(RightPoint), LocaltoWorld.MultiplyPoint3x4(LeftPoint));
            size_Y = Vector3.Distance(LocaltoWorld.MultiplyPoint3x4(TopPoint), LocaltoWorld.MultiplyPoint3x4(BottomPoint));
            size_Z = Vector3.Distance(LocaltoWorld.MultiplyPoint3x4(FrountPoint), LocaltoWorld.MultiplyPoint3x4(BackPoint));
            return size_X * size_Y * size_Z * VolumnOfCube;
        }

        private void CheckOpen()
        {
            if (Opened)
            {
                if (LiquidFlow == null)
                {
                    GameObject LiquidObj = Instantiate(Resources.Load<GameObject>("LiquidContainer/"+(isMobile? "FlowMobile" : "Flow")), transform) as GameObject;
                    LiquidObj.transform.localPosition = Vector3.zero;
                    LiquidObj.transform.localEulerAngles = Vector3.zero;
                    LiquidObj.transform.localScale = Vector3.one;
                    LiquidFlow = LiquidObj.GetComponent<LineRenderer>();
                    LiquidFlow.enabled = false;
                }
                else
                {
                    LiquidFlow.material.SetColor("_ColorTop", colorTop);
                    LiquidFlow.material.SetColor("_ColorBottom", colorBottom);
                }

                if (WaterLine > 0F && !isCorkSet())
                {
                    if (ContainerMouth.GetComponent<SphereCollider>())
                    {
                        ContainerMouth.GetComponent<SphereCollider>().radius = OpenningRadius;
                        ContainerMouth.GetComponent<SphereCollider>().enabled = true;
                    }
                    else
                    {
                        SphereCollider sc = ContainerMouth.gameObject.AddComponent<SphereCollider>();
                        ContainerMouth.gameObject.layer = 4;
                        sc.isTrigger = true;
                    }
                    if (isReachOpenning())
                    {
                        flow_size = Mathf.MoveTowards(flow_size, 1F, Time.deltaTime * 4F);
                    }
                    else
                    {
                        flow_size = Mathf.MoveTowards(flow_size, 0F, Time.deltaTime * (isCorkSet() ? 20F : 2F));
                    }

                }
                else
                {
                    flow_size = Mathf.MoveTowards(flow_size, 0F, Time.deltaTime * (isCorkSet() ? 20F : 2F));
                }

                if (flow_size > 0F)
                {
                    LiquidFlow.enabled = true;
                    _pos.Clear();
                    Vector3 _down = Vector3.Cross(Vector3.Cross(ContainerMouth.up, Vector3.down), ContainerMouth.up);
                    _pos.Add(ContainerMouth.position + _down * (OpenningRadius - 0.01F) - ContainerMouth.up * 0.02F);
                    _pos.Add(_pos[0] + Vector3.Lerp(ContainerMouth.up, _down, 0.3F) * 0.04F);
                    _pos.Add(FindFlowHitPoint(_pos[1]));
                    LiquidFlow.positionCount = _pos.Count;
                    LiquidFlow.SetPositions(_pos.ToArray());
                    LiquidFlow.widthMultiplier = flow_size * 0.08F;
                    AddLiquid(-Time.deltaTime * 0.1F * FlowOutSpeed * Volumn, colorTop, colorBottom, true);
                    WaterLine = Mathf.MoveTowards(WaterLine, 0F, Time.deltaTime * 0.1F * FlowOutSpeed);
                }
                else
                {
                    LiquidFlow.enabled = false;
                    if (PondingObj)
                    {
                        if (PondingObj.transform.localScale.x > 0F)
                            PondingObj.transform.localScale = Vector3.Lerp(PondingObj.transform.localScale, Vector3.zero, Time.deltaTime);
                        else
                            PondingObj.SetActive(false);

                        if (PondingObj.GetComponentInChildren<ParticleSystem>().isPlaying) PondingObj.GetComponentInChildren<ParticleSystem>().Stop();
                    }
                    if (SprayObj && SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Stop();
                }
            }
            else
            {
                if (LiquidFlow && flow_size > 0F)
                {
                    flow_size = Mathf.MoveTowards(flow_size, 0F, Time.deltaTime * 20F);
                    LiquidFlow.widthMultiplier = flow_size * 0.08F;
                }
                if (PondingObj)
                {
                    if (PondingObj.transform.localScale.x > 0F)
                        PondingObj.transform.localScale = Vector3.Lerp(PondingObj.transform.localScale, Vector3.zero, Time.deltaTime);
                    else
                        PondingObj.SetActive(false);

                    if (PondingObj.GetComponentInChildren<ParticleSystem>().isPlaying) PondingObj.GetComponentInChildren<ParticleSystem>().Stop();
                }
                if (SprayObj && SprayObj.GetComponent<ParticleSystem>().isPlaying) SprayObj.GetComponent<ParticleSystem>().Stop();
            }
        }

        private float CalSizeRealTime()
        {
            float _x = 1F - (90 - Mathf.Abs(90 - Vector3.Angle(LiquidMeshFilter.transform.right, Vector3.up))) / 90;
            float _y = 1F - (90 - Mathf.Abs(90 - Vector3.Angle(LiquidMeshFilter.transform.up, Vector3.up))) / 90;
            float _z = 1F - (90 - Mathf.Abs(90 - Vector3.Angle(LiquidMeshFilter.transform.forward, Vector3.up))) / 90;

            return size_X * _x + size_Y * _y + size_Z * _z - Mathf.Lerp(WaterLineOffset.x, WaterLineOffset.y, WaterLine);
        }

        private void CheckWaterLine()
        {
            size_realtime = CalSizeRealTime();
            LiquidMeshFilter.GetComponent<MeshRenderer>().material.SetFloat("_Size", size_realtime);
            LiquidMeshFilter.GetComponent<MeshRenderer>().material.SetFloat("_WaterLine", WaterLine);
        }

        private void FollowWaterSurface()
        {
            foreach (Transform obj in FollowSurfaceObjs)
            {
                Vector3 _target = new Vector3(GetLiquidSurfaceCenter().x + Mathf.Sin(Time.time * Velocity*0.55F) * 0.005F * Velocity* FollowWave,
                    GetLiquidSurfaceCenter().y + Mathf.Sin(Time.time * Velocity*0.45F+0.2F) * 0.008F * Velocity * FollowWave,
                    GetLiquidSurfaceCenter().z + Mathf.Sin(Time.time * Velocity*0.5F+0.3F) * 0.005F * Velocity * FollowWave);
                float _dis = Vector3.Distance(new Vector3(obj.position.x, 0F, obj.position.z), new Vector3(GetLiquidSurfaceCenter().x,0F,_target.z))*100F;
                
                float _x = Mathf.MoveTowards(obj.position.x,_target.x, Time.deltaTime * FollowSpeed * 2F* _dis* _dis);
                float _y = Mathf.MoveTowards(obj.position.y,_target.y, (obj.position.y>_target.y+0.02F?Time.deltaTime*1F: Time.deltaTime * (FollowSpeed * 1F * _dis * _dis+0.5F)));
                float _z = Mathf.MoveTowards(obj.position.z,_target.z, Time.deltaTime * FollowSpeed * 2F * _dis * _dis);

                obj.position = new Vector3(_x, _y, _z);
                obj.transform.up = Vector3.Lerp(obj.transform.up,Vector3.up, Time.deltaTime * FollowSpeed * 10F);
            }

        }

        private void FollowWaterSurfaceRigi()
        {
            foreach (Rigidbody obj in FloatingRigibodies)
            {
                float _offset = Mathf.Clamp01(GetLiquidSurfaceHeight() - obj.transform.position.y) * 100F;
                if (obj.transform.position.y < GetLiquidSurfaceHeight())
                {
                    obj.AddForce(Vector3.up * 300F * _offset * Time.fixedDeltaTime, ForceMode.Acceleration);
                }
            }
        }

        #endregion

        #region Draw Helper
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1F, 1F, 0F, 0.2F);
            if (ShowOpenningHelper && ContainerMouth != null)
                Gizmos.DrawSphere(ContainerMouth.position, OpenningRadius);
        }
        #endregion


    }
}