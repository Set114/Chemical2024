using UnityEngine;

// 假设 FDGradient 类存在于 YourNamespace 命名空间中
namespace YourNamespace
{
    public class FDGradient : MonoBehaviour
    {
        protected Material GradientMat;

        protected virtual void Start()
        {
            GradientMat = GetComponent<Renderer>().material;
            UpdateGradient();
        }

        protected virtual void UpdateGradient()
        {
            // 在基类中更新渐变的逻辑
        }
    }
}

// FDGradientRadial 类继承自 FDGradient 类
public class FDGradientRadial : FDGradient
{
    public Vector2 Center = new Vector2(0.5f, 0.5f);
    public Vector2 Aspect = Vector2.one;

    protected override void UpdateGradient()
    {
        base.UpdateGradient();

        if (GradientMat != null)
        {
            GradientMat.SetFloat("_CenterX", Center.x);
            GradientMat.SetFloat("_CenterY", Center.y);
            GradientMat.SetFloat("_AspectX", Aspect.x);
            GradientMat.SetFloat("_AspectY", Aspect.y);
        }
    }
}

// 示例使用：将此脚本挂载到一个游戏对象上
public class GradientExample : MonoBehaviour
{
    public FDGradientRadial gradientRadial;

    private void Start()
    {
        // 在场景中创建一个渐变对象
        GameObject gradientObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gradientObject.transform.position = new Vector3(0, 0, 0);
        gradientObject.AddComponent<FDGradientRadial>();

        // 获取渐变对象上的 FDGradientRadial 组件并设置属性
        gradientRadial = gradientObject.GetComponent<FDGradientRadial>();
        gradientRadial.Center = new Vector2(0.5f, 0.5f);
        gradientRadial.Aspect = new Vector2(1f, 1f);
    }
}
