using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlatformView : MonoBehaviour
{
    public SpriteRenderer backSpriteRenderer;
    public SpriteRenderer frontSpriteRenderer;
    public EdgeCollider2D edgeCollider2D;
    private readonly float THICKNESS = 0.25f;
    public void Init(Vector2 size, Vector2 position)
    {
        InitAnimation1(size, position);
    }
    private void InitAnimation1(Vector2 size, Vector2 position)
    {
        backSpriteRenderer.size = size;
        frontSpriteRenderer.size = size - Vector2.one * THICKNESS;
        transform.localPosition = position;
        List<Vector2> points = new List<Vector2>() {
            new Vector2(-size.x / 2, size.y / 2),
            new Vector2(size.x / 2, size.y / 2)
        };
        edgeCollider2D.SetPoints(points);
        //transform.DOLocalMoveY(topCenterPosition.y - size.y / 2, 0.5f);
    }
}
