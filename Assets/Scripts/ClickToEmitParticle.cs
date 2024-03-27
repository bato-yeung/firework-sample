using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToEmitParticle : MonoBehaviour
{
    [SerializeField]
    private float _distanceFromMainCamera = 10f;

    private ParticleSystem _target;
    private float _subEmitterTimer = 0f;

    private const float SubEmitterFireRate = 50f;

    private void Start()
    {
        _target = GetComponent<ParticleSystem>();

        if (_target == null)
        {
            // Force disable component
            enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) == true)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 viewport = Vector3.zero;
            viewport.x = mousePosition.x / Screen.width;
            viewport.y = mousePosition.y / Screen.height;
            viewport.z = _distanceFromMainCamera;

            Debug.Log($"mousePosition: {mousePosition}, Screen.size: ({Screen.width}, {Screen.height}), viewport: {viewport}");

            Vector3 worldPoint = Camera.main.ViewportToWorldPoint(viewport);

            Debug.Log($"worldPoint: {worldPoint}");

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
            {
                startLifetime = CalculateTimeToReachMaxmiumHeight(),
                position = worldPoint + Vector3.down * CalculateProjectileMaxmiumHeight(),
                applyShapeToPosition = true,
            };
            _target.Emit(emitParams, 1);
        }

        if (_target.subEmitters.enabled == true)
        {
            float interval = 1f / SubEmitterFireRate;
            _subEmitterTimer += Time.deltaTime;
            while (_subEmitterTimer >= interval)
            {
                _target.TriggerSubEmitter(0);
                _subEmitterTimer -= interval;
            }
        }
    }

    [ContextMenu("Calculate Projectile Maxmium Height")]
    public float CalculateProjectileMaxmiumHeight()
    {
        float initialVelocity = _target.main.startSpeed.Evaluate(0f);
        float degree = 90f; // upward
        float maxHeight = Mathf.Pow(initialVelocity * Mathf.Sin(Mathf.Deg2Rad * degree), 2f) / (2f * -Physics.gravity.y);
        //Debug.Log($"Projectile Maxmium Height: {maxHeight}");
        return maxHeight;
    }

    [ContextMenu("Calculate Time to Reach Maxmium Height")]
    public float CalculateTimeToReachMaxmiumHeight()
    {
        // t = v/g
        float time = _target.main.startSpeed.Evaluate(0f) / -Physics.gravity.y;
        //Debug.Log($"Time to Reach Maxmium Height: {time}");
        return time;
    }
}
