using System;
using Steamworks;
using UnityEngine;
using Zenject;

namespace SDK.Steam
{

    public class SteamIntegration : ITickable, IDisposable
    {
        private const uint Appid = 3118310;

        public SteamIntegration()
        {
            try
            {
                SteamClient.Init(Appid, true);
                Debug.Log("Steam is working");
            }
            catch (Exception e)
            {
                Console.WriteLine("Steam error: " + e.ToString());
                throw;
            }

            // Печатаем базовую информацию
            Debug.Log("My Steam ID: " + SteamClient.SteamId);
            Debug.Log("My Steam Username: " + SteamClient.Name);
        }


        public void Tick()
        {
            SteamClient.RunCallbacks();
        }

        public void Dispose()
        {
            SteamClient.Shutdown();
        }
    }
}