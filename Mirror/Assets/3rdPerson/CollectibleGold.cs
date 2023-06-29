using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollectibleGold : MonoBehaviour
{
    [SerializeField]
    private int _worth = 1;

    private bool collected = false;

    private IEnumerator wasCollected;

    public bool TryCollect(out int worth, Transform whoTriesToCollect)
    {
        if (collected)
        {
            worth = 0;
            return false;
        }

        worth = _worth;
        collected = true;
        wasCollected = WasCollected(whoTriesToCollect);
        StartCoroutine(wasCollected);
        return true;
    }

    IEnumerator WasCollected(Transform byWho)
    {
        DisableRigidbody();
        DisableColliders();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        yield return null;
        float time = 0;
        while (time < 1 || audioSource.isPlaying )
        {
            float delta = Time.deltaTime;
            time += delta;

            transform.position = Vector3.Lerp(transform.position, byWho.position, time);

            transform.localScale = (1 - (delta*3f)) * transform.localScale;
            yield return null;
        }
        Destroy(gameObject);
        yield break;

        void DisableRigidbody()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
                return;
            rigidbody.isKinematic = true;
        }
        void DisableColliders()
        {
            foreach (var collider in GetComponents<Collider>())
                collider.enabled = false;
        }
    }


}
