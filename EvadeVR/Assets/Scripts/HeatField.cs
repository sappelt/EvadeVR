using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class HeatField
	{
		private GameObject cell;
		private int visited;

		public HeatField (GameObject cell, int visited)
		{
			this.cell = cell;
			this.visited = visited;
		}

		public GameObject getCell(){
			return this.cell;
		}

		public int getVisited(){
			return this.visited;
		}
		public void setVisited(int count){
			this.visited = count;
		}
		public void addUpVisited() {
			this.visited++;
		}
		public void resetVisited(){
			this.visited = 0;
		}
	}
}

