using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    public Vector3 highlightPos;

    void OnMouseDown()
    {
        this.highlightPos = transform.position;
        Debug.Log($"Touched highlight at position: {highlightPos}");

        // 親オブジェクトから駒の管理スクリプトを探す
        Transform parentTransform = transform.parent;
        if (parentTransform != null)
        {
            ChickManager chickManager = parentTransform.GetComponent<ChickManager>();
            if (chickManager != null)
            {
                chickManager.MoveToPosition(this.gameObject, highlightPos);
                return;
            }

            ElephantManager elephantManager = parentTransform.GetComponent<ElephantManager>();
            if (elephantManager != null)
            {
                elephantManager.MoveToPosition(this.gameObject, highlightPos);
                return;
            }

            GiraffeManager giraffeManager = parentTransform.GetComponent<GiraffeManager>();
            if (giraffeManager != null)
            {
                giraffeManager.MoveToPosition(this.gameObject, highlightPos);
                return;
            }

            LionManager lionManager = parentTransform.GetComponent<LionManager>();
            if (lionManager != null)
            {
                lionManager.MoveToPosition(this.gameObject, highlightPos);
                return;
            }
        }

        Debug.LogWarning("No movable component found on the object.");
    }
}
