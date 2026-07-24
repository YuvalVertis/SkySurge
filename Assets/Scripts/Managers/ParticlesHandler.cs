using UnityEngine;

public static class ParticlesHandler
{
    public static void Spawn(GameObject particleObj, Vector3 position, Quaternion rotation = default)
    {
        if(particleObj == null) return;

        if(rotation == default)
        {
            rotation = Quaternion.identity;   
        }
        
        GameObject instance = Object.Instantiate(particleObj, position, rotation);
        ParticleSystem ps = instance.GetComponent<ParticleSystem>();
        if(ps != null)
        {
            ps.Play();
            float lifeDuration = ps.main.duration + ps.main.startLifetime.constantMax;
            Object.Destroy(ps.gameObject, lifeDuration);
        }
    }

    public static void Play(GameObject particleObj)
    {
        if(particleObj == null) return;

        var ps = particleObj.GetComponent<ParticleSystem>();
        if(ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
    }
}