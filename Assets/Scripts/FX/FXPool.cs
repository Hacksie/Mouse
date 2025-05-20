using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class FXPool:AutoSingleton<FXPool>
    {
        [SerializeField] private FX fxPrefab;
        
        private readonly List<FX> fxPool = new();
        //[SerializeField] private List<FX> bloodSplat = new();
        //[SerializeField] private List<FX> hitSplat = new();

        //private readonly Dictionary<int, List<FX>> bloodPool = new();
        //private readonly Dictionary<int, List<FX>> hitPool = new();

        public void Spawn(FXType type, Vector3 position, Vector3 direction)
        {
            var obj = fxPool.FirstOrDefault(x => !x.isActiveAndEnabled);

            if (obj == null)
            {
                obj = Instantiate(fxPrefab, this.transform);
                fxPool.Add(obj);
            }

            obj.transform.position = position;
            obj.transform.rotation.SetLookRotation(direction);

            obj.Spawn(type);
        }

        /*
        private void SpawnAny(ref List<FX> fxPrefabs, Dictionary<int, List<FX>> pool, Vector3 position, Vector3 direction)
        {
            var idx = Random.Range(0, fxPrefabs.Count);
            FX obj = null;

            if (pool.TryGetValue(idx, out var list) && list != null)
            {
                obj = list.FirstOrDefault(x => !x.isActiveAndEnabled);
            }

            if (obj == null)
            {
                obj = Instantiate(fxPrefabs[idx], this.transform);
                if (!pool.ContainsKey(idx))
                {
                    pool.Add(idx, new List<FX>());
                }

                pool[idx].Add(obj);
            }

            obj.transform.position = position;
            obj.transform.rotation.SetLookRotation(direction);
            obj.Spawn();
        }

        public void SpawnBloodSplat(Vector3 position, Vector3 direction)
        {
            SpawnAny(ref bloodSplat, bloodPool, position, direction);
        }

        public void SpawnHitSplash(Vector3 position, Vector3 direction)
        {
            SpawnAny(ref hitSplat, hitPool, position, direction);
        }*/
    }
}
