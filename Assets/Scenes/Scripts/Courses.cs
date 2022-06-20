using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Courses : MonoBehaviour
{

    public GameObject courseInfoContainer;
    public GameObject courseInfoTemplate;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:3000/get-courses"));   
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
                     var courses = JsonConvert.DeserializeObject<JToken>(webRequest.downloadHandler.text);
                    //Debug.Log(stu[0]);
                    
                    foreach (var item in courses)
                    {
                        GameObject gobjc = (GameObject)Instantiate(courseInfoTemplate);

                        //Debug.Log(item["firstName"]);
                        gobjc.transform.SetParent(courseInfoContainer.transform);
                        gobjc.GetComponent<CourseInfo>().title.text = item["title"].ToString();
                        gobjc.GetComponent<CourseInfo>().provider.text = item["constructorName"].ToString();
                        gobjc.GetComponent<CourseInfo>().numberOfCertified.text = item["certifiedStudents"].ToString();
                    }

                    break;
            }
        }
    }
}
