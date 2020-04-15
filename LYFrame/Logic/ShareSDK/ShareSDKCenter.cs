using cn.sharesdk.unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareSDKCenter : MonoBehaviour
{
    public ShareSDK ssdk;

    public MobSDK mobsdk;
    // Start is called before the first frame update
    void Start()
    {
        ssdk = gameObject.GetComponent<ShareSDK>();
        ssdk.authHandler = OnAuthResultHandler;
        ssdk.shareHandler = OnShareResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;
        ssdk.getFriendsHandler = OnGetFriendsResultHandler;
        ssdk.followFriendHandler = OnFollowFriendResultHandler;
        mobsdk = gameObject.GetComponent<MobSDK>();

#if UNITY_ANDROID
		
#elif UNITY_IPHONE
		mobsdk.getPolicy = OnFollowGetPolicy;
        ssdk.wxRequestHandler = GetWXRequestTokenResultHandler;
        ShareSDKRestoreScene.setRestoreSceneListener(OnRestoreScene);
#endif
    }

    private void OnFollowFriendResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        throw new NotImplementedException();
    }

    private void OnGetFriendsResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        throw new NotImplementedException();
    }

    private void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        throw new NotImplementedException();
    }

    private void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        throw new NotImplementedException();
    }

    private void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        throw new NotImplementedException();
    }

}
