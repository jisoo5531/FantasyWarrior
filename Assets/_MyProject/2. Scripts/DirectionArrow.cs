using UnityEngine;
using UnityEngine.UI;

public class DirectionArrow : MonoBehaviour
{
    public Transform player;       // 플레이어의 Transform
    public Transform target;       // 목표 지점의 Transform
    public RectTransform arrowUI;  // 화살표 이미지의 RectTransform
    private void Start()
    {
        
    }

    void Update()
    {
        // 목표 지점과 플레이어의 위치 차이를 벡터로 계산
        Vector3 direction = target.position - player.position;
        direction.y = 0; // 높이 차이를 무시하고 평면 상의 방향만 계산

        // 방향 벡터를 각도로 변환
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // 화살표 UI를 각도에 맞춰 회전
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
