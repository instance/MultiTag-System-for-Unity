﻿using System.Collections.Generic;
using ToolBox.Attributes;
using UnityEngine;

namespace ToolBox.Groups
{
	[CreateAssetMenu(menuName = "ToolBox/Group")]
	public class Group : ScriptableObject
	{
		public int MembersCount => membersCount;
		public IReadOnlyList<GameObject> Members => listMembers;
		public bool IsEmpty { get; private set; } = false;

		[SerializeField, ReadOnly, BoxGroup("Debug")] private int membersCount = 0;

		private List<GameObject> listMembers = new List<GameObject>();
		private HashSet<GameObject> hashMebmers = new HashSet<GameObject>();

		[Button("Clear group")]
		private void ClearGroup()
		{
			listMembers.Clear();
			hashMebmers.Clear();
			membersCount = 0;
		}

		/// <summary>
		/// Adds a target to the group
		/// </summary>
		public void AddMember(GameObject newMember)
		{
			if (!hashMebmers.Contains(newMember))
			{
				listMembers.Add(newMember);
				hashMebmers.Add(newMember);

				membersCount++;

				IsEmpty = false;
			}
		}

		/// <summary>
		/// Removes a target from the group
		/// </summary>
		public void RemoveMember(GameObject member)
		{
			if (hashMebmers.Contains(member))
			{
				listMembers.Remove(member);
				hashMebmers.Remove(member);

				membersCount--;

				if (membersCount <= 0)
					IsEmpty = true;
			}
		}

		/// <summary>
		/// Checks if the target is in a group
		/// </summary>
		public bool HasMember(GameObject entity) => hashMebmers.Contains(entity);

		/// <summary>
		/// Returns a random member of the group
		/// </summary>
		public GameObject GetRandomMember()
		{
			if (IsEmpty)
				return null;

			int randomIndex = Random.Range(0, membersCount - 1);
			return listMembers[randomIndex];
		}

		/// <summary>
		/// Returns a certain number of group members
		/// </summary>
		public List<GameObject> GetRandomMembers(int count)
		{
			if (IsEmpty)
				return null;

			List<GameObject> randomObjects = new List<GameObject>(count);
			int randomIndex = -1;

			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < 10000; j++)
				{
					int newRandomIndex = Random.Range(0, membersCount - 1);

					if (newRandomIndex != randomIndex)
					{
						randomIndex = newRandomIndex;
						randomObjects[i] = listMembers[randomIndex];
						break;
					}
				}
			}

			return randomObjects;
		}

		/// <summary>
		/// Checks if there is a target in all or one of these groups
		/// </summary>
		public static bool IsEntityInGroups(GameObject target, Group[] groups, CheckType checkType)
		{
			int groupsCount = groups.Length;

			if (checkType == CheckType.AllGroups)
			{
				for (int i = 0; i < groupsCount; i++)
				{
					if (!groups[i].HasMember(target))
						return false;
				}

				return true;
			}
			else
			{
				for (int i = 0; i < groupsCount; i++)
				{
					if (groups[i].HasMember(target))
						return true;
				}

				return false;
			}
		}
	}
}

