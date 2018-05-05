using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct abcStruct {
	public Items itemType;
	public int number;
}

[System.Serializable]
public struct BlockCostume {
	public Items itemType;
	public Mesh mesh;
	public Texture texture;
	public float hp;
	public float reward;
}

public class Algorism : MonoBehaviour {
	public List<BlockCostume> costume;

	public float StartX;
	public float StartZ;
	public float StartY;
	public int x = 0;
	public int y = 0;
	public List<abcStruct> abc;
	public GameObject cube;
	public string[,] map;
	public float space;

	void Start () {
		float fixX = x;
		float fixZ = y;
		map = new string[x,y];
		
		for(int i=0;i<abc.Count;i++) {
			for (int a = 0;a < abc[i].number;a++) {
				var xx = Random.Range(0,x);
				var yy = Random.Range(0,y);
				if (map[xx,yy] == null) {
					map[xx,yy] = ((int)abc[i].itemType).ToString();
				}
				else
				a--; //for문의 전으로
			}
		}

		string ss = "";
		foreach(var a in map)
		ss += a+" / ";
		
		print(ss);
		
		for(int xx = 0;xx < x;xx++) 
		for(int yy = 0;yy< y;yy++) {
			fixX = StartX + xx * space;
			fixZ = StartZ + yy * space;
			GameObject cubeObj = Instantiate(cube);
			cubeObj.transform.SetParent(this.gameObject.transform);
			cubeObj.transform.localPosition = new Vector3(fixX,StartY,fixZ);
			
			cubeObj.GetComponent<Block>().blockType = (Items)int.Parse(map[xx,yy]);
			cubeObj.GetComponent<Block>().costume = costume;
		}
		
	}
	
}
