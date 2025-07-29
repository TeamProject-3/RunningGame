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
            // ȸ������ ĵ��
            if (task.IsCanceled)
            {
                Debug.LogError("ȸ������ ���");
                return;
            }
            // ȸ������ ����
            if (task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
                return;
            }
            FirebaseUser newUser = task.Result.User;
            Debug.Log("ȸ������ ����");
        });
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            // ȸ������ ĵ��
            if (task.IsCanceled)
            {
                Debug.LogError("�α��� ���");
                return;
            }
            // ȸ������ ����
            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
                return;
            }
            FirebaseUser newUser = task.Result.User;
            Debug.Log("�α��� ����");
        });
    }
    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }
}
