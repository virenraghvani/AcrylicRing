using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public int totalNoUpperParticles;
    public int totalNoBottomParticles;

    private int count;

    private Transform upperRingPart, bottomRingPart;

    public GameManager gameManager;
    public SandingMachine sandingMachine;

    [SerializeField]
    private ParticleSystem debrif;

    public float glossiness;

    private void Start()
    {
        upperRingPart = gameManager.currentRing.GetComponent<RingData>().upperRingPart;
        bottomRingPart = gameManager.currentRing.GetComponent<RingData>().bottomRingPart;

        totalNoUpperParticles = upperRingPart.childCount - 1;
        totalNoBottomParticles = bottomRingPart.childCount - 1;

        gameManager.isSandingUpperPart = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isSandingUpperPart)
        {
            if (other.CompareTag("particles_upper"))
            {
                other.transform.parent = null;
                other.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                other.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<Rigidbody>().isKinematic = false;
                other.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0, 5), Random.Range(5, 25), Random.Range(0, 5));
                Destroy(other.gameObject, 3);

                count++;
            }

            glossiness = .38f * count / totalNoUpperParticles;
            gameManager.ringFinalOutput.material.SetFloat("_Glossiness", glossiness);

            debrif.Play();

            if (count >= totalNoUpperParticles - 15)
            {
                count = 0;

                GameManager.IS_READY_FOR_INPUT = false;
                gameManager.SandingUpperDone();
                sandingMachine.SandingDone();

                Debug.Log("hhhh Upper part is done");
                gameManager.isSandingUpperPart = false;

                upperRingPart.gameObject.SetActive(false);

            }
        }else
        {

            if (other.CompareTag("particles_bottom"))
            {
                other.transform.parent = null;
                other.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                other.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<Rigidbody>().isKinematic = false;
                other.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0, 5), Random.Range(5, 25), Random.Range(0, 5));
                Destroy(other.gameObject, 3);

                count++;
            }

            glossiness = .38f * count / totalNoBottomParticles;
            gameManager.ringFinalOutput.material.SetFloat("_Glossiness", glossiness + .38f);

            debrif.Play();

            if (count >= totalNoBottomParticles - 15)
            {
                GameManager.IS_READY_FOR_INPUT = false;
                sandingMachine.SandingDone();
                gameManager.SandingBottomDone();

                bottomRingPart.gameObject.SetActive(false);

                Debug.Log("hhhh Bottom part is done");

            }
        }
        
    }
}
