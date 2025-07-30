using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class FIrebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public static FIrebaseAuthManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // ������ ���� �ɶ� �α׾ƿ�
        Logout();
    }
    void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase �غ� �Ϸ�");
            }
            else
            {
                Debug.LogError($"Firebase ���� : {dependencyStatus}");
            }
        });
    }

    // ȸ������ (async : �񵿱���)
    public async Task<FirebaseUser> SignUp(string email, string password)
    {
        if (auth == null) return null;

        try
        {
            //(await : ������)
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"ȸ������ �Ϸ� �̸���: {user.Email}");
            return user;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ȸ������ ����: {e.Message}");
            return null;
        }
    }

    // �α���
    public async Task<FirebaseUser> Login(string email, string password)
    {
        if (auth == null) return null;
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"�α��� �Ϸ� �̸���: {user.Email}");
            return user;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"�α��� ����: {e.Message}");
            return null;
        }
    }

    // �α׾ƿ�
    public void Logout()
    {
        if (auth != null && user != null)
        {
            auth.SignOut();
            user = null;
            Debug.Log("�α� �ƿ�");
        }
    }

    // ���� �α��ε� ���� ���� ��������
    public FirebaseUser GetCurrentUser()
    {
        // auth�� null�̰ų� ���� ������ ���ٸ� null ��ȯ (null ���տ�����)
        return user ?? auth?.CurrentUser;
    }

    // ���� �α��ε� ������ ���� ID ��������
    public string GetUserUID()
    {
        return GetCurrentUser()?.UserId;
    }
}
