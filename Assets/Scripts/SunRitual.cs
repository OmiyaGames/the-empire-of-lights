using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using OmiyaGames;

[RequireComponent(typeof(FirstPersonController))]
public class SunRitual : MonoBehaviour
{
    static SunRitual ritual;

    [SerializeField]
    Light directionalLight;
    [SerializeField]
    KeyCode changeLightKey = KeyCode.E;
    [SerializeField]
    float XSensitivity = 2f;
    [SerializeField]
    float YSensitivity = 2f;
    [SerializeField]
    float MinimumX = -90F;
    [SerializeField]
    float MaximumX = 90F;

    FirstPersonController controller;
    MenuManager menus;
    PauseMenu pauseMenu;
    SunMenu sunMenu;
    Quaternion lightRotation;
    bool allowMoving = true;

    public static SunRitual Instance
    {
        get
        {
            return ritual;
        }
    }

    public Transform LightRotation
    {
        get
        {
            return directionalLight.transform;
        }
    }

    // Use this for initialization
    void Start ()
    {
        ritual = this;
        lightRotation = LightRotation.rotation;
        controller = GetComponent<FirstPersonController>();
        menus = Singleton.Get<MenuManager>();
        pauseMenu = menus.GetMenu<PauseMenu>();
        sunMenu = menus.GetMenu<SunMenu>();
    }

    // Update is called once per frame
    void Update ()
    {
        allowMoving = false;
        if (pauseMenu.CurrentState == IMenu.State.Hidden)
        {
            if(Input.GetKey(changeLightKey) == true)
            {
                sunMenu.Show();
                RotateSun();
            }
            else
            {
                sunMenu.Hide();
                allowMoving = true;
            }
        }
        else
        {
            sunMenu.Hide();
        }
        controller.CanMove = allowMoving;
    }

    void RotateSun()
    {
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        lightRotation *= Quaternion.Euler(0f, yRot, 0f);
        lightRotation *= Quaternion.Euler(-xRot, 0f, 0f);

        lightRotation = ClampRotationAroundXAxis(lightRotation);

        LightRotation.rotation = lightRotation;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
