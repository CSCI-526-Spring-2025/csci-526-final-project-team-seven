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
    public SpriteRenderer bottomSpriteRenderer;
    public SpriteRenderer leftSpriteRenderer1;
    public SpriteRenderer leftSpriteRenderer2;
    public SpriteRenderer rightSpriteRenderer1;
    public SpriteRenderer rightSpriteRenderer2;
    //private readonly float THICKNESS = 0.25f;
    public void Init(Vector2 size, Vector2 position, int from = 0)
    {
        InitAnimation1(size, position, from);
    }
    public void SetBottomHeight(float h)
    {
        var size=bottomSpriteRenderer.size;
        size.y = h;
        bottomSpriteRenderer.size = size;
    }
    private void InitAnimation1(Vector2 size, Vector2 position, int from = 0)
    {
        backSpriteRenderer.size = size;
        frontSpriteRenderer.size = size;
        transform.localPosition = position;
        List<Vector2> points = new List<Vector2>() {
            new Vector2(-size.x / 2, size.y / 2),
            new Vector2(size.x / 2, size.y / 2)
        };
        edgeCollider2D.SetPoints(points);
        backSpriteRenderer.color = ColorCenter.platformHiddenColor;
        frontSpriteRenderer.color = ColorCenter.platformHiddenColor;
        backSpriteRenderer.DOFade(1, 0.5f);
        frontSpriteRenderer.DOFade(1, 0.5f);

        if (from == 0)
        {
            bottomSpriteRenderer.DOColor(ColorCenter.platformFadeColor, 0.5f);
            leftSpriteRenderer1.color = ColorCenter.platformHiddenColor;
            leftSpriteRenderer2.color = ColorCenter.platformHiddenColor;
            rightSpriteRenderer1.color= ColorCenter.platformHiddenColor;
            rightSpriteRenderer2.color = ColorCenter.platformHiddenColor;
        }
        if (from == 1)
        {
            bottomSpriteRenderer.color = ColorCenter.platformHiddenColor;
            leftSpriteRenderer1.DOColor(ColorCenter.platformFadeColor, 0.5f);
            leftSpriteRenderer2.DOColor(ColorCenter.platformFadeColor, 0.5f);
            rightSpriteRenderer1.color = ColorCenter.platformHiddenColor;
            rightSpriteRenderer2.color = ColorCenter.platformHiddenColor;
        }
        if (from == 2)
        {
            bottomSpriteRenderer.color = ColorCenter.platformHiddenColor;
            leftSpriteRenderer1.color = ColorCenter.platformHiddenColor;
            leftSpriteRenderer2.color = ColorCenter.platformHiddenColor;
            rightSpriteRenderer1.DOColor(ColorCenter.platformFadeColor, 0.5f);
            rightSpriteRenderer2.DOColor(ColorCenter.platformFadeColor, 0.5f);
        }
    }

}
