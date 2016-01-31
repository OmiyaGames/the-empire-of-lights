using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using OmiyaGames;

[RequireComponent(typeof(FirstPersonController))]
public class SunRitual : MonoBehaviour
{
    static SunRitual ritual;

    [Header("Lights")]
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
    float MaximumX = 90f;

    [Header("Pop up")]
    [SerializeField]
    float popUpDuration = 2f;

    FirstPersonController controller;
    MenuManager menus;
    PauseMenu pauseMenu;
    LevelCompleteMenu levelCompleteMenu;
    SunMenu sunMenu;
    Vector3 lightRotationEular;
    bool allowMoving = true;
    WaitForSeconds waitForPopUp;
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
        waitForPopUp = new WaitForSeconds(popUpDuration);
        controller = GetComponent<FirstPersonController>();
        menus = Singleton.Get<MenuManager>();
        pauseMenu = menus.GetMenu<PauseMenu>();
        sunMenu = menus.GetMenu<SunMenu>();
        levelCompleteMenu = menus.GetMenu<LevelCompleteMenu>();
    }

    // Update is called once per frame
    void Update ()
    {
        allowMoving = false;
        if ((pauseMenu.CurrentState == IMenu.State.Hidden) && (levelCompleteMenu.CurrentState == IMenu.State.Hidden))
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

        lightRotationEular = LightRotation.rotation.eulerAngles;
        lightRotationEular.x = ClampedAngle(lightRotationEular.x - xRot);
        lightRotationEular.y += yRot;

        LightRotation.rotation = Quaternion.Euler(lightRotationEular);
    }

    float ClampedAngle(float angle)
    {
        // Get angle to be between -180 and 180
        while(angle > 180)
        {
            angle -= 360f;
        }
        while (angle < -180)
        {
            angle += 360f;
        }
        return Mathf.Clamp(angle, MinimumX, MaximumX);
    }

    public void OnCoinCollectionChanged(CoinCollection collection)
    {
        if(collection.NumCoins > 0)
        {
            StartCoroutine(PopUp(collection.NumCoins, collection.MaxCoins));
        }
        else
        {
            levelCompleteMenu.Show();
        }
    }

    System.Collections.IEnumerator PopUp(int numCoins, int maxCoins)
    {
        ulong id = menus.PopUps.ShowNewDialog("You found " + (maxCoins - numCoins) + " coins out of " + maxCoins);
        yield return waitForPopUp;
        menus.PopUps.RemoveDialog(id);
    }
}
