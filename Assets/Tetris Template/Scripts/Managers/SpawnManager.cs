using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public GameObject[] shapeTypes;

    public int[] spanList;
    public int spanListCnt = -1;

    public void ResetSpwanList(){
        int i;
        int swap;
        int r1,r2;
        for(i=0;i<shapeTypes.Length;i++){
            spanList[i] = i;
        }
        for(i=0;i<100;i++){
            r1 = Random.Range(0, 7);
            r2 = Random.Range(0, 7);
            swap = spanList[r1];
            spanList[r1] = spanList[r2];
            spanList[r2] = swap;
        }
    }

    public void Spawn()
    {
        // Random Shape
        if(spanListCnt==-1){
            spanList = new int[shapeTypes.Length];
            ResetSpwanList();
            spanListCnt = 0;
        }

        int i = spanList[spanListCnt];

        // Spawn Group at current Position
        GameObject temp =Instantiate(shapeTypes[i]) ;
        Managers.Game.currentShape = temp.GetComponent<TetrisShape>();
        temp.transform.parent = Managers.Game.blockHolder;
        Managers.Input.isActive = true;

        spanListCnt++;
        if(spanListCnt >= spanList.Length){
            ResetSpwanList();
            spanListCnt = 0;
        }
    }
}
