using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{
    // UI 요소들을 연결 (Inspector 창에서)
    public InputField emailInput;
    public InputField passwordInput;
    public Text statusText;



    // --- 버튼과 연결할 함수들 ---

    // 회원가입 버튼 클릭 시 호출
    public async void OnSignUpButtonClicked()
    {
        statusText.text = "회원가입 중...";
        var user = await FIrebaseAuthManager.Instance.SignUp(emailInput.text, passwordInput.text);
        if (user != null)
        {
            statusText.text = $"Welcome {user.Email}!";
            // 회원가입 성공 시 초기 데이터 생성 및 저장
            DataManager.Instance.currentPlayerdata = new PlayerData();
            await DataManager.Instance.SaveData(user.UserId);
        }
        else
        {
            statusText.text = "회원가입 실패";
        }
    }

    // 로그인 버튼 클릭 시 호출
    public async void OnLoginButtonClicked()
    {
        statusText.text = "로그인 중...";
        var user = await FIrebaseAuthManager.Instance.Login(emailInput.text, passwordInput.text);
        if (user != null)
        {
            statusText.text = $"Logged in as {user.Email}";
            // 로그인 성공 시 데이터 불러오기
            DataManager.Instance.currentPlayerdata = await DataManager.Instance.LoadData(user.UserId);
            SceneManager.LoadScene("TestScene 4_Main"); // 메인 메뉴로 이동
        }
        else
        {
            statusText.text = "로그인 실패.";
        }
    }

    // 로그아웃 버튼 클릭 시 호출
    public void OnLogoutButtonClicked()
    {
        FIrebaseAuthManager.Instance.Logout();
        statusText.text = "로그아웃";
        DataManager.Instance.currentPlayerdata = null;
    }
}
