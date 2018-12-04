using Clouds.Enums;
using Clouds.Extensions;

namespace Clouds
{
    public class Initialize : MonoSingleton<Initialize>
    {
        protected override void Awake()
        {
            base.Awake();

            SceneTools.LoadSceneAsync(Scenes.LoadUI);
        }
    }
}