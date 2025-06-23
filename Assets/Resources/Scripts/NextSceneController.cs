using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneController : MonoBehaviour
{
    public AudioClip sonidoCerca;
    private AudioSource audioSource;
    public DataRepo dataRepo;
    public Database database;
    public int EnemigosDerrotados;
    public int MonedasRecolectadas;
    private bool isPased = false;

    void Start()
    {
        dataRepo = DataRepo.GetInstance();
        database = dataRepo.GetData();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (database.EnemigosMuertos >= EnemigosDerrotados && database.coins >= MonedasRecolectadas)
        {
            isPased = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPased)
            return;
        if (collision.CompareTag(tagsClass.PLAYER))
        {
            if (audioSource != null && sonidoCerca != null)
                audioSource.PlayOneShot(sonidoCerca);

            StartCoroutine(TeletransportarDespuesDeEspera());
        }
    }

    private IEnumerator TeletransportarDespuesDeEspera()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(3);
    }
}