using Unity.Entities;

namespace VRising.WebServer.Utils
{
    public class WorldFactory
    {
        internal static bool AssetsLoaded = false;
        private static World _server;
        public static World Server
        {
            get
            {
                if (!AssetsLoaded)
                {
                    return null;
                }

                if (_server != null)
                {
                    return _server;
                }

                foreach (var world in World.All)
                {
                    if (world.Name != "Server" || !world.IsCreated)
                    {
                        continue;
                    }

                    _server = world;
                    return _server;
                }

                return null;
            }
        }
    }
}
