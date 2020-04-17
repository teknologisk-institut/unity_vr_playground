using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTransitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Color32 m_NormalColor = Color.white;
    public Color32 m_HoverColor = Color.grey;
    public Color32 m_DownColor = Color.white;
    public bool m_toggled = false;

    private Image m_Image = null;

    void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.color = m_HoverColor;
        gameObject.GetComponentInChildren<Text>().color = m_HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_toggled)
        {
            m_Image.color = m_NormalColor;
            gameObject.GetComponentInChildren<Text>().color = m_NormalColor;
        }
        else 
        {
            m_Image.color = m_DownColor;
            gameObject.GetComponentInChildren<Text>().color = m_DownColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down");
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Up");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click");
        m_toggled = !m_toggled;
        m_Image.color = m_DownColor;
        gameObject.GetComponentInChildren<Text>().color = m_DownColor;
    }
}
