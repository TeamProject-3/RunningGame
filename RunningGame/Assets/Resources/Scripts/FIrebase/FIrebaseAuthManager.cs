using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;


public class FIrebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public InputField email;
    public InputField password;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Create()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            // 회원가입 캔슬
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            // 회원가입 실패
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
                return;
            }
            FirebaseUser newUser = task.Result.User;
            Debug.Log("회원가입 성공");
        });
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            // 회원가입 캔슬
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            // 회원가입 실패
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                return;
            }
            FirebaseUser newUser = task.Result.User;
            Debug.Log("로그인 성공");
        });
    }
    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}
