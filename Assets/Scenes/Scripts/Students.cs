using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Students : MonoBehaviour
{

    public GameObject userInfoContainer;
    public GameObject userInfoTemplate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:3000/get-users"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    var stu = JsonConvert.DeserializeObject<JToken>(webRequest.downloadHandler.text);
                    //Debug.Log(stu[0]);
                    
                    foreach (var item in stu)
                    {
                        GameObject gobj = (GameObject)Instantiate(userInfoTemplate);

                        //Debug.Log(item["firstName"]);
                        gobj.transform.SetParent(userInfoContainer.transform);
                        gobj.GetComponent<UserInfo>().FirstName.text = item["firstName"].ToString();
                        gobj.GetComponent<UserInfo>().LastName.text = item["lastName"].ToString();
                        gobj.GetComponent<UserInfo>().Email.text = item["email"].ToString();
                    }

                    break;
            }
        }
    }

   

}
