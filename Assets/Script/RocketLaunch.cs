using UnityEngine;
using System.Collections;

public class RocketLaunch : MonoBehaviour
{
    public Transform rocketPart;
    public GameObject explosionEffect;

    public float flySpeed = 3f;
    public float flyHeight = 5f;
    public float destroyAfterSeconds = 1.5f;

    private bool isFlying = false;
    private bool launched = false;
    private Vector3 rocketStartLocalPos;

    void Start()
    {
        if (rocketPart != null)
            rocketStartLocalPos = rocketPart.localPosition;

        if (explosionEffect != null)
            explosionEffect.SetActive(false);
    }

    void Update()
    {
        if (isFlying && rocketPart != null)
        {
            rocketPart.localPosition += Vector3.up * flySpeed * Time.deltaTime;

            if (rocketPart.localPosition.y >= rocketStartLocalPos.y + flyHeight)
            {
                isFlying = false;
                StartCoroutine(ExplodeAndDestroy());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!launched && other.CompareTag("Player"))
        {
            launched = true;
            isFlying = true;
        }
    }

    IEnumerator ExplodeAndDestroy()
    {
        if (rocketPart != null)
            rocketPart.gameObject.SetActive(false);

        if (explosionEffect != null)
            explosionEffect.SetActive(true);

        yield return new WaitForSeconds(destroyAfterSeconds);

        Destroy(gameObject);
    }
}