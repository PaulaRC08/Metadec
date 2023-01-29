CHANGELOG v1.03
========================
1. Add Avatar Renderer to display on UI.
2. Add Eye Animation Handler and Voice Handler in RPMRuntime so we can control the parameter.
3. Add option to load avatar on start.
4. Move PlayerFollowCamera outside PlayerArmatureRPM Prefab, and assign it reference at runtime.
5. Add option to Find avatar parent using Find unique GameObject as reference, instead of FindObjectWithTag so assigning avatars can work uniquely especially in scenes where there are other objects that use the same Tag. 
you can revert back to FindObjectWithTag in AvatarProcessor.cs by delete or commenting line 74 and activate line 75.

preview : https://youtu.be/e4lKmQSlJQY

INTEGRATION STEP
========================
1. Delete previous RPMxTPCBridge folder from your project if any.
2. Download and Import Ready Player Me_v1.11.0xStarter_Assets_TPC_v.1.03.unitypackage
3. Edit AvatarProcessor.cs,
if you use previous version then change line 73 to 96, or 73 to 76 in fresh project with below script

            //Added
            var usingTPC = GameObject.Find("https://api.readyplayer.me/v1/avatars/"+ $"{avatar.name}"+ ".glb").transform.parent; //Find Player with assigned avatarUrl only to parenting avatar, if you have more than 1 Player in the scene.
            // var usingTPC = GameObject.FindGameObjectWithTag("Player"); //Find Player with Tag to parent avatar
            if (usingTPC)  //Set Custom Process if using TPC
            {
                avatar.transform.parent = usingTPC.transform; //Set parent for avatar
                avatar.transform.SetPositionAndRotation(usingTPC.transform.position, usingTPC.transform.rotation); //Set pos and rot avatar to align in runtime
                Animator animator = avatar.GetComponentInParent<Animator>(); //Get PlayerArmature Animator
                animator.avatar = animationAvatar; //Assign Animator Avatar
                // animator.applyRootMotion = false; //Set Animator Root Motion to false, it's false by default
            }
            else  //Back to original Ready Player Me Process if not using TPC
            {
                Animator animator = avatar.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorController;
                animator.avatar = animationAvatar;
                animator.applyRootMotion = true;
            }
            //End
			
TEST AVATAR :
===========================
https://api.readyplayer.me/v1/avatars/622a38c8cc9780a06940af32.glb
https://api.readyplayer.me/v1/avatars/62b57e2948960be56ef930af.glb
https://api.readyplayer.me/v1/avatars/62bbdcc748960be56eb41340.glb
https://api.readyplayer.me/v1/avatars/62bf0f3348960be56e139cbb.glb

Feminine :
https://api.readyplayer.me/v1/avatars/631c0fa53a656b9c32ddbf5a.glb

Masculine :
https://api.readyplayer.me/v1/avatars/631c10373a656b9c32ddcf24.glb

Known Issue :
===========================
Error Message : Release of invalid GC handle. The handle is from a previous domain. The release operation is skipped.
sometime it's happening when Stop the scene while still loading Avatar Renderer.
Simillar Issue Tracker https://issuetracker.unity3d.com/issues/2020-dot-3-resolve-of-invalid-gc-handle-error-occurs-when-exiting-play-mode-after-creating-a-tilemap