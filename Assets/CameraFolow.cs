using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    public static CameraFolow folow;
    public Transform[] pathPoints; // Array de objetos vazios que definem o caminho
    public float speed = 5f; // Velocidade da câmera ao longo do caminho
    public bool loop = false; // Se a câmera deve reiniciar o caminho após o fim
    public float rotationSpeed = 2f; // Velocidade da rotação da câmera

    private int currentPointIndex = 0; // Índice do ponto atual no caminho
    private float progress = 0f; // Progresso da câmera entre pontos
    public bool started;
    private void Awake() {
        if (folow == null) folow = this; else Destroy(this);
    }

    void Start()
    {
        if (pathPoints.Length > 0)
        {
            // Inicializar a posição e rotação da câmera para o primeiro ponto
            transform.position = pathPoints[0].position;
            transform.rotation = pathPoints[0].rotation;
        }
    }

    void Update() {
        if(!started)
            return;
        if (pathPoints.Length > 1) {
            // Obter os pontos atual e próximo no caminho
            Transform currentPoint = pathPoints[currentPointIndex];
            Transform nextPoint = pathPoints[(currentPointIndex + 1) % pathPoints.Length];

            // Atualizar o progresso com base na velocidade e no tempo
            progress += speed * Time.deltaTime;

            // Interpolação linear para suavizar a movimentação
            transform.position = Vector3.Lerp(currentPoint.position, nextPoint.position, progress);

            // Interpolação para suavizar a rotação
            transform.rotation = Quaternion.Lerp(currentPoint.rotation, nextPoint.rotation, progress * rotationSpeed);

            // Verificar se a câmera chegou ao próximo ponto
            if (progress >= 1f) {
                progress = 0f;
                if (currentPointIndex == pathPoints.Length - 1) {
                    Debug.LogWarning("FINALIZADO");
                    Destroy(this.gameObject);
                }else
                    currentPointIndex++;
            }
        }
    }
}
