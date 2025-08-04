using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{
    // UI ��ҵ��� ���� (Inspector â����)
    public InputField emailInput;
    public InputField passwordInput;
    public Text statusText;



    // --- ��ư�� ������ �Լ��� ---

    // ȸ������ ��ư Ŭ�� �� ȣ��
    public async void OnSignUpButtonClicked()
    {
        statusText.text = "ȸ������ ��...";
        var user = await FIrebaseAuthManager.Instance.SignUp(emailInput.text, passwordInput.text);
        if (user != null)
        {
            statusText.text = $"Welcome {user.Email}!";
            // ȸ������ ���� �� �ʱ� ������ ���� �� ����
            DataManager.Instance.currentPlayerdata = new PlayerData();
            await DataManager.Instance.SaveData(user.UserId);
        }
        else
        {
            statusText.text = "ȸ������ ����";
        }
    }

    // �α��� ��ư Ŭ�� �� ȣ��
    public async void OnLoginButtonClicked()
    {
        statusText.text = "�α��� ��...";
        var user = await FIrebaseAuthManager.Instance.Login(emailInput.text, passwordInput.text);
        if (user != null)
        {
            statusText.text = $"Logged in as {user.Email}";
            // �α��� ���� �� ������ �ҷ�����
            DataManager.Instance.currentPlayerdata = await DataManager.Instance.LoadData(user.UserId);
            SceneManager.LoadScene("TestScene 4_Main"); // ���� �޴��� �̵�
        }
        else
        {
            statusText.text = "�α��� ����.";
        }
    }

    // �α׾ƿ� ��ư Ŭ�� �� ȣ��
    public void OnLogoutButtonClicked()
    {
        FIrebaseAuthManager.Instance.Logout();
        statusText.text = "�α׾ƿ�";
        DataManager.Instance.currentPlayerdata = null;
    }
}
