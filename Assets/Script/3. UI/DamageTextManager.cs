using UnityEngine;
using TMPro;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }
    public RectTransform canvasRect;
    public Camera mainCamera;
    public TMP_Text damageTextPrefab;
    public float floatUpDistance = 80f;
    public float duration = 1.0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (mainCamera == null)
            mainCamera = Camera.main;
    }



    public void ShowDamage(Vector3 worldPos, int damage, bool isCritical)
    {
        Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, mainCamera, out anchoredPos);


        TMP_Text dmgText = Instantiate(damageTextPrefab, canvasRect);
        dmgText.rectTransform.anchoredPosition = anchoredPos + Vector2.up * 20f; // 데미지 텍스트 보스 위로 조금 올리기


        if (isCritical)
        {          
            dmgText.text = $"<b>{damage}<size=80%><color=#FF2222>치명타!</color></size></b>";
            dmgText.color = new Color(1f, 0.3f, 0.3f); // 치명타는 붉은 글자로
            Camera.main.GetComponent<PlayerCamera>().CriticalZoom();
        }

        else
        {
            dmgText.text = dmgText.text = $"<b>{damage}</b>";
            dmgText.color = Color.white;
        }

        StartCoroutine(FloatAndFade(dmgText));
    }

    private System.Collections.IEnumerator FloatAndFade(TMP_Text dmgText)
    {
        float elapsed = 0f;
        Vector2 start = dmgText.rectTransform.anchoredPosition;
        Vector2 end = start + Vector2.up * floatUpDistance;
        Color startColor = dmgText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            dmgText.rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            dmgText.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            yield return null;
        }
        Destroy(dmgText.gameObject);
    }
}