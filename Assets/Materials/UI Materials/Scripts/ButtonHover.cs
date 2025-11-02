using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverTrigger : MonoBehaviour, IPointerEnterHandler
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("Highlighted");
    }

    public void OnPointerExit(PointerEventData eventData)
{
    anim.SetTrigger("Exit");
}

}
