using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class Heatmap
	{
//		HeatField heatfield;
		Dictionary <Vector2, HeatField> persistentMap;
		Dictionary <Vector2, HeatField> temporalMap;

		private int columns;
		private int rows;

		//persistent or temporal
		private String type;

		private bool visible = true;

		public Heatmap (GameObject[,] grid)
		{
			this.columns = grid.GetLength(0);
			this.rows = grid.GetLength(1);

			this.persistentMap = new Dictionary<Vector2, HeatField> ();
			this.temporalMap = new Dictionary<Vector2, HeatField> ();

//			HeatField heatfield = new HeatField (grid [0, 0], 0);
//			this.map.Add (new Vector2 ((float) 0, (float) 0), new HeatField (grid [0, 0], 0));

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					this.persistentMap.Add (new Vector2 ((float)i, (float)j), new HeatField (grid [i, j], 0));
					this.temporalMap.Add (new Vector2 ((float)i, (float)j), new HeatField (grid [i, j], 0));
				}
			}

		}

		public void update(string type, Vector2 temp) {
//			Debug.Log("update");
			//Vector2 temp = new Vector2(item.Path[heatmapStep].x, item.Path[heatmapStep].z);
			if (type == "persistent") {
//				float factor = 0.05f;
//				float x = this.persistentMap [temp].getVisited () * factor;

				if (this.visible) {
					this.persistentMap [temp].getCell ().GetComponent<Renderer> ().material.color = new Color (this.persistentMap [temp].getVisited () * 0.05f, 1 - this.persistentMap [temp].getVisited () * 0.05f, 0, 0.8f);
				} else {
					this.persistentMap [temp].getCell ().GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0.8f);
				}

				this.persistentMap [temp].addUpVisited ();

			} else if (type == "temporal") {
				this.temporalMap [temp].addUpVisited ();
			}
		}

		public void draw() {
			Debug.Log (this.visible);
//			Debug.Log("draw");
			if (this.visible) {
				foreach (HeatField field in this.temporalMap.Values) {
					if (field.getVisited () > 4) {
						field.getCell ().GetComponent<Renderer> ().material.color = new Color (1, 0, 0, 0.8f);
					} else if (field.getVisited () > 2) {
						field.getCell ().GetComponent<Renderer> ().material.color = new Color (1, 1, 0, 0.8f);
					} else if (field.getVisited () > 0) {
						field.getCell ().GetComponent<Renderer> ().material.color = new Color (0, 1, 0, 0.8f);
					} else if (field.getVisited () == 0) {
						field.getCell ().GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0.8f);
					}
				}
			}
		}

		public void reset() {

			foreach (Vector2 tempKey in this.temporalMap.Keys) {
				this.temporalMap [tempKey].getCell ().GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0.8f);
				this.temporalMap [tempKey].resetVisited ();
			}

			foreach (Vector2 persKey in this.persistentMap.Keys) {
				if (this.visible) {
					if (this.persistentMap [persKey].getVisited () == 0) {
						this.persistentMap [persKey].getCell ().GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0.8f);
					} else {
						this.persistentMap [persKey].getCell ().GetComponent<Renderer> ().material.color = new Color (this.persistentMap [persKey].getVisited () * 0.05f, 1 - this.persistentMap [persKey].getVisited () * 0.05f, 0, 0.8f);
					}
				} else {
					this.persistentMap [persKey].getCell ().GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0.8f);
				}
			}
		}

		public void setHeatMapType(){
		}

		public void hide() {
			this.visible = false;
		}
		public void show() {
			this.visible = false;
		}
		public void toggleVisibility() {
			this.visible = !this.visible;
			this.reset ();
		}
			
	}
}

