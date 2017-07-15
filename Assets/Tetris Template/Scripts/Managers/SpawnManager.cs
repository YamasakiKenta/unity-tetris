using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public GameObject[] shapeTypes;

    public GameObject holdShape;
    public GameObject nextShape;
    public GameObject nowShape;
    public int spanListCnt = -1;
    public int spanListHold = -1;
    public int spanListNow = -1;
    public int[] spanList;

    public void ResetSpawnList(){
        int i, r1,r2, swap;
        spanList = new int[] { 0, 1, 2, 3, 4, 5, 6 };
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
            NextSpawn();
        }

        int i = spanList[spanListCnt];
        spanListNow = i;

        // Spawn Group at current Position
        nowShape = Instantiate(shapeTypes[i]);
        Managers.Game.currentShape = nowShape.GetComponent<TetrisShape>();
        nowShape.transform.parent = Managers.Game.blockHolder;
        Managers.Input.isActive = true;

        NextSpawn();
    }

    public void NextSpawn()
    {
        int i;
        Destroy(nextShape.gameObject);
        spanListCnt++;
        if(spanListCnt >= spanList.Length){
            ResetSpawnList();
            spanListCnt = 0;
        }

        i = spanList[spanListCnt];
        nextShape = Instantiate(shapeTypes[i]);
        nextShape.transform.position = new Vector2(1, 19);
    }

    public void Hold()
    {
        int i;

        Destroy(nowShape.gameObject);
        Managers.Game.currentShape = null;

        if(spanListHold>=0)
        {
            i = spanListHold;
            spanListHold = spanListNow;

            nowShape = Instantiate(shapeTypes[i]) ;
            Managers.Game.currentShape = nowShape.GetComponent<TetrisShape>();
            nowShape.transform.parent = Managers.Game.blockHolder;
            Managers.Input.isActive = true;
            NextSpawn();
        }
        else
        {
            spanListHold = spanListNow;
            Spawn();
        }

        Destroy(holdShape.gameObject);
        i = spanListHold;
        holdShape = Instantiate(shapeTypes[i]);
        holdShape.transform.position = new Vector2(7, 19);
    }
}
