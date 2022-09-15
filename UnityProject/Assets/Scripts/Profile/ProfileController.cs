using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
#endif


public class CallbackMessage
{
    public bool IsSuccess;
    public string ErrorMessage;
}
  


public class CallbackSetUserDataMessage: CallbackMessage
{
    public string UserID;
    public User UserData;
}

public class CallbackInitFirebaseMessage : CallbackMessage
{
    
}

public class CallbackGetUserDataMessage : CallbackMessage
{
    public User UserData;
}
public class CallbackLoginMessage : CallbackMessage
{
}

public class ProfileController : MonoBehaviour
{
    public static ProfileController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public bool FirebaseIsInited = false;
    public bool FirebaseLogin = false;

#if UNITY_EDITOR
    private FirebaseAuth Auth;
    private FirebaseDatabase Database;
    private void CleanTask(Task _task)
    {
        _task.Dispose();
        _task = null;
    }
#endif
    


    public void InitFirebase(Action<CallbackInitFirebaseMessage> _callback = null)
    {
#if UNITY_EDITOR
        CallbackInitFirebaseMessage _response = new CallbackInitFirebaseMessage();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                _response.IsSuccess = true;
                Auth = FirebaseAuth.DefaultInstance;
                Database = FirebaseDatabase.DefaultInstance;

                FirebaseIsInited = true;
                CleanTask(task);
            }
            else
            {
                CleanTask(task);
            }

            if (_callback != null)
            {
                _callback.Invoke(_response);
            }
        });
#else
        CallbackInitFirebaseMessage _response = new CallbackInitFirebaseMessage();
        _response.IsSuccess = true;
        if (_callback != null)
        {
            _callback.Invoke(_response);
        }
#endif
    }

    public void LoginWithCustomTokenID(Action<CallbackLoginMessage> _callback = null)
    {
#if UNITY_EDITOR
        CallbackLoginMessage logMessage = new CallbackLoginMessage();
        Auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCustomTokenAsync was canceled.");
                logMessage.IsSuccess = false;
                logMessage.ErrorMessage = "SignInWithCustomTokenAsync was canceled";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCustomTokenAsync encountered an error: " + task.Exception);
                logMessage.IsSuccess = false;
                logMessage.ErrorMessage = "SignInWithCustomTokenAsync encountered an error: " + task.Exception;
                return;
            }
            CleanTask(task);
            FirebaseLogin = true;
            logMessage.IsSuccess = true;
            if (_callback != null)
            {
                _callback.Invoke(logMessage);
            }
        });
#else
        auth_callback = _callback;
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseAuth.SignInAnonymously(gameObject.name, "WebGL_AuthSuccess", "WebGL_AuthFailed");
#endif
    }


    public void SetUserData(User _user, Action<CallbackSetUserDataMessage> _callback = null)
    {
#if UNITY_EDITOR
        string json = JsonUtility.ToJson(_user);
        CallbackSetUserDataMessage _logMsg = new CallbackSetUserDataMessage();
        Database.RootReference.Child(GameConstant.USER_DATABASE_ROOT_NAME).Child(_user.UserID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                _logMsg.ErrorMessage = "Set user data was canceled";
                _logMsg.IsSuccess = false;
                _callback.Invoke(_logMsg);

                CleanTask(task);
                return;
            }
            if (task.IsFaulted)
            {
                _logMsg.ErrorMessage = "Set User Data encountered an error: " + task.Exception;
                _logMsg.IsSuccess = false;
                _callback.Invoke(_logMsg);
                CleanTask(task);
                return;
            }
            _logMsg.IsSuccess = true;
            _logMsg.UserID = _user.UserID;
            _logMsg.UserData = _user;
            if (_callback != null)
            {
                _callback.Invoke(_logMsg);
            }
            CleanTask(task);
        });
#else
        string json = JsonUtility.ToJson(_user);
        set_user_callback = _callback;
        _tmpUserData = _user;
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseDatabase.PostJSON(GameConstant.USER_DATABASE_ROOT_NAME + "/" + _user.UserID, json, gameObject.name, "WebGL_SetUserDataSuccess", "WebGL_SetUserDataFailed");
#endif
    }

    public void GetUserData(string _userID, Action<CallbackGetUserDataMessage> _callback)
    {
#if UNITY_EDITOR
        CallbackGetUserDataMessage _response = new CallbackGetUserDataMessage();
        Query databaseQuery = Database.RootReference.Child(GameConstant.USER_DATABASE_ROOT_NAME).Child(_userID);
        databaseQuery.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                try
                {
                    User _user = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue().ToString());
                    _response.IsSuccess = true;
                    _response.UserData = _user;
                    if (_callback != null)
                    {
                        _callback.Invoke(_response);
                    }
                    CleanTask(task);
                }
                catch (Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.UserData = null;
                    if (_callback != null)
                    {
                        _callback.Invoke(_response);
                    }
                    CleanTask(task);
                }
                
            }
            else
            {
                _response.IsSuccess = false;
                _response.UserData = null;
                if (_callback != null)
                {
                    _callback.Invoke(_response);
                }
                CleanTask(task);
            }
        });
#else
        get_user_callback = _callback;
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseDatabase.GetJSON(GameConstant.USER_DATABASE_ROOT_NAME + "/" + _userID, gameObject.name, "WebGL_GetUserDataSuccess", "WebGL_GetUserDataFailed");
#endif
    }


#if UNITY_WEBGL && !UNITY_EDITOR
    private Action<CallbackLoginMessage> auth_callback;
    // WebGL events
    public void WebGL_AuthSuccess(string result)
    {
        Debug.Log("WebGL_AuthSuccess:" + result);
        CallbackLoginMessage logMessage = new CallbackLoginMessage();
        logMessage.IsSuccess = true;
        auth_callback?.Invoke(logMessage);
        auth_callback = null;
    }

    public void WebGL_AuthFailed(string result)
    {
        Debug.Log("WebGL_AuthFailed:" + result); 
        CallbackLoginMessage logMessage = new CallbackLoginMessage();
        logMessage.IsSuccess = false;
        auth_callback?.Invoke(logMessage);
        auth_callback = null;
    }

    private Action<CallbackSetUserDataMessage> set_user_callback;
    private User _tmpUserData;
    // WebGL events
    public void WebGL_SetUserDataSuccess(string result)
    {
        Debug.Log("WebGL_SetUserDataSuccess:" + result);
        CallbackSetUserDataMessage logMessage = new CallbackSetUserDataMessage();
        logMessage.IsSuccess = true;
        logMessage.UserData = _tmpUserData;
        set_user_callback?.Invoke(logMessage);
        set_user_callback = null;
    }

    public void WebGL_SetUserDataFailed(string result)
    {
        Debug.Log("WebGL_SetUserDataFailed:" + result);
        CallbackSetUserDataMessage logMessage = new CallbackSetUserDataMessage();
        logMessage.IsSuccess = false;
        logMessage.ErrorMessage = result;
        set_user_callback?.Invoke(logMessage);
        set_user_callback = null;
    }

    private Action<CallbackGetUserDataMessage> get_user_callback;
    // WebGL events
    public void WebGL_GetUserDataSuccess(string result)
    {
        Debug.Log("WebGL_GetUserDataSuccess:" + result);
        CallbackGetUserDataMessage logMessage = new CallbackGetUserDataMessage();
        try
        {
            logMessage.IsSuccess = true;
            logMessage.UserData = JsonUtility.FromJson<User>(result);
            get_user_callback?.Invoke(logMessage);
        }
        catch (Exception ex)
        {
            logMessage.IsSuccess = false;
            logMessage.ErrorMessage = ex.Message;
            get_user_callback?.Invoke(logMessage);
        }
        get_user_callback = null;
    }

    public void WebGL_GetUserDataFailed(string result)
    {
        Debug.Log("WebGL_GetUserDataFailed:" + result);
        CallbackGetUserDataMessage logMessage = new CallbackGetUserDataMessage();
        logMessage.IsSuccess = false;
        logMessage.ErrorMessage = result;
        get_user_callback?.Invoke(logMessage);
        get_user_callback = null;
    }
#endif


}
