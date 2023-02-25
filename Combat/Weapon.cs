using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public float reloadTime;
    private bool canShoot = true;
    public float range = 100f; // дальность лазера
    public float lineDuration = 0.5f;
    public string weaponName;
    
    public void Shoot(GameObject source, GameObject target)
    {
        // Если пушка не может стрелять, то выходим из функции
        if (!canShoot) return;
        FireAtTarget(source, target.transform);
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            if (targetHealth.currentShield > 5)
            {
                targetHealth.TakeShieldDamage(damage);
            }
            else
            {
                targetHealth.TakeDamage(damage);
            }
        }
        canShoot = false;
        StartCoroutine(Reload());
    }

    private void FireAtTarget(GameObject source, Transform target)
    {
        // вычисляем координаты пушки из координат source
        Vector3 position =  source.transform.position;

        // Вычисляем примерные координаты цели, добавляя к ним случайный вектор, чтобы лазер не всегда попадал в центр цели
        Vector3 targetPosition = target.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

        // Вычисляем направление, в котором должен пулять лазер
        Vector3 direction = (targetPosition - position).normalized;

        if (Physics.Raycast(position, direction, out var hit, range))
        {
            // Рисуем линию от пушки к цели
            StartCoroutine(DrawLine(position, hit.point, lineDuration));
        }
        else
        {
            // Рисуем линию от пушки до конца дальности
            Vector3 end = position + direction * range;
            StartCoroutine(DrawLine(position, end, lineDuration));
        }
    }


    private IEnumerator DrawLine(Vector3 start, Vector3 end, float duration)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        if (line == null) {
            line = gameObject.AddComponent<LineRenderer>();
        }
        else {
            Destroy(line);
            line = gameObject.AddComponent<LineRenderer>();
        }
        
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.yellow;
        line.endColor = Color.red;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(duration);

        Destroy(line);
    }
    
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }
}