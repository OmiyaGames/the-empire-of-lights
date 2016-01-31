using UnityEngine;
using OmiyaGames;
using System;

public class SunMenu : IMenu
{
    [SerializeField]
    GameObject defaultUI;
    [SerializeField]
    Transform rotateTransform;
    [SerializeField]
    Transform playerTransform;

    public override GameObject DefaultUi
    {
        get
        {
            return defaultUI;
        }
    }

    public override Type MenuType
    {
        get
        {
            return Type.UnmanagedMenu;
        }
    }

    public override void Show(Action<IMenu> stateChanged)
    {
        base.Show(stateChanged);
        playerTransform.rotation = Quaternion.Euler(0, SunRitual.Instance.transform.rotation.eulerAngles.y, 0);
    }

    void Update ()
    {
        rotateTransform.rotation = SunRitual.Instance.LightRotation.rotation;
    }
}
