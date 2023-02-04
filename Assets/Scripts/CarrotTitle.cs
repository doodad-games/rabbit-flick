using UnityEngine;

public class CarrotTitle : MonoBehaviour
{
    public static CarrotTitle I { get; private set; }

    public void OnEnable() =>
        I = this;

    public void OnDisable() =>
        I = null;

    public void Show()
    {
        var prefab = Resources.Load<GameObject>(Constants.Resources.CARROT_PREFAB);

        var tfm = transform;
        for (var i = tfm.childCount - 1; i != -1; --i)
            Instantiate(prefab, tfm.GetChild(i));
    }

    public void Hide()
    {
        foreach (var carrot in GetComponentsInChildren<Carrot>())
            carrot.Destroy();
    }
}
