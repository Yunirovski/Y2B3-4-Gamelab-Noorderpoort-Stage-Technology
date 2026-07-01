using UnityEngine;
using UnityEngine.UI;

public class FiveScrollbarBounce : MonoBehaviour
{
    public Scrollbar[] scrollbars;
    public float speed = 3f;

    void Update()
    {
        if (scrollbars == null) return;

        for (int i = 0; i < scrollbars.Length; i++)
        {
            if (scrollbars[i] != null)
            {
                scrollbars[i].value = (Mathf.Sin(Time.unscaledTime * speed + i) + 1f) * 0.5f;
            }
        }
    }
}