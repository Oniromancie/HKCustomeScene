using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using HutongGames.PlayMaker.Actions;
using ModCommon;
using ModCommon.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Modding.Logger;
using Modding;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Linq;
using GlobalEnums;
using UnityEngine.Audio;

namespace ExampleMod1
{
    internal class SceneLoader : MonoBehaviour
    {
        string sceneName;
        HeroController _target;

        private IEnumerator Start()
        {
            On.GameManager.EnterHero += GameManager_EnterHero;
            yield return new WaitWhile(() => !HeroController.instance);
            _target = HeroController.instance;
            Log("Start SceneLoader");
            
        }


        private void CreateGateway(string gateName, Vector2 pos, Vector2 size, string toScene, string entryGate, bool right, bool left, bool onlyOut, GameManager.SceneLoadVisualizations vis)
        {
            Log("Entered CreateGateway");
            GameObject gate = new GameObject(gateName);
            gate.transform.SetPosition2D(pos);
            var tp = gate.AddComponent<TransitionPoint>();
            if (!onlyOut)
            {
                var bc = gate.AddComponent<BoxCollider2D>();
                bc.size = size;
                bc.isTrigger = true;
                tp.targetScene = toScene;
                tp.entryPoint = entryGate;
            }
            tp.alwaysEnterLeft = left;
            tp.alwaysEnterRight = right;
            GameObject rm = new GameObject("Hazard Respawn Marker");
            rm.transform.parent = tp.transform;
            rm.transform.position = new Vector2(rm.transform.position.x - 3f, rm.transform.position.y);
            var tmp = rm.AddComponent<HazardRespawnMarker>();
            tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
            tp.sceneLoadVisualization = vis;
        }

        private void GameManager_EnterHero(On.GameManager.orig_EnterHero orig, GameManager self, bool additiveGateSearch)
        {
            Log("EnterHero");
            self.UpdateSceneName();
            if (self.sceneName == "ModScene1")
            {
                sceneName = "ModScene1";
                //From custome scene to Tutorial_01
                CreateGateway("left custScene", new Vector2(-1f, 1.1f), new Vector2(1f, 4f),
                              "Tutorial_01", "left test", true, false, false, GameManager.SceneLoadVisualizations.Default);
               
                orig(self, false);
                return;
            }
            else if (self.sceneName == "Tutorial_01")
            {
                Log("Detect Tutorial 01");
                sceneName = "Tutorial_01";
                CreateGateway("left test", new Vector2(163.6f, 63.4f), new Vector2(1f, 4f),
                              "ModScene1", "left custScene", false, true, false, GameManager.SceneLoadVisualizations.GodsAndGlory);
                Log("Created Gate Detected");
                orig(self, false);
                return;
            }

            orig(self, additiveGateSearch);
        }


        public static void Log(object o)
        {
            Logger.Log("[Area Loader] " + o);
        }
    }
}