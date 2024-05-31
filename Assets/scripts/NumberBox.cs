using System;
using System.Collections;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    public int index = 0;
    int x = 0;
    int y = 0;
    float scale = 2.0f; // Scale factor cần đồng bộ với lớp Puzzle
    Vector2 startPos; // Tọa độ ban đầu cần đồng bộ với lớp Puzzle

    private Action<int, int> swapFunc = null;

    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.swapFunc = swapFunc;
        UpdatePox(i, j);
    }

    public void UpdatePox(int i, int j)
    {
        x = i; y = j;
        StartCoroutine(Move());
    }

    public void SetStartPosition(Vector2 startPosition)
    {
        startPos = startPosition;
    }

    IEnumerator Move()
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector2 start = this.gameObject.transform.localPosition;
        Vector2 end = startPos + new Vector2(x * scale, y * scale);

        while (elapsedTime < duration)
        {
            this.gameObject.transform.localPosition = Vector2.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.transform.localPosition = end;
    }

    public bool IsEmpty()
    {
        return index == 16;
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && swapFunc != null)
            swapFunc(x, y);
    }
}
