using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeDragMove : MonoBehaviour
{
    bool isInSkillTree = false;
    Vector3 mousePos0 = Vector3.zero;

    [SerializeField] Vector2 maxPos;

    void Update()
    {
        if (!isInSkillTree) return;

        if (Input.GetMouseButtonDown(2))
        {
            mousePos0 = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 newSkillTreePos = (Input.mousePosition - mousePos0) * 0.01f;
            UIManager.instance.SetSkillTreePosition(newSkillTreePos, maxPos);
        }
    }

    public void SetSkillTreeState(bool state)
    {
        isInSkillTree = state;
    }
}
