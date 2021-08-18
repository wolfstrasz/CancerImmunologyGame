using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;
using UnityEngine.SceneManagement;

namespace ImmunotherapyGame.Abilities
{
    public class AbilityEffectManager : Singleton<AbilityEffectManager>
    {
        [SerializeField] private List<AbilityEffectPrefab> abilityEffectPrefabs = new List<AbilityEffectPrefab>();
        [SerializeField] private Dictionary<AbilityEffectType, GameObject> prefabs = new Dictionary<AbilityEffectType, GameObject>();
        [SerializeField] private Dictionary<AbilityEffectType, List<AbilityEffect>> aliveEffects = new Dictionary<AbilityEffectType, List<AbilityEffect>>();
        [SerializeField] private Dictionary<AbilityEffectType, List<AbilityEffect>> deadEffects = new Dictionary<AbilityEffectType, List<AbilityEffect>>();

        public void Initialise()
		{
            foreach (AbilityEffectPrefab prefab in abilityEffectPrefabs)
			{
                prefabs.Add(prefab.type, prefab.prefab);
                aliveEffects.Add(prefab.type, new List<AbilityEffect>());
                deadEffects.Add(prefab.type, new List<AbilityEffect>());
			}
		}

		protected void OnEnable()
		{
            SceneManager.activeSceneChanged += OnSceneChangeReset;
		}
        protected void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnSceneChangeReset;
        }

        private void OnSceneChangeReset(Scene id1, Scene id2)
		{
            foreach (var pair in aliveEffects)
			{
                List<AbilityEffect> effects = pair.Value;
                int index = effects.Count - 1;
                for (int i = index; i >= 0; --i)
				{
                    effects[i].gameObject.SetActive(false);
                    deadEffects[effects[i].type].Add(effects[i]);
                }
                effects.Clear();
            }
		}

        internal AbilityEffect GetEffect(Ability ability)
		{
            AbilityEffectType type = ability.effectType;
            
            if (type == null || !prefabs.ContainsKey(type))
			{
                Debug.LogError("Ability (" + ability.name + ") requested an effect of type ( " + type.name + " ) that is not in dictionary of prefabs");
                return null;
			}
            List<AbilityEffect> listToSearch = deadEffects[type];

            if (listToSearch.Count > 0)
			{
                AbilityEffect effectToActivate = listToSearch[listToSearch.Count - 1];
                listToSearch.RemoveAt(listToSearch.Count - 1);
                aliveEffects[type].Add(effectToActivate);
                effectToActivate.gameObject.SetActive(true);
                return effectToActivate;
			} 
            else
			{

                GameObject prefab = prefabs[type];
                AbilityEffect effectToSpawn = Instantiate(prefab, this.transform, false).GetComponent<AbilityEffect>();
                effectToSpawn.type = type;
                aliveEffects[type].Add(effectToSpawn);
                effectToSpawn.gameObject.SetActive(true);

                return effectToSpawn;
			}
		}

        public void OnFixedUpdate()
		{
            foreach (var pair in aliveEffects)
            {
                List<AbilityEffect> aliveEffectList = pair.Value;
                for (int i = 0; i < aliveEffectList.Count; ++i)
				{
                    aliveEffectList[i].OnFixedUpdate();
				}
			}
		}

        internal void OnEffectLifeEnd(AbilityEffect effect)
		{
            if (effect == null)
			{
                Debug.LogError("Null effect requested to be removed");
                return;
			} 
			else if (effect.type == null || !prefabs.ContainsKey(effect.type))
			{
                Debug.LogError("Effect type of " + effect.gameObject + " is null or not in prefab dictionary and cannot be removed");
                return;
            }

            effect.gameObject.SetActive(false);
            List<AbilityEffect> listToSearch = aliveEffects[effect.type];
            int lastIndex = listToSearch.Count - 1;

            if (lastIndex == 0)
			{
                listToSearch.Clear();
			} 
            else
			{
                for (int i = 0; i <= lastIndex; ++i)
                {
                    if (listToSearch[i] == effect)
                    {
                        listToSearch[i] = listToSearch[lastIndex];
                        listToSearch.RemoveAt(lastIndex);
                        break;
                    }
                }
            }
            deadEffects[effect.type].Add(effect);
		}


		[System.Serializable]
        public struct AbilityEffectPrefab
		{
            public AbilityEffectType type;
            public GameObject prefab;
		}


    }
}
