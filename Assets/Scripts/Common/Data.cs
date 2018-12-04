using System;

namespace Clouds.Data
{
    [Serializable]
    public struct WeatherData
    {
        public TemperatureData main;
    }

    [Serializable]
    public struct TemperatureData
    {
        public float temp;
    }
}