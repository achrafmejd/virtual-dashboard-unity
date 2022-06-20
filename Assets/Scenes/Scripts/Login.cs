using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Login : MonoBehaviour
{
    [SerializeField] private string authentificationEndPoint = "http://localhost:3000/unity-login";


    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;


    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    
    public void OnLoginClick(){
        alertText.text = "Signing In...";
        loginButton.interactable=false;
        StartCoroutine(TryLogin());
    }



    
    public void MoveToScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }



    

    private IEnumerator TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if(username.Length < 3 || username.Length > 24){
            alertText.text = "Invalid Username";
            loginButton.interactable = true;
            yield break;
        }

        if(password.Length < 3 || password.Length > 24){
            alertText.text = "Invalid Password";
            loginButton.interactable = true;
            yield break;
        }

        
        UnityWebRequest request = UnityWebRequest.Get($"{authentificationEndPoint}?rUsername={username}&rPassword={password}");

        var handler = request.SendWebRequest();
        float startTime = 0.0f;

        while(!handler.isDone){

            startTime += Time.deltaTime;

            if(startTime>10.0f){
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success)
        {      
            
            var res = JsonConvert.DeserializeObject<JToken>(request.downloadHandler.text);

            //Debug.Log(res["code"]);
            if(res["code"].ToString() == "400"){
                Debug.Log("Errors");
                if(res["errors"]["email"].ToString()!= ""){
                    //Debug.Log("Email");
                    alertText.text = "This Email is not registered";
                    loginButton.interactable=true;
                }else{
                    //Debug.Log("Password");
                    alertText.text = "The password is incorrect";
                    loginButton.interactable=true;
                }
            }else{
                //Debug.Log("User");
                alertText.text = "Welcome";
                MoveToScene(1);
                loginButton.interactable=false;
            }
            //Debug.Log(res);
           /*  var basedOn = request.downloadHandler.text;
            if(basedOn.Length == 65){
                alertText.text = "This Email is not registered";
                loginButton.interactable=true;
            }else if(basedOn.Length == 62){
                alertText.text = "The password is incorrect";
                loginButton.interactable=true;
            }else{
                // Everything is OK
                MoveToScene(1);
                alertText.text = "Hello";
                loginButton.interactable=false;
            } */
            

        }else{
            alertText.text = "Error connecting to the server...";
            loginButton.interactable=true;

            Debug.Log("Unable");
        }
        //Debug.Log(UnityWebRequest.Result.Success);
        //Debug.Log(request.result);

        Debug.Log($"{username}:{password}");

        yield return null;
    }
    

}
