﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
	public class ReagentResource : MonoBehaviour
	{
		int _reagents;
		TextMeshProUGUI _textMeshPro;

		void Awake()
		{
			_textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
		}
        
		void Start()
		{
			_reagents = 0;//Int32.Parse(Value.text);
			UpdateValue();
		}

		void OnEnable()
		{
			Unit.DiedEvent += ChangeValue;
		}

		void OnDisable()
		{
			Unit.DiedEvent -= ChangeValue;
		}

		void ChangeValue(Unit unit)
		{
//			reagentValue += enemy.FoodReward;
			UpdateValue();
		}

		void UpdateValue()
		{
			_textMeshPro.text = _reagents.ToString();
		}
	}
}