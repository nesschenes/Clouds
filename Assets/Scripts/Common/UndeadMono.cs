namespace Clouds
{
    public class UndeadMono : MonoSingleton<UndeadMono>
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}