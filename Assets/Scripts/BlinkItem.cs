using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkItem : MonoBehaviour
{
    private SpriteRenderer m_sprite;

    private void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkItemCoroutine());
    }

    IEnumerator BlinkItemCoroutine()
    {
        while (true)
        {
            m_sprite.enabled = !m_sprite.enabled;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
