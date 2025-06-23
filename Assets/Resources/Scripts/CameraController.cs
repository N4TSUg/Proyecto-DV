using UnityEngine;

public class CameraController : MonoBehaviour
{
  private Transform objetivo;
  public float cameraSpeed = 0.025f;
  public Vector3 desplazamiento;

  void LateUpdate()
  {
    if (objetivo == null)
    {
      objetivo = GameObject.FindGameObjectWithTag(tagsClass.PLAYER).transform;
    }
    if (objetivo == null) return;
    Vector3 posicionDeseada = objetivo.position + desplazamiento;
    Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, cameraSpeed);
    transform.position = posicionSuavizada;
  }
}