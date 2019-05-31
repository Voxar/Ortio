using UnityEngine;

public class Hilight : MonoBehaviour
{
    public Transform target;
    float maxSpeed = 100f;

    public float size = 5f;

    new Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
    }
    private void Update()
    {
        light.enabled = target != null;
    }


    float currentVelocityX = 1f;
    float currentVelocityY = 1f;
    float currentSpotAngleVelocity;
    private void FixedUpdate()
    {
        light.spotAngle = Mathf.SmoothDamp(light.spotAngle, size, ref currentSpotAngleVelocity, 0.1f, maxSpeed, Time.deltaTime);

        if (target == null || target.position.Equals(transform.position)) { return; }
        var x = Mathf.SmoothDamp(transform.position.x, target.position.x, ref currentVelocityX, 0.1f, Constant.defaultDampSpeed, Time.deltaTime);
        var z = Mathf.SmoothDamp(transform.position.z, target.position.z, ref currentVelocityY, 0.1f, Constant.defaultDampSpeed, Time.deltaTime);
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
