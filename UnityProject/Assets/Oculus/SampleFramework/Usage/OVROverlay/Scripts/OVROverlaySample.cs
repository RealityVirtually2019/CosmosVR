/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace OculusSampleFramework
{

    /// <summary>
    /// The rendering methods swappable via radio buttons
    /// </summary>
    public enum EUiDisplayType
    {
        EUDT_WorldGeoQuad,
        EUDT_OverlayQuad,
        EUDT_None,
        EUDT_MaxDislayTypes
    }

    /// <summary>
    /// Usage: demonstrate how to use overlay layers for a paneled UI system
    /// On Mobile, we support both Cylinder layer and Quad layer
    /// Press any button: it will cycle  [world geometry Quad]->[overlay layer Quad]->[world geometry cylinder]->[overlay layer cylinder]
    /// On PC, only Quad layer is supported
    /// Press any button: it will cycle  [world geometry Quad]->[overlay layer Quad]
    /// 
    /// You should be able to observe sharper and less aliased image when switch from world geometry to overlay layer.
    /// 
    /// </summary>
    public class OVROverlaySample : MonoBehaviour
    {

        bool inMenu;

        /// <summary>
        /// The string identifiers for DebugUI radio buttons
        /// </summary>
        const string ovrOverlayID = "OVROverlayID";
        const string applicationID = "ApplicationID";
        const string noneID = "NoneID";

        /// <summary>
        /// Toggle references
        /// </summary>
        Toggle overlayRadioButton;
        Toggle applicationRadioButton;
        Toggle noneRadioButton;
        
        [Header("App vs Compositor Comparison Settings")]
        /// <summary>
        /// The main camera used to calculate reprojected OVROverlay quad
        /// </summary>
        public GameObject mainCamera;

        /// <summary>
        /// The camera used to render UI panels
        /// </summary>
        public GameObject uiCamera;
        
        /// <summary>
        /// The parents of grouped UI panels
        /// </summary>
        public GameObject uiGeoParent;
        public GameObject worldspaceGeoParent;

        /// <summary>
        /// The OVROverlay component to pass the uiCamera rendered RT to
        /// </summary>
        public OVROverlay cameraRenderOverlay;

        /// <summary>
        /// The OVROverlay component displaying which rendering mode is active
        /// </summary>
        public OVROverlay renderingLabelOverlay;

        /// <summary>
        /// The quad textures to indicate the active rendering method
        /// </summary>
        public Texture applicationLabelTexture;
        public Texture compositorLabelTexture;

        /// <summary>
        /// Indicate current ui display type
        /// </summary>
        EUiDisplayType desiredUiType = EUiDisplayType.EUDT_OverlayQuad;

        [Header("Level Loading Sim Settings")]
        public GameObject prefabForLevelLoadSim;
        public OVROverlay cubemapOverlay;
        public OVROverlay loadingTextQuadOverlay;
        public float distanceFromCamToLoadText;
        public float cubeSpawnRadius;
        public float heightBetweenItems;
        public int numObjectsPerLevel;
        public int numLevels;
        List<GameObject> spawnedCubes = new List<GameObject>();
        const int NumLoopsTrigger = 500000000;

        #region MonoBehaviour handler

        void Start()
        {
            DebugUIBuilder.instance.AddLabel("OVROverlay Sample");
            DebugUIBuilder.instance.AddDivider();
            DebugUIBuilder.instance.AddLabel("Level Loading Example");
            DebugUIBuilder.instance.AddButton("Simulate Level Load", TriggerLoad);
            DebugUIBuilder.instance.AddButton("Destroy Cubes", TriggerUnload);
            DebugUIBuilder.instance.AddDivider();
            DebugUIBuilder.instance.AddLabel("OVROverlay vs. Application Render Comparison");
            overlayRadioButton = DebugUIBuilder.instance.AddRadio("OVROverlay", "group", delegate (Toggle t) { RadioPressed(ovrOverlayID, "group", t); }).GetComponentInChildren<Toggle>();
            applicationRadioButton = DebugUIBuilder.instance.AddRadio("Application", "group", delegate (Toggle t) { RadioPressed(applicationID, "group", t); }).GetComponentInChildren<Toggle>();
            noneRadioButton = DebugUIBuilder.instance.AddRadio("None", "group", delegate (Toggle t) { RadioPressed(noneID, "group", t); }).GetComponentInChildren<Toggle>();
        
            DebugUIBuilder.instance.Show();

            // Start with Overlay Quad
            desiredUiType = EUiDisplayType.EUDT_OverlayQuad;
            CameraAndRenderTargetSetup();
            cameraRenderOverlay.enabled = true;
            cameraRenderOverlay.currentOverlayShape = OVROverlay.OverlayShape.Quad;

            /*
            switch(desiredUiType)
            {
                case EUiDisplayType.EUDT_OverlayQuad:
                    ActivateOVROverlay();
                    break;
                case EUiDisplayType.EUDT_WorldGeoQuad:
                    ActivateWorldGeo();
                    break;
                case EUiDisplayType.EUDT_None:
                    ActivateNone();
                    break;
            }
            */
            spawnedCubes.Capacity = numObjectsPerLevel * numLevels;
        }

        void Update()
        {
            // Switch ui display types 
            if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
            {
                if (inMenu) DebugUIBuilder.instance.Hide();
                else DebugUIBuilder.instance.Show();
                inMenu = !inMenu;
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                if(desiredUiType == EUiDisplayType.EUDT_None)
                {
                    desiredUiType = EUiDisplayType.EUDT_WorldGeoQuad;
                }
                else if(desiredUiType == EUiDisplayType.EUDT_OverlayQuad)
                {
                    desiredUiType = EUiDisplayType.EUDT_None;
                }
                else
                {
                    desiredUiType = EUiDisplayType.EUDT_WorldGeoQuad;
                }
                switch(desiredUiType)
                {
                    case EUiDisplayType.EUDT_OverlayQuad:
                        //ActivateOVROverlay();
                        overlayRadioButton.isOn = true;
                        break;
                    case EUiDisplayType.EUDT_WorldGeoQuad:
                        applicationRadioButton.isOn = true;
                        //ActivateWorldGeo();
                        break;
                    case EUiDisplayType.EUDT_None:
                        noneRadioButton.isOn = true;
                        //ActivateNone();
                        break;
                }
                Debug.Log("Desired ui type = " + desiredUiType);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                TriggerLoad();
            }
        }
        #endregion

        #region Private Functions
        void ActivateWorldGeo()
        {
            worldspaceGeoParent.SetActive(true);
            uiGeoParent.SetActive(false);
            uiCamera.SetActive(false);
            cameraRenderOverlay.enabled = false;
            renderingLabelOverlay.enabled = true;
            renderingLabelOverlay.textures[0] = applicationLabelTexture;
            desiredUiType = EUiDisplayType.EUDT_WorldGeoQuad;
            Debug.Log("Switched to ActivateWorldGeo");
            //applicationRadioButton.isOn = true;
        }

        void ActivateOVROverlay()
        {
            worldspaceGeoParent.SetActive(false);
            uiCamera.SetActive(true);
            cameraRenderOverlay.enabled = true;
            uiGeoParent.SetActive(true);
            renderingLabelOverlay.enabled = true;
            renderingLabelOverlay.textures[0] = compositorLabelTexture;
            desiredUiType = EUiDisplayType.EUDT_OverlayQuad;
            Debug.Log("Switched to ActivateOVROVerlay");
            //overlayRadioButton.isOn = true;
        }

        void ActivateNone()
        {
            worldspaceGeoParent.SetActive(false);
            uiCamera.SetActive(false);
            cameraRenderOverlay.enabled = false;
            uiGeoParent.SetActive(false);
            renderingLabelOverlay.enabled = false;
            desiredUiType = EUiDisplayType.EUDT_None;
            Debug.Log("Switched to ActivateNone");
            //noneRadioButton.isOn = true;
        }

        // This function is to simulate a level load event in Unity
        // The idea is to enable a cubemap overlay right before any action that will stall the main thread
        // This cubemap overlay can be combined with other OVROverlay objects, such as animated textures to indicate "Loading..."
        void TriggerLoad()
        {
            StartCoroutine(WaitforOVROverlay());
        }

        void TriggerUnload()
        {
            ClearObjects();
            //ActivateWorldGeo();
            applicationRadioButton.isOn = true;
        }

        IEnumerator WaitforOVROverlay()
        {
            Transform camTransform = mainCamera.transform;
            Transform uiTextOverlayTrasnform = loadingTextQuadOverlay.transform;
            Vector3 newPos = camTransform.position + camTransform.forward * distanceFromCamToLoadText;
            newPos.y = camTransform.position.y;
            uiTextOverlayTrasnform.position = newPos;
            cubemapOverlay.enabled = true;
            loadingTextQuadOverlay.enabled = true;
            //ActivateNone();
            noneRadioButton.isOn = true;
            yield return new WaitForSeconds(0.1f);
            ClearObjects();
            SimulateLevelLoad();
            cubemapOverlay.enabled = false;
            loadingTextQuadOverlay.enabled = false;
            yield return null;
        }


        /// <summary>
        /// Usage: Recreate UI render target according overlay type and overlay size
        /// </summary>
        void CameraAndRenderTargetSetup()
        {
            float overlayWidth = cameraRenderOverlay.transform.localScale.x;
            float overlayHeight = cameraRenderOverlay.transform.localScale.y;
            float overlayRadius = cameraRenderOverlay.transform.localScale.z;

#if UNITY_ANDROID
		// Gear VR display panel resolution
		float hmdPanelResWidth = 2560;
		float hmdPanelResHeight = 1440;
#else
            // Rift display panel resolution
            float hmdPanelResWidth = 2160;
            float hmdPanelResHeight = 1200;
#endif

            float singleEyeScreenPhysicalResX = hmdPanelResWidth * 0.5f;
            float singleEyeScreenPhysicalResY = hmdPanelResHeight;

            // Calculate RT Height     
            // screenSizeYInWorld : how much world unity the full screen can cover at overlayQuad's location vertically
            // pixelDensityY: pixels / world unit ( meter )

            float halfFovY = mainCamera.GetComponent<Camera>().fieldOfView / 2;
            float screenSizeYInWorld = 2 * overlayRadius * Mathf.Tan(Mathf.Deg2Rad * halfFovY);
            float pixelDensityYPerWorldUnit = singleEyeScreenPhysicalResY / screenSizeYInWorld;
            float renderTargetHeight = pixelDensityYPerWorldUnit * overlayWidth;

            // Calculate RT width
            float renderTargetWidth = 0.0f;

            // screenSizeXInWorld : how much world unity the full screen can cover at overlayQuad's location horizontally
            // pixelDensityY: pixels / world unit ( meter )

            float screenSizeXInWorld = screenSizeYInWorld * mainCamera.GetComponent<Camera>().aspect;
            float pixelDensityXPerWorldUnit = singleEyeScreenPhysicalResX / screenSizeXInWorld;
            renderTargetWidth = pixelDensityXPerWorldUnit * overlayWidth;

            // Compute the orthographic size for the camera
            float orthographicSize = overlayHeight / 2.0f;
            float orthoCameraAspect = overlayWidth / overlayHeight;
            uiCamera.GetComponent<Camera>().orthographicSize = orthographicSize;
            uiCamera.GetComponent<Camera>().aspect = orthoCameraAspect;

            if (uiCamera.GetComponent<Camera>().targetTexture != null)
                uiCamera.GetComponent<Camera>().targetTexture.Release();

            RenderTexture overlayRT = new RenderTexture(
                    (int)renderTargetWidth * 2,
                    (int)renderTargetHeight * 2,
                    0,
                    RenderTextureFormat.ARGB32,
                    RenderTextureReadWrite.sRGB);
            Debug.Log("Created RT of resolution w: " + renderTargetWidth + " and h: " + renderTargetHeight);

            overlayRT.hideFlags = HideFlags.DontSave;
            overlayRT.useMipMap = true;
            overlayRT.filterMode = FilterMode.Trilinear;
            overlayRT.anisoLevel = 4;
#if UNITY_5_5_OR_NEWER
            overlayRT.autoGenerateMips = true;
#else
		overlayRT.generateMips = true;
#endif
            uiCamera.GetComponent<Camera>().targetTexture = overlayRT;

            cameraRenderOverlay.textures[0] = overlayRT;
        }

        void SimulateLevelLoad()
        {
            int numToPrint = 0;
            for (int p = 0; p < NumLoopsTrigger; p++)
            {
                numToPrint++;
            }
            Debug.Log("Finished " + numToPrint + " Loops");
            Vector3 playerPos = mainCamera.transform.position;
            playerPos.y = 0.5f;
            // Generate a bunch of blocks, "blocking" the mainthread ;)
            for (int j = 0; j < numLevels; j++)
            {
                for (var i = 0; i < numObjectsPerLevel; i++)
                {
                    var angle = i * Mathf.PI * 2 / numObjectsPerLevel;
                    float stagger = (i % 2 == 0) ? 1.5f : 1.0f;
                    var pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * cubeSpawnRadius * stagger;
                    pos.y = j * heightBetweenItems;
                    var newInst = Instantiate(prefabForLevelLoadSim, pos + playerPos, Quaternion.identity);
                    var newObjTransform = newInst.transform;
                    newObjTransform.LookAt(playerPos);
                    Vector3 newAngle = newObjTransform.rotation.eulerAngles;
                    newAngle.x = 0.0f;
                    newObjTransform.rotation = Quaternion.Euler(newAngle);
                    spawnedCubes.Add(newInst);
                }
            }
        }

        void ClearObjects()
        {
            for (int i = 0; i < spawnedCubes.Count; i++)
            {
                DestroyImmediate(spawnedCubes[i]);
            }
            spawnedCubes.Clear();
            GC.Collect();
        }
        #endregion

        #region Debug UI Handlers
        public void RadioPressed(string radioLabel, string group, Toggle t)
        {
            if (string.Compare(radioLabel, ovrOverlayID) == 0)
            {
                ActivateOVROverlay();
            }
            else if (string.Compare(radioLabel, applicationID) == 0)
            {
                ActivateWorldGeo();
            }
            else if (string.Compare(radioLabel, noneID) == 0)
            {
                ActivateNone();
            }
        }
        #endregion
    }
}
