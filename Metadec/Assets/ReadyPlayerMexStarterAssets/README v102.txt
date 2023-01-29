README:
==============
1. Install required packages
	- Unity Starter Assets Third Person Controller
	https://assetstore.unity.com/packages/essentials/starter-assets-third-person-character-controller-196526
	- Ready Player Me Unity SDK v.1.11.0
	https://docs.readyplayer.me/ready-player-me/integration-guides/unity-sdk/unity-sdk-download
	- RPM 11 x TPC Bridge
		Download Link			: https://drive.google.com/drive/folders/1w6PzqP1OAiGaA5gME1r5W2WX-tK4KT3V?usp=sharing			
		Name					: Ready Player Me_v1.11.0xStarter_Assets_TPC_v.1.02.unitypackage
		File Size 				: 32KB
		Supported Unity Version	: 2020LTS and above
		Package Content 		:
			RPMxTPC Bridge
				- Prefabs
					- PlayerArmatureRPM.prefab <- Pre set PlayerArmature for quick testing
				- Scenes
					- Playground_RPM.scene <- a playground for quick testing
				- Scripts
					- RPMRuntime.cs <- modified from RuntimeExample.cs
				
2. Edit AvatarProcessor.cs
	Path : Assets/Plugins/Ready Player Me/Runtime/Operations/AvatarProcessor.cs
	
	Go to SetupAnimator private function Line 63, Change line 73 to 76 with below script :
	
			//Added
            var usingTPC = GameObject.FindGameObjectWithTag("Player");   //Find Player to parent avatar
            if (usingTPC)  //Set Custom Process if using TPC
            {
                avatar.transform.parent = usingTPC.transform;   //Change avatar parent to PlayerArmature Prefabs
                avatar.transform.position = usingTPC.transform.position; //Change avatar position to PlayerArmature position
                avatar.transform.rotation = usingTPC.transform.rotation; //Change avatar rotation to PlayerArmature rotation
                Animator animator = avatar.GetComponentInParent<Animator>(); //Get PlayerArmature Prefabs Animator
                //RuntimeAnimatorController animatorControllerTPC = Resources.Load<RuntimeAnimatorController>("StarterAssetsThirdPerson_RPM");  // used this if you want to Load Custom Controller From Resources Folder
                //animator.runtimeAnimatorController = animatorControllerTPC; //Assign Runtime Animator if used
                animator.avatar = animationAvatar;  //Assign Animator Avatar
                animator.applyRootMotion = false;   //Set Animator Root Motion to false
                avatar.AddComponent<EyeAnimationHandler>(); //Add Ready Player Me Extra Component for Auto Blink avatar
                VoiceHandler voiceHandler = avatar.AddComponent<VoiceHandler>();    //Add Ready Player Me Extra Component Voice Handler for avatar Lipsync  
                voiceHandler.AudioSource = avatar.GetComponentInParent<AudioSource>(); ;    //Assign Audio Source for Voice Handler
            }
            else  //Back to original Ready Player Me Process if not using TPC
            {
                Animator animator = avatar.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorController;
                animator.avatar = animationAvatar;
                animator.applyRootMotion = true;
            }
            //End
	
3. Test Demo Scene, you can use sample avatar link below :

https://api.readyplayer.me/v1/avatars/622a38c8cc9780a06940af32.glb
https://api.readyplayer.me/v1/avatars/62b57e2948960be56ef930af.glb
https://api.readyplayer.me/v1/avatars/62bbdcc748960be56eb41340.glb
https://api.readyplayer.me/v1/avatars/62bf0f3348960be56e139cbb.glb

Feminine :
https://api.readyplayer.me/v1/avatars/631c0fa53a656b9c32ddbf5a.glb

Masculine :
https://api.readyplayer.me/v1/avatars/631c10373a656b9c32ddcf24.glb

	
Known Related Issues :
==============
1. "InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, but you have switched active Input handling to Input System package in Player Settings..."
it's because RPM try to read input using UnityEngine.Input class, while starter assets using New Input System Package.
Possible fix :  go to "Edit > Project Settings > Player > Active Input Handline" and change to "Both"

Unrelated Issues :
==============
1. https://issuetracker.unity3d.com/issues/upgrading-to-1-dot-4-1-throws-errors-for-keyboard-use
Possible fix : Downgrading Input System to 1.3