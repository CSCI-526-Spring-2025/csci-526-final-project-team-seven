using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Events;

public class CameraZoomAndMove : MonoBehaviour
{
    public static CameraZoomAndMove Instance { get; private set; }
    public CinemachineVirtualCamera vcam;   
    public Vector3 targetPosition;           
    public float targetSize = 3f;           
    public float duration = 2f;               

    private float originalSize;
    private Vector3 originalPosition;
    private void Awake()
    {
        Instance = this;
    }
    public void ZoomAndMove(TweenCallback action = null)
    {
        originalSize = vcam.m_Lens.OrthographicSize;
        originalPosition = vcam.transform.position;

        Time.timeScale = 0f;
        targetPosition = PlayerManager.Instance.playerView.transform.position + new Vector3(4, 0, 0);
        targetPosition.z = -10;

        DOTween.To(() => vcam.m_Lens.OrthographicSize,
                   x => vcam.m_Lens.OrthographicSize = x,
                   targetSize,
                   duration)
               .SetEase(Ease.OutQuad).SetUpdate(true);


        vcam.transform.DOMove(targetPosition, duration)
                      .SetEase(Ease.OutQuad).SetUpdate(true).onComplete += action;
    }

    public void ResetCamera()
    {
        DOTween.To(() => vcam.m_Lens.OrthographicSize,
                   x => vcam.m_Lens.OrthographicSize = x,
                   originalSize,
                   duration)
               .SetEase(Ease.OutQuad).SetUpdate(true);

        vcam.transform.DOMove(originalPosition, duration)
                      .SetEase(Ease.OutQuad).SetUpdate(true).onComplete += () =>
                      {
                          Time.timeScale = 1f;
                      };
    }
}
