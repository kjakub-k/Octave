using Discord;
using System;
using System.Collections;
using UnityEngine;
namespace KJakub.Octave.Managers.DiscordManager
{
    public class DiscordController : MonoBehaviour
    {
        private Discord.Discord discord;
        private long clientId = 1503420000562839784;
        private bool isDiscordRunning = false;
        [SerializeField]
        private float reconnectInterval = 10f;
        private Coroutine connectionCoroutine;
        private void Start()
        {
            connectionCoroutine = StartCoroutine(TryConnectDiscord());
        }
        private IEnumerator TryConnectDiscord()
        {
            while (!isDiscordRunning)
            {
                try
                {
                    discord = new Discord.Discord(clientId, (ulong)CreateFlags.NoRequireDiscord);
                    isDiscordRunning = true;
                    UpdateActivity();
                }
                catch
                {
                    discord = null;
                }

                yield return new WaitForSeconds(reconnectInterval);
            }
        }
        private void Update()
        {
            if (!isDiscordRunning || discord == null)
                return;

            try
            {
                discord.RunCallbacks();
            }
            catch
            {
                ResetDiscordConnection();
            }
        }
        public void UpdateActivity()
        {
            if (!isDiscordRunning || discord == null) 
                return;

            var activityManager = discord.GetActivityManager();
            var activity = new Activity
            {
                Assets = {
                    LargeImage = "icon",
                    LargeText = "Octave"
                },
                Timestamps = {
                    Start = DateTimeOffset.Now.ToUnixTimeSeconds()
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {

            });
        }
        private void ResetDiscordConnection()
        {
            isDiscordRunning = false;

            if (discord != null)
            {
                try 
                { 
                    discord.Dispose();
                } catch 
                { 

                }

                discord = null;
            }

            if (connectionCoroutine != null) 
                StopCoroutine(connectionCoroutine);

            connectionCoroutine = StartCoroutine(TryConnectDiscord());
        }
        private void OnApplicationQuit()
        {
            if (connectionCoroutine != null) 
                StopCoroutine(connectionCoroutine);

            if (discord != null)
                discord.Dispose();
        }
    }
}