using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]

//Klase, kas �auj main�t objekta materi�la poz�ciju render��anas rind� (queue). Izmantots, lai �dens geometrija neb�tu redzama laivas iek�pus�
public class SetRenderQueue : MonoBehaviour {
	
	[SerializeField]
	protected int[] m_queues = new int[]{3000};
	
	protected void Awake() {
		Material[] materials = GetComponent<Renderer>().materials;
		for (int i = 0; i < materials.Length && i < m_queues.Length; ++i) {
			materials[i].renderQueue = m_queues[i];
		}
	}
}