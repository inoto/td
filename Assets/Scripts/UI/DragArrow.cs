﻿using UnityEngine;
using UnityEngine.UI.Extensions;

namespace TowerDefense
{
	[RequireComponent(typeof(UILineRenderer))]
	public class DragArrow : MonoBehaviour
	{
		[SerializeField] LayerMask clickableLayers;

		UILineRenderer lineRenderer;

		GameObject owner;

		void Awake()
		{
			lineRenderer = GetComponent<UILineRenderer>();
		}

		void Start()
		{
			
		}

		public void Init(GameObject owner)
		{
			this.owner = owner;
			gameObject.SetActive(false);
		}

		public void Start(Vector2 point)
		{
			var building = owner.GetComponent<Building>();
			if (building != null)
			{
				if (building.SoldiersCount <= 0)
					return;
			}

			lineRenderer.Points[1] = lineRenderer.Points[0] =
				Camera.main.WorldToScreenPoint(owner.transform.position);
			lineRenderer.SetVerticesDirty();
			gameObject.SetActive(true);
		}

		public void UpdatePosition(Vector2 point, GameObject target)
		{
			lineRenderer.Points[1] = point;
			lineRenderer.SetVerticesDirty();

			if (target != null && target.GetInstanceID() != owner.GetInstanceID())
			{
				lineRenderer.color = Color.green;
			}
			else
			{
				lineRenderer.color = Color.white;
			}
		}

		public void End(GameObject target)
		{
			Destroy(gameObject);
			if (target != null && target.GetInstanceID() != owner.GetInstanceID())
			{
				var building = target.GetComponent<Building>();
				if (building != null)
				{
					if (building.SoldiersCount >= building.MaxSoldiersCount)
						return;

					owner.GetComponent<Building>().RemoveSoldier().AssignToBuilding(building);
				}

				var wizard = target.GetComponent<Wizard>();
				if (wizard != null)
				{
					owner.GetComponent<Building>().RemoveSoldier().AttackWizard(wizard);
				}
			}
		}
	}
}