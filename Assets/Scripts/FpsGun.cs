using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class FpsGun : MonoBehaviour {

    [SerializeField]
    private int damagePerShot = 20;
    [SerializeField]
    private float timeBetweenBullets = 0.2f;
    [SerializeField]
    private float weaponRange = 100.0f;
    [SerializeField]
    private TpsGun tpsGun;
    [SerializeField]
    private ParticleSystem gunParticles;
    [SerializeField]
    private LineRenderer gunLine;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Camera raycastCamera;

    private float timer;

    /// <summary>
    /// Initalize the timer to 0 in the beginning
    /// </summary>
    void Start() {
        timer = 0.0f;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Can Added functionality to support multiple fire rate
    /// </summary>
    void Update() {
        timer += Time.deltaTime;
        bool shooting = CrossPlatformInputManager.GetButton("Fire1");
        // To implement different fire rates
        if (shooting && timer >= timeBetweenBullets && Time.timeScale != 0) {
            Shoot();
        }
        animator.SetBool("Firing", shooting);
    }

    /// <summary>
    /// Used to shoot paint balls also calls the RPC fire
    /// To do Added fucntionality to customize the paint splash color and rotate paint ball image as per the player's perspective
    /// <summary>
    void Shoot() {
        timer = 0.0f;

        RaycastHit shootHit;
        Ray shootRay = raycastCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0f));
        if (Physics.Raycast(shootRay, out shootHit, weaponRange, LayerMask.GetMask("Shootable"))) {
            string hitTag = shootHit.transform.gameObject.tag;
            switch (hitTag) {
                case "Player":
                    shootHit.collider.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damagePerShot, PhotonNetwork.LocalPlayer.NickName);
                    PhotonNetwork.Instantiate("impactFlesh", shootHit.point, Quaternion.Euler(shootHit.normal.x - 90, shootHit.normal.y, shootHit.normal.z), 0);
                    break;
                default:
                    PhotonNetwork.Instantiate("impactFlesh", shootHit.point, Quaternion.Euler(shootHit.normal.x - 90, shootHit.normal.y, shootHit.normal.z), 0);
                    break;
            }
        }
        tpsGun.RPCShoot();  // RPC for third person view
    }


}
