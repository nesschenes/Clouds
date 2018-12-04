using Clouds.Data;
using Clouds.Events;
using Clouds.Extensions;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Clouds.Game
{
    public class GameMain : MonoSingleton<GameMain>
    {
        [SerializeField]
        Transform m_Island = null;

        LocationServiceExt mLocationService = null;

        public float Temperature { get; private set; }

        IEnumerator Start()
        {
            yield return InitSceneObjects_co();

            yield return ParseLocation_co();

            yield return ParseWeathermap_co();
        }

        IEnumerator ParseLocation_co()
        {
            //Pass true to use mocked Location. Pass false or don't pass anything to use real location
            mLocationService = new LocationServiceExt(true);

            LocationInfoExt locInfo = new LocationInfoExt();
            locInfo.latitude = 25.029968f;
            locInfo.longitude = 121.520057f;
            locInfo.altitude = 7f;
            locInfo.horizontalAccuracy = 5.0f;
            locInfo.verticalAccuracy = 5.0f;
            mLocationService.lastData = locInfo;

            // First, check if user has location service enabled
            if (!mLocationService.isEnabledByUser)
                yield break;

            // Start service before querying location
            mLocationService.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (mLocationService.status == LocationServiceStatusExt.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }

            // Connection has failed
            if (mLocationService.status == LocationServiceStatusExt.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                string location = "Location: " + mLocationService.lastData.latitude + " "
                    + mLocationService.lastData.longitude + " " + mLocationService.lastData.altitude
                    + " " + mLocationService.lastData.horizontalAccuracy + " " + mLocationService.lastData.timestamp;
                print(location);
            }

            // Stop service if there is no need to query location updates continuously
            mLocationService.Stop();
        }

        IEnumerator ParseWeathermap_co()
        {
            var appid = "52ac715f4aefe908d6fa6c5b0dad3308";
            var url = string.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid={2}", mLocationService.lastData.latitude, mLocationService.lastData.longitude, appid);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET, new DownloadHandlerBuffer(), null);

            print(url);

            yield return request.SendWebRequest();

            if (request.error != null) yield break;

            print(request.downloadHandler.text);
            var data = request.downloadHandler.text.Deserialize<WeatherData>();
            print(data.main.temp);

            Temperature = data.main.temp;

            GameEvents.OnCloudsRefresh?.Invoke();
        }

        IEnumerator InitSceneObjects_co()
        {
            var prefabs = Resources.LoadAll("Trees/Prefabs", typeof(GameObject));
            var prefabsCount = prefabs.Length;

            for (int i = 0; i < 20; i++)
            {
                var prefab = prefabs[Random.Range(0, prefabsCount)];

                Vector3 spawnPosition = Random.onUnitSphere * (m_Island.localScale.x / 2) + m_Island.position;
                GameObject newCharacter = Instantiate(prefab, spawnPosition, Quaternion.identity, m_Island) as GameObject;
                newCharacter.transform.up = spawnPosition - m_Island.position;

                yield return null;
            }
        }
    }
}