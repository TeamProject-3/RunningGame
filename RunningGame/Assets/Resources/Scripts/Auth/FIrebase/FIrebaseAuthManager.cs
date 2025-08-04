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
        // 게임이 시작 될때 로그아웃
        Logout();
    }
    void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase 준비 완료");
            }
            else
            {
                Debug.LogError($"Firebase 실패 : {dependencyStatus}");
            }
        });
    }

    // 회원가입 (async : 비동기적)
    public async Task<FirebaseUser> SignUp(string email, string password)
    {
        if (auth == null) return null;

        try
        {
            //(await : 동기적)
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"회원가입 완료 이메일: {user.Email}");
            return user;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"회원가입 실패: {e.Message}");
            return null;
        }
    }

    // 로그인
    public async Task<FirebaseUser> Login(string email, string password)
    {
        if (auth == null) return null;
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"로그인 완료 이메일: {user.Email}");
            return user;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"로그인 실패: {e.Message}");
            return null;
        }
    }

    // 로그아웃
    public void Logout()
    {
        if (auth != null && user != null)
        {
            auth.SignOut();
            user = null;
            Debug.Log("로그 아웃");
        }
    }

    // 현재 로그인된 유저 정보 가져오기
    public FirebaseUser GetCurrentUser()
    {
        // auth가 null이거나 현재 유저가 없다면 null 반환 (null 병합연산자)
        return user ?? auth?.CurrentUser;
    }

    // 현재 로그인된 유저의 고유 ID 가져오기
    public string GetUserUID()
    {
        return GetCurrentUser()?.UserId;
    }
}
