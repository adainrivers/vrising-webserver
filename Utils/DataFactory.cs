using System.Collections.Generic;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using VRising.WebServer.Models;

namespace VRising.WebServer.Utils
{
    internal class DataFactory
    {
        internal static List<UserModel> GetUsers()
        {
            var world = WorldFactory.Server;
            if (world == null)
            {
                return null;
            }

            var prefabSystem = world.GetExistingSystem<PrefabCollectionSystem>();
            var query = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<User>());
            var items = query.ToEntityArray(Allocator.Temp);

            var result = new List<UserModel>();

            foreach (var itemEntity in items)
            {

                var user = world.EntityManager.GetComponentData<User>(itemEntity);
                result.Add(UserModel.FromUser(user));
            }

            return result;
        }

    }
}
