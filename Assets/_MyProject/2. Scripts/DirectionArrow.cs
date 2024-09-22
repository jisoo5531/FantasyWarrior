using UnityEngine;
using UnityEngine.UI;

public class DirectionArrow : MonoBehaviour
{
    public Transform player;       // �÷��̾��� Transform
    public Transform target;       // ��ǥ ������ Transform
    public RectTransform arrowUI;  // ȭ��ǥ �̹����� RectTransform
    private void Start()
    {
        
    }

    void Update()
    {
        // ��ǥ ������ �÷��̾��� ��ġ ���̸� ���ͷ� ���
        Vector3 direction = target.position - player.position;
        direction.y = 0; // ���� ���̸� �����ϰ� ��� ���� ���⸸ ���

        // ���� ���͸� ������ ��ȯ
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // ȭ��ǥ UI�� ������ ���� ȸ��
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
