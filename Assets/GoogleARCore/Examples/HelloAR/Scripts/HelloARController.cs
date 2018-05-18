﻿//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;

namespace GoogleARCore.Examples.HelloAR
{
    using UnityEngine.XR;
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        private List<GameObject> objects = new List<GameObject>();
        public Slider superSlider;
        Text SaldoText;
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject CoinsPrefab;

        public GameObject BallPrefab;
        public GameObject AndyAndroidPrefab;
        public GameObject BurgerPrefab;
        public GameObject ChipsPrefab;
        public GameObject BananaPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        public GameObject PlaneGenerator; 
        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public int Saldo;

        public float DropDelay = 0.5f;

        public int DropHeight = 1;

        private const int _coinValue = 1;
        private const int _ballValue = 15;
        private const int _burgerValue = 3;
        private const int _chipsValue = 2;
        private const int _bananaValue = 1;
        private int selectedModel = 0;
        int numberOfLoops=0;
        int superSaldo =0;
        private static bool canDelay = true;

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUi = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUi = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUi);
            
            // if there is phone input....

            if (Input.touchCount > 0)
            {
                Debug.Log("touch");
                // If the player has not touched the screen, we are done with this update.
                Touch touch;
               
                if (Input.touchCount < 1 )
                {
                    Debug.Log("no touch count");

                    return;
                }
                if ((touch = Input.GetTouch(0)).phase == TouchPhase.Began)
                {
                    TrackableHit hit;
                    TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                                                      TrackableHitFlags.FeaturePointWithSurfaceNormal;
                
                    if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                    {
                        Debug.Log("he he het werkt eindelijk...");

                        numberOfLoops = GetNumberOfLoops();
                        DelayLoop(hit, numberOfLoops);
                        //                        InstantiateObject(hit);
                        if (selectedModel == 0)
                        {
                           
                            //SaldoText.text = numberOfLoops.ToString();
                        }
                        else
                        {
                            //superSlider.normalizedValue = (float)numberOfLoops / (float)superSaldo;
// SaldoText.text = numberOfLoops.ToString();
                        }
                    }
                    else
                    {
                        Debug.Log(touch.position.x);
                        Debug.Log(touch.position.y);
                        Debug.Log("Fout met touch");
                    }
                }
                else
                {
                    Debug.Log("Not began");
                }
            } 
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("klik");
                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                                                  TrackableHitFlags.FeaturePointWithSurfaceNormal;

                if (Frame.Raycast(Input.mousePosition.x, Input.mousePosition.y, raycastFilter, out hit))
                {
                    Debug.Log("Start looping");
                    numberOfLoops = GetNumberOfLoops();
                    int superSaldo = PlayerPrefs.GetInt("saldo");
                    if (selectedModel == 0)
                    {
                        //superSlider.normalizedValue = (float)numberOfLoops / (float)(superSaldo / _ballValue);
                       // SaldoText.text = numberOfLoops.ToString();
                    } else
                    {
                        //superSlider.normalizedValue = (float)numberOfLoops / (float)superSaldo;
                       // SaldoText.text = numberOfLoops.ToString();
                    }
                    if (canDelay)
                    {
                        DelayLoop(hit, numberOfLoops);                        
//                        InstantiateObject(hit);
                    }
                }
            }
        }

        /// <summary>
        /// Divedes salde by coinvalue to calculate how many coins can drop
        /// </summary>
        private int GetNumberOfLoops()
        {
            if (selectedModel == 0)
            {
                int nLoops = Saldo / _ballValue;
                return nLoops;
            } else if (selectedModel == 2)
            {
                int nLoops = Saldo / _burgerValue;
                return nLoops;
            }
            else if (selectedModel == 3)
            {
                int nLoops = Saldo / _chipsValue;
                return nLoops;
            }
            else if (selectedModel == 4)
            {
                int nLoops = Saldo / _bananaValue;
                return nLoops;
            }
            else
            {
                int nLoops = Saldo / _coinValue;
            
                Debug.Log("Saldo: " + nLoops);
                Debug.Log("coin value: " + nLoops);
                Debug.Log("nLoops: " + nLoops);
                return nLoops;
            }
        }

        /// <summary>
        /// Starts the coroutine for the delayed loop
        /// </summary>
        private void DelayLoop(TrackableHit hit, int numberOfLoops1)
        {
            StartCoroutine(Corotine(hit, numberOfLoops1));
        }

        private IEnumerator Corotine(TrackableHit hit, int numberOfLoops1)
        {
            Debug.Log("Start delay loop"); 
            
            DetectedPlaneVisualizer detectedPlaneVisualizer = PlaneGenerator.GetComponentInChildren<DetectedPlaneVisualizer>();

            if (detectedPlaneVisualizer == null)
            {
                Debug.Log("Plane visualizer is null");
            }

            DetectedPlane plane = detectedPlaneVisualizer.getDetectedPlane();
            Pose planePos = plane.CenterPose;
            
            float x = plane.ExtentX * 0.1f;   

            for (int i = 0; i < numberOfLoops1; i++)
            {
               
                InstantiateObject(hit, planePos, x);
//                Debug.Log("Object: " + i);
                yield return new WaitForSeconds(DropDelay);
                int i2 = i+1;
                SaldoText.text = i2.ToString();
                if(selectedModel == 0)
                {
                    superSlider.normalizedValue = (float)i2 / (float)(superSaldo / _ballValue);
                } else if(selectedModel == 2)
                {
                    superSlider.normalizedValue = (float)i2 / (float)(superSaldo / _burgerValue);
                }
                else if (selectedModel == 3)
                {
                    superSlider.normalizedValue = (float)i2 / (float)(superSaldo / _chipsValue);
                }
                else if (selectedModel == 4)
                {
                    superSlider.normalizedValue = (float)i2 / (float)(superSaldo / _bananaValue);
                }
                else
                {
                    superSlider.normalizedValue = (float)i2 / (float)superSaldo;
                }
            }
            
            Debug.Log("End delay loop"); 
        }
        
        /// <summary>
        /// Generates an object 
        /// </summary>
        /// <param name="hit"></param>
        private void InstantiateObject(TrackableHit hit, Pose planePos, float x)
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if (hit.Trackable is DetectedPlane &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                // Instantiate Andy model at the hit pose.
                // todo do things here
              
                
                
                float randomRangeX = Random.Range(-x, x);
                float randomRangeY = Random.Range(-x, x);
                GameObject andyObject;
                if (selectedModel == 1)
                {
                    andyObject = Instantiate(CoinsPrefab, planePos.position + new Vector3(randomRangeX, DropHeight, randomRangeY), planePos.rotation);
                } else if (selectedModel == 2){
                    andyObject = Instantiate(BurgerPrefab, planePos.position + new Vector3(randomRangeX, DropHeight, randomRangeY), planePos.rotation);
                }
                else if (selectedModel == 3)
                {
                    andyObject = Instantiate(ChipsPrefab, planePos.position + new Vector3(randomRangeX, DropHeight, randomRangeY), planePos.rotation);
                }
                else if (selectedModel == 4)
                {
                    andyObject = Instantiate(BananaPrefab, planePos.position + new Vector3(randomRangeX, DropHeight, randomRangeY), planePos.rotation);
                }
                else
                {
                    andyObject = Instantiate(BallPrefab,
                        planePos.position + new Vector3(randomRangeX, DropHeight, randomRangeY), planePos.rotation);
                }
                objects.Add(andyObject);

                // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                // world evolves.
                Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make Andy model a child of the anchor.
                andyObject.transform.parent = anchor.transform;
// superSlider.normalizedValue = (float)numberOfLoops / (float)(superSaldo / _ballValue);
  //              SaldoText.text = numberOfLoops.ToString();
            }
        }
        public void clearARComponents()
        {
            
        }
    

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }
        public void Start()
        {
            Saldo = PlayerPrefs.GetInt("saldo");
            superSaldo = PlayerPrefs.GetInt("saldo");
            GameObject temp = GameObject.Find("superSlider");
            superSlider = temp.GetComponent<Slider>();

            GameObject temp2 = GameObject.Find("TextSaldo");
            SaldoText = temp2.GetComponent<Text>();

        }

        public void Button0()
        {
            if (selectedModel == 0)
            {
                
            }
            else
            {
                selectedModel = 0;
                superSlider.normalizedValue = 0;
                SaldoText.text = numberOfLoops.ToString();
                destroyGames();
                objects = new List<GameObject>();

            }

        }
        public void destroyGames()
        {
            foreach (GameObject g in objects)
            {
                Destroy(g);
            }
        }
        public void Button1()
        {
            if (selectedModel == 1)
            {

            }
            else
            {
                selectedModel = 1;
                superSlider.normalizedValue = 0;
                SaldoText.text = numberOfLoops.ToString();
                destroyGames();
                objects = new List<GameObject>();
            }

        }

        public void Button2()
        {
            if (selectedModel == 2)
            {

            }
            else
            {
                selectedModel = 2;
                superSlider.normalizedValue = 0;
                SaldoText.text = numberOfLoops.ToString();
                destroyGames();
                objects = new List<GameObject>();
            }

        }
        public void Button3()
        {
            if (selectedModel == 3)
            {

            }
            else
            {
                selectedModel = 3;
                superSlider.normalizedValue = 0;
                SaldoText.text = numberOfLoops.ToString();
                destroyGames();
                objects = new List<GameObject>();
            }

        }
        public void Button4()
        {
            if (selectedModel == 4)
            {

            }
            else
            {
                selectedModel = 4;
                superSlider.normalizedValue = 0;
                SaldoText.text = numberOfLoops.ToString();
                destroyGames();
                objects = new List<GameObject>();
            }

        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
