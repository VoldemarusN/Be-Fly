using UnityEngine;
using Zenject;

namespace Traps.TrapsGenerationLogic
{
    public class StaticTrapHandler : IInitializable
    {
        private readonly TrapGenerationSettings _settings;


        public StaticTrapHandler(TrapGenerationSettings settings)
        {
            _settings = settings;
        }
        
        public void Initialize()
        {
            foreach (var settingsStaticTrap in _settings.StaticTraps)
            {
                Vector3 position = new Vector3(settingsStaticTrap.Distance,_settings.StaticTrapHightPosition,0);
                Object.Instantiate(settingsStaticTrap.TrapPrefab, position, Quaternion.identity);
            }
        }
    }
}