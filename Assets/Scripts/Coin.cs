using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    [SerializeField]
    GameObject graphics;
    [SerializeField]
    ParticleSystem fireworks;
    [SerializeField]
    OmiyaGames.SoundEffect sound;

    Collider thisCollider;
    bool isTriggered = false;
    CoinCollection collection;

    public CoinCollection Collection
    {
        get
        {
            return collection;
        }
        set
        {
            collection = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        thisCollider = GetComponent<Collider>();
        thisCollider.isTrigger = true;

        isTriggered = false;
        graphics.SetActive(true);
        fireworks.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            Trigger(SunRitual.Instance);
        }
    }
    public void Trigger(SunRitual ritual)
    {
        if((isTriggered == false) && (Collection != null))
        {
            graphics.SetActive(false);
            //fireworks.Play();
            Collection.RemoveCoin(this);
            isTriggered = true;
            sound.Play();
            ritual.OnCoinCollectionChanged(Collection);
        }
    }
}
