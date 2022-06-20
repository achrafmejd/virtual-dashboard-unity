using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeToSinglePage : MonoBehaviour
{

     public void MoveToScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }


    public void OnStudentClick(){
        MoveToScene(3);
        return; 
    }

    public void OnCoursesClick(){
        MoveToScene(2);
        return;
    }

    public void OnBackClick(){
        MoveToScene(1);
        return;
    }

    public void OnLogOut(){
        MoveToScene(0);
        return;
    }
}
