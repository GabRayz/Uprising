using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waves : MonoBehaviour {

	public float waveHeight = 0.31f;
	public float speed = 1.18f;
	public float waveLength = 2f;
	public float noiseStrength = 0f;
	public float noiseWalk = 1f;
	public float randomHeight = 0.01f;
	public float randomSpeed = 9f;
	public float noiseOffset = 20.0f;

	private Vector3[] baseHeight;
	private Vector3[] vertices;
	private List<float> perVertexRandoms = new List<float>();
	private Mesh mesh;
	[Header("Sprite animation")]
	public bool UseSprites;
	public Texture[] Sprites;
	public Material SpriteMaterial;
	public float SpriteFPS = 0.4f;

	void Awake() {
		mesh = GetComponent<MeshFilter>().mesh;
		if (baseHeight == null) {
			baseHeight = mesh.vertices;
		}

		for(int i=0; i < baseHeight.Length; i++) {
			perVertexRandoms.Add(Random.value * randomHeight);
		}
		if (UseSprites)
		{
			StartCoroutine (SpriteAnimation ());
		}
	}

	int SpriteFrame = 0;
	IEnumerator SpriteAnimation()
	{
		if (SpriteFrame != Sprites.Length - 1)
		{
			SpriteFrame += 1;
		}
		else
		{
			SpriteFrame = 0;
		}
		SpriteMaterial.SetTexture ("_MainTex", Sprites [SpriteFrame]);
		yield return new WaitForSeconds (SpriteFPS);
		StartCoroutine (SpriteAnimation ());
	}

	void Update () {
		if (vertices == null) {
			vertices = new Vector3[baseHeight.Length];
		}

		for (int i=0;i<vertices.Length;i++) {
			Vector3 vertex = baseHeight[i];
			Random.InitState((int)((vertex.x + noiseOffset) * (vertex.x + noiseOffset) + (vertex.z + noiseOffset) * (vertex.z + noiseOffset)));
			vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x * waveLength + baseHeight[i].y * waveLength) * waveHeight;
			vertex.y += Mathf.Sin(Mathf.Cos(Random.value * 1.0f) * randomHeight * Mathf.Cos (Time.time * randomSpeed * Mathf.Sin(Random.value * 1.0f)));
			//vertex.y += Mathf.PerlinNoise(baseHeight[i].x + Mathf.Cos(Time.time * 0.1f) + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
			vertices[i] = vertex;
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
	}
}