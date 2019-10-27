﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TowerDefense
{
	[Serializable]
	public class WaveData
	{
		public GameObject MobPrefab;
		public int MobCount;
		public float Interval;
		public string PathName;

		public WaveData()
		{
			MobPrefab = null;
			MobCount = 5;
			Interval = 2;
			PathName = "Path0";
		}
	}
	
	public class Wave : MonoBehaviour
	{
		public static event Action<int> StartedEvent;
		public static event Action<int> EndedEvent;
		public static event Action<Unit, Wave> MobSpawnedEvent;
		public static event Action<Wave> LookingForSpawnPointsEvent;
		
		public bool ShowWaypoints;
		public bool Active = true;
		int waveNumber;
		[NonSerialized] public Dictionary<string, Vector2> SpawnPoints;

		void Start()
		{
			if (LookingForSpawnPointsEvent != null)
				LookingForSpawnPointsEvent(this);
		}

		public virtual void InitWave(int waveNumber)
		{
			this.waveNumber = waveNumber;
			StartCoroutine(StartWave());
			
			if (StartedEvent != null)
				StartedEvent(waveNumber);
		}

		public virtual IEnumerator StartWave()
		{
			Debug.Log(string.Format("# Wave # Wave {0} started", waveNumber));

			yield return null;
		}

		public virtual void EndWave()
		{
			Debug.Log(string.Format("# Wave # Wave {0} ended", waveNumber));

			if (EndedEvent != null)
				EndedEvent(waveNumber);
		}
		
		public void StartSpawn(GameObject mobPrefab, int mobCount, float interval, string pathName)
		{
			StartCoroutine(Spawning(mobPrefab, mobCount, interval, pathName));
		}

		IEnumerator Spawning(GameObject mobPrefab, int mobCount, float interval, string pathName)
		{
			int mobCounter = 0;
			while (mobCounter < mobCount)
			{
				float timeElapsed = 0;
				while (timeElapsed < interval)
				{
					timeElapsed += Time.fixedDeltaTime;
					//Debug.Log(Time.deltaTime.ToString());
					yield return null;
				}
				SpawnMob(mobPrefab, pathName);
				mobCounter += 1;
			}
		}

		void SpawnMob(GameObject mobPrefab, string pathName)
		{
			GameObject mob = SimplePool.Spawn(mobPrefab,
				SpawnPoints[pathName], transform.rotation);
			//				mob.transform.position += new Vector3(i % 2 == 0 ? WaveControl.RangeBetweenMobsInGroup.x*i : 0,
			//					i % 2 == 0 ? WaveControl.RangeBetweenMobsInGroup.y*i : 0, 0);
			Unit unit = mob.GetComponent<Unit>();
			unit.Init(pathName);

			if (MobSpawnedEvent != null)
				MobSpawnedEvent(unit, this);
		}
		
		void OnDrawGizmos()
		{
			if (ShowWaypoints)
			{
				GUIStyle style = new GUIStyle();
				style.normal.textColor = Color.blue;
				style.fontStyle = FontStyle.Bold;

				if (transform.childCount > 0)
				{
					Gizmos.color = Color.blue;
					for (int i = 0; i < transform.childCount - 1; i++)
					{
						Gizmos.DrawSphere(transform.GetChild(i).position, Vector3.one.x * 0.05f);
						Handles.Label(transform.GetChild(i).position, i.ToString(), style);
						Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
					}

					int indexLast = transform.childCount - 1;
					Gizmos.DrawSphere(transform.GetChild(indexLast).position, Vector3.one.x * 0.05f);
					Handles.Label(transform.GetChild(indexLast).position, indexLast.ToString(), style);
				}
			}
		}
	}
}