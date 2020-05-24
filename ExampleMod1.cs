using System.IO;
using System.Reflection;
using GlobalEnums;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExampleMod1
{
    /// <summary>
    /// The main mod class
    /// </summary>
    /// <remarks>This configuration has settings that are save specific</remarks>
    public class ExampleMod1 : Mod
    {



        public static AssetBundle ab, ab2;
        /// <summary>
        /// Represents this Mod's instance.
        /// </summary>
        internal static ExampleMod1 Instance;

        /// <summary>
        /// Fetches the Mod Version From AssemblyInfo.AssemblyVersion
        /// </summary>
        /// <returns>Mod's Version</returns>
        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Called after the class has been constructed.
        /// </summary>
        public override void Initialize()
        {
            //Assign the Instance to the instantiated mod.
            Instance = this;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
            ab = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "scene"));
            ab2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "assetsofbundle"));
            Log("Initializing");


            
            ModHooks.Instance.NewGameHook += NewGame;
            ModHooks.Instance.SavegameLoadHook += SaveGameLoad;



            Log("Initialized");
        }
        public void NewGame() => SaveGameLoad();
        public void SaveGameLoad(int id = -1)
        {
            Log("Game Manager Check: " + GameManager.instance.gameObject.name);
            GameManager.instance.gameObject.AddComponent<SceneLoader>();
        }
        private void OnSceneChanged(Scene from, Scene to)
        {
            Log(from.name + " " + to.name);

            string _scene = to.name;
            if (_scene == "Tutorial_01")
            {
                Log("Maya has detected a Tutorial");
                if (ab == null) Log("Asset 1 not loaded");
                if (ab2 == null) Log("Asset 2 not loaded");
                System.Object[] assets = ab.LoadAllAssets();
                System.Object[] assets2 = ab2.LoadAllAssets();
                string scenePath = ab.GetAllScenePaths()[0];
                Log("ScenePath: " + scenePath);
                //Log("Asset Contains: " + assets);
            }
        }
        private void AddComponent()
        {
            LogDebug("In AddComponent");
            GameManager.instance.gameObject.AddComponent<SceneLoader>();
        }


    }
}
