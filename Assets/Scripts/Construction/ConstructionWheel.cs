﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace TowerDefense
{
	[Serializable]
	public class ConstructData
	{
		public GameObject BuildingPrefab;
		public Sprite IconSprite;
		public Color IconColor = Color.white;
		public int Cost;
		public string TooltipText;
		// [SerializeField]
		// public Button.ButtonClickedEvent Actions;
	}
	
	public class ConstructionWheel : Singleton<ConstructionWheel>
	{
		Transform _transform;
		Selectable _selectable;
		
		[SerializeField] GameObject SlotPrefab;
		[SerializeField] List<ConstructionWheelSlot> SlotsList;

		protected override void Awake()
		{
			base.Awake();

			_transform = GetComponent<Transform>();
			
//			for (int i = 0; i < slotsArray.Length; i++)
//				slotsArray[i].gameObject.SetActive(false);
		}

		void Start()
		{
			Hide();
			Selectable.BuildPlaceClickedEvent += Show;
		}

		public void Show(Selectable selectable, List<ConstructData> constructDataList)
		{
			_selectable = selectable;

			gameObject.SetActive(true);
			Vector3 newPos = (Vector2) selectable.transform.position;
			newPos.z -= 1;
			_transform.position = newPos;
			
			_transform.localScale = Vector3.zero;
			float scaleTime = 0.1f;
			LeanTween.scale(gameObject, Vector3.one, scaleTime);

			for (int i = 0; i < constructDataList.Count; i++)
			{
				ConstructionWheelSlot slot = GameObject.Instantiate(SlotPrefab, transform).GetComponent<ConstructionWheelSlot>();
				slot.Init(constructDataList[i]);
				slot.Button.onClick.AddListener(() => Build(slot));
				SlotsList.Add(slot);
			}

			LeanTween.delayedCall(scaleTime, ActivateSlots);
		}

		void ActivateSlots()
		{
			for (int i = 0; i < SlotsList.Count; i++)
				SlotsList[i].Active = true;
		}
		
		public void Hide()
		{
			for (int i = 0; i < transform.childCount; i++)
				Destroy(transform.GetChild(i).gameObject);
			SlotsList.Clear();
//			for (int i = 0; i < slotsArray.Length; i++)
//				slotsArray[i].gameObject.SetActive(false);
			
			gameObject.SetActive(false);
		}
		
		void Build(ConstructionWheelSlot slot)
		{
			Hide();
			
			GameObject go = Instantiate(slot.BuildingPrefab);
			go.transform.position = _selectable.transform.position;
			go.GetComponent<Building>().Init(_selectable);
			
			Destroy(_selectable.gameObject);
		}
	}
}