using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public GameObject highlightPrefab;
    private List<GameObject> highlights = new List<GameObject>();

    // ハイライトを生成する
    public GameObject CreateHighlight(Vector3 position)
    {
        GameObject highlight = Instantiate(highlightPrefab, position, Quaternion.identity);
        highlights.Add(highlight);
        return highlight;
    }

    // すべてのハイライトを削除する
    public void DestroyAllHighlights()
    {
        foreach (GameObject highlight in highlights)
        {
            Destroy(highlight);
        }
        highlights.Clear();
    }
}
