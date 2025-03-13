using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Traps.TrapsGenerationLogic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class GameEffector : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;

    private IObjectPool<ParticleSystem> _hitEffectPool;
    private TrapPatternHandler _trapPatternHandler;


    [Inject]
    private void Construct(TrapPatternHandler handler)
    {
        _trapPatternHandler = handler;
    }

    private void Start()
    {
        _hitEffectPool = new ObjectPool<ParticleSystem>(CreateNewParticle);
        _trapPatternHandler.OnPatternSpawned += SubscribeOnPatternTraps;
    }

    private ParticleSystem CreateNewParticle()
    {
        return Instantiate(_hitEffect);
    }


    private void OnDestroy()
    {
        _trapPatternHandler.OnPatternSpawned -= SubscribeOnPatternTraps;
    }

    private void SubscribeOnPatternTraps(TrapPattern pattern)
    {
        pattern.OnTrapDestroyedWithPlayer += trap =>
        {
            if (trap is WindSpawner) return;
            _ = PlayHitEffect(trap);
        };
    }


    private async UniTaskVoid PlayHitEffect(BaseTrap baseTrap)
    {
        var hitInstance = _hitEffectPool.Get();
        hitInstance.transform.position = baseTrap.transform.position;
        hitInstance.Play();
        await UniTask.Delay(1000);
        _hitEffectPool.Release(hitInstance);
    }
}