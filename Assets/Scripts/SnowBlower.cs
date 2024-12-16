using System.Collections;
using UnityEngine;

public class SnowBlower : MonoBehaviour
{
    public GameObject snowballPrefab; // ������ ������
    public Transform firePoint; // �����, ������ ����� �������� ������
    public float fireRate = 1f; // �������� �������� (����� ����� ����������)
    public float snowballForce = 10f; // ���� ��������

    private bool canFire = true; // ����, ����������� ��������

    void Update()
    {
        // ���������, ������ �� ������ ��� �������� (��������, ����� ������ ���� ��� ������ �� �����������)
        if (Input.GetButtonDown("Fire1") && canFire)
        {
            FireSnowball();
        }
    }

    void FireSnowball()
    {
        // ������� ������ �� ����� ��������
        GameObject snowball = Instantiate(snowballPrefab, firePoint.position, firePoint.rotation);

        // ��������� ���� � ������
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * snowballForce, ForceMode.VelocityChange);
        }

        // ��������� �������� ��� �����������
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        canFire = false; // ��������� ��������
        yield return new WaitForSeconds(fireRate); // ���� ��������� �����, ����� ��������������
        canFire = true; // ��������� ��������
    }
}
