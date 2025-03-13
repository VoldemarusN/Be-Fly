using System;
using System.Collections.Generic;
using System.Linq;
using Plane;
using Plane.PlaneData;
using Traps.Wind;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Traps.TrapsGenerationLogic
{
    public class TrapPatternProvider
    {
        private readonly PlaneView _planeView;
        private readonly TrapGenerationSettings _settings;
        private readonly Dictionary<TrapPatternDifficulty, List<string>> _patternHashes;
        private Dictionary<string, IObjectPool<TrapPattern>> _pools;
        private float _planeThreshold;


        public TrapPatternProvider(PlaneView planeView, TrapGenerationSettings settings,
            TrapPattern[] patterns, Dictionary<Type, ITrapComposer> trapComposers)
        {
            _pools = new Dictionary<string, IObjectPool<TrapPattern>>();
            _patternHashes = new Dictionary<TrapPatternDifficulty, List<string>>();

            _planeView = planeView;
            _settings = settings;
            PreparePatterns(patterns, trapComposers);
        }

        private void PreparePatterns(TrapPattern[] patterns, Dictionary<Type, ITrapComposer> trapComposers)
        {
            foreach (var trapPattern in patterns)
            {
                if (!_patternHashes.ContainsKey(trapPattern.Difficulty))
                    _patternHashes.Add(trapPattern.Difficulty, new List<string>());

                InitializePattern(trapPattern, trapComposers);
                trapPattern.GenerateHash();

                _patternHashes[trapPattern.Difficulty].Add(trapPattern.Hash);
                var linkedPool = new LinkedPool<TrapPattern>(
                    () => TrapCreateFunc(trapPattern, trapComposers),
                    OnPatternGotten, OnPatternReleased);
                linkedPool.Get(out var pattern1);
                linkedPool.Get(out var pattern2);
                linkedPool.Release(pattern1);
                linkedPool.Release(pattern2);
                _pools.Add(trapPattern.Hash, linkedPool);
            }
        }

        private TrapPattern TrapCreateFunc(TrapPattern trapPattern, Dictionary<Type, ITrapComposer> trapComposers)
        {
            TrapPattern duplicate = Object.Instantiate(trapPattern);
            InitializePattern(duplicate, trapComposers);
            return duplicate;
        }

        private void InitializePattern(TrapPattern trapPattern, Dictionary<Type, ITrapComposer> trapComposers)
        {
            trapPattern.InitializeTraps();
            trapPattern.Compose(_planeView.transform, _settings.PlaneOffsetToDestroy);
            trapPattern.ComposeTraps(trapComposers);
        }

        private void OnPatternReleased(TrapPattern pattern)
        {
            pattern.gameObject.SetActive(false);
        }

        private void OnPatternGotten(TrapPattern pattern)
        {
            pattern.Activate();
            pattern.gameObject.SetActive(true);
        }


        public TrapPattern GetPatternByDifficulty(TrapPatternDifficulty difficulty)
        {
            if (_patternHashes.ContainsKey(difficulty))
            {
                string hash = _patternHashes[difficulty][Random.Range(0, _patternHashes[difficulty].Count)];
                IObjectPool<TrapPattern> objectPool = _pools[hash];
                return objectPool.Get();
            }

            return null;
        }

        public void ReleasePattern(TrapPattern trapPattern)
        {
            _pools[trapPattern.Hash].Release(trapPattern);
        }
    }
}