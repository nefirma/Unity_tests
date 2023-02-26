using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public float baseDamage;
    [HideInInspector]
    public float reloadTime;
    public float baseReloadTime;
    public float reloadTimeMod;
    private bool canShoot = true;
    public float range = 100f; // дальность лазера
    public float lineDuration = 0.5f;
    public string weaponName;
    public GameObject hitPointGlow;
    
    public void Start()
    {
        reloadTime = baseReloadTime;
        //Instantiate hitPointGlow and hide it
        hitPointGlow = Instantiate(hitPointGlow, transform.position, Quaternion.identity);
        hitPointGlow.SetActive(false);
        
    }
    
    public void Shoot(GameObject source, GameObject target)
    {
        // Если пушка не может стрелять, то выходим из функции
        if (!canShoot) return;
        FireAtTarget(source, target.transform);
        var targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            if (targetHealth.currentShield > 1)
            {
                targetHealth.TakeShieldDamage(baseDamage);
            }
            else
            {
                targetHealth.TakeDamage(baseDamage);
            }
        }
        canShoot = false;
        StartCoroutine(Reload());
    }

    private void FireAtTarget(GameObject source, Transform target)
    {
        // вычисляем координаты пушки из координат source
        var spaceshipController = source.GetComponent<SpaceshipController>();
        var positionGun = spaceshipController.weaponPoint1.transform.position;
        // var positionGun = source.transform.position;
        var positionTarget = target.position;
        var delay = 0.5f; //послесвечение точки попадания, зависит от типа поверхности
        
        // Проверяем, что у цели health.shieldOn = true и если да, то вычисляем координаты щита
        var targetHealth = target.GetComponent<Health>();
        if (targetHealth != null && targetHealth.shieldOn)
        {
            var shield = targetHealth.shieldObject;
            if (shield != null)
            {
                // Зная вектор направления от пушки к щиту и радиус щита, можно вычислить координаты точки на щите, в которую попадет лазер
                positionTarget = target.position + (shield.transform.position - target.position).normalized * shield.transform.localScale.x / 2;
            }
        }
        else
        {
            // Если щит не включен, то вычисляем координаты цели
            positionTarget = target.position;
            delay = 1f;
            // Поскольку у корабля есть корпус, то по аналогии с щитом вычисляем координаты точки на корпусе, в которую попадет лазер. SphereCollider у child-объекта корпуса
            var body = target.GetComponentInChildren<SphereCollider>();
            if (body != null)
            {
                positionTarget = target.position + (body.transform.position - target.position).normalized * body.transform.localScale.x / 2;
            }
            
            
        }
        
        // Выводим в консоль состояние цели
        Debug.Log("Target health: " + targetHealth.currentHealth + ", shield: " + targetHealth.currentShield);
        
        // Вычисляем примерные координаты цели, добавляя к ним случайный вектор, чтобы лазер не всегда попадал в центр цели
        var targetPosition = positionTarget + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

        // Вычисляем направление, в котором должен пулять лазер
        var direction = (targetPosition - positionGun).normalized;

        if (Physics.Raycast(positionGun, direction, out var hit, range))
        {
            // Рисуем линию от пушки к цели
            StartCoroutine(DrawLine(positionGun, hit.point, lineDuration, delay));
                    
            // Если щит на цели включен, то подсвечиваем его на 0.5 секунды через корутину
            if (targetHealth != null && targetHealth.shieldOn)
            {
                StartCoroutine(ShieldOn(target.gameObject));
            }
       
        }
        else
        {
            // Рисуем линию от пушки до конца дальности
            var end = positionGun + direction * range;
            StartCoroutine(DrawLine(positionGun, end, lineDuration, delay));
        }
    }
    
    private IEnumerator DrawLine(Vector3 start, Vector3 end, float duration, float delay)
    {
        var line = GetComponent<LineRenderer>();
        if (line == null) {
            line = gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.yellow;
            line.endColor = Color.red;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
        }
        else {
            line.enabled = true;
        }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
        
        //Switch on the hitPointGlow object at the end of the line
        hitPointGlow.transform.position = end;
        StartCoroutine(HitGlow(delay));
        

        yield return new WaitForSeconds(duration);

        line.enabled = false;
    }

    // Coroutine для срабатывания щита на цели
    private IEnumerator ShieldOn(GameObject target)
    {
        var targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.shieldObjectGlow.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        if (targetHealth != null)
        {
            targetHealth.shieldObjectGlow.SetActive(false);
        }
    }
    
    // Coroutine для отрисовки эффекта попадания в цель
    private IEnumerator HitGlow(float delay)
    {
        // Включаем эффект попадания
        hitPointGlow.SetActive(true);
        yield return new WaitForSeconds(delay);
        // Выключаем эффект попадания
        hitPointGlow.SetActive(false);
        
    }    
    
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime * reloadTimeMod);
        canShoot = true;
    }
}