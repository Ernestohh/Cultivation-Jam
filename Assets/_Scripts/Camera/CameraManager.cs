using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance = null;
   
    const float kCamMoveSpringStrength = 20f;
    const float kCamMoveSpringDamping = 0.00001f;
    Spring cam_wide_pos_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, true);
    Spring cam_player_pos_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, true);
    Spring cam_book_pos_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, true);
    public Vector3 camLastKnownPos;
    public Quaternion camLastKnownRot;
    public bool camIsAtCharacter = true;

    [SerializeField] Transform camWidePos;
    [SerializeField] Transform camPlaygroundPos;
    [SerializeField] Transform bookReadPos;
    [SerializeField] Camera game_Camera;
    [SerializeField] GameObject cinemachine_freeLookGameObject;
    float previousPlayerCamState;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple DetectableTargetManager found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        camLastKnownPos = game_Camera.transform.position;
        camLastKnownRot = game_Camera.transform.rotation;
        cam_wide_pos_spring.target_state = 1f;
        SetNeededCamPos();
        cam_book_pos_spring.target_state = 0f;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && camIsAtCharacter) //everything else didn't work so i had to go for this solution but still we need better solution
        {
            if(!Interactor.Instance.isInteractingWithBook)
                SetLastKnownCamPos();
        }
        GetInputAndChangeCamSpring();
        UpdateCamTransformation();
        UpdateSprings();
        if (previousPlayerCamState < 1f && cam_player_pos_spring.state >= 1f)//it means cam almost arrived the character
        {
            camIsAtCharacter = true;
            GivePlayerControlBack();
        }
    }

    void GivePlayerControlBack()
    {
        cinemachine_freeLookGameObject.SetActive(true);
        TPSMovement.Instance.canControlPlayer = true;
    }

    void UpdateSprings()
    {
        cam_wide_pos_spring.Update();
        previousPlayerCamState = cam_player_pos_spring.state;
        cam_player_pos_spring.Update();
        cam_book_pos_spring.Update();
    }
    void GetInputAndChangeCamSpring()
    {
        if (Input.GetKeyDown(KeyCode.V) && Camera.main != null)
        {
            if (!Interactor.Instance.isInteractingWithBook)
            {
                if (cam_player_pos_spring.target_state == 1f && cam_player_pos_spring.state == 1f)
                {
                    
                    camLastKnownPos = Camera.main.transform.position;
                    camLastKnownRot = Camera.main.transform.rotation;
                    TPSMovement.Instance.canControlPlayer = false;
                    Interactor.Instance.interactionMessageUI.Close();
                    cinemachine_freeLookGameObject.SetActive(false);
                    cam_player_pos_spring.target_state = 0f;
                    cam_wide_pos_spring.target_state = 1f;
                }
                else if (cam_player_pos_spring.target_state == 0f && cam_player_pos_spring.state == 0f)
                {
                    cam_player_pos_spring.target_state = 1f;
                    cam_wide_pos_spring.target_state = 0f;
                }
            }
         
        }
    }
    void UpdateCamTransformation()
    {
        ApplyPose(camWidePos, cam_wide_pos_spring.state);
        ApplyPoseSpecial(camLastKnownPos, camLastKnownRot, cam_player_pos_spring.state);
        ApplyPose(bookReadPos, cam_book_pos_spring.state);
    }
    public void ApplyPose(Transform poseTransform, float amount)
    {
        Transform pose = poseTransform;
        if (amount == 0.0f || (pose == null))
        {
            return;
        }
        game_Camera.transform.position = mix(game_Camera.transform.position, pose.position, amount);
        game_Camera.transform.rotation = mixRot(game_Camera.transform.rotation, pose.rotation, amount);
    }
    public void ApplyPoseSpecial(Vector3 posePosition,Quaternion poseRotation, float amount)
    {
        if (amount == 0.0f)
        {
            return;
        }
        game_Camera.transform.position = mix(game_Camera.transform.position, posePosition, amount);
        game_Camera.transform.rotation = mixRot(game_Camera.transform.rotation, poseRotation, amount);
    }
    public void SetLastKnownCamPos()
    {
        camLastKnownPos = Camera.main.transform.position;
        camLastKnownRot = Camera.main.transform.rotation;
    }
    public void SetNeededCamPos()
    {
        if (Interactor.Instance.isInteractingWithBook)
        {
            //neededPos = bookReadPos;
            Debug.Log("buraya girdiði kesin " + game_Camera.transform.position);
            SetLastKnownCamPos();
            TPSMovement.Instance.canControlPlayer = false;
            cinemachine_freeLookGameObject.SetActive(false);
            cam_player_pos_spring.target_state = 0f;
            cam_wide_pos_spring.target_state = 0f;
            cam_book_pos_spring.target_state = 1f;
        }
        
        if (!Interactor.Instance.isInteractingWithBook)
        {
            //neededPos = camPlaygroundPos;
            
            cam_book_pos_spring.target_state = 0f;
            cam_wide_pos_spring.target_state = 0f;
            cam_player_pos_spring.target_state = 1f;
        }
    }
    public Vector3 mix(Vector3 basePos, Vector3 targetPos, float loadedPercent)
    {
        return basePos + (targetPos - basePos) * loadedPercent;
    }
    public Quaternion mixRot(Quaternion a, Quaternion b, float val)
    {
        float angle = 0.0f;
        Vector3 axis = new Vector3();
        (Quaternion.Inverse(b) * a).ToAngleAxis(out angle, out axis);
        if (angle > 180)
        {
            angle -= 360.0f;
        }
        if (angle < -180)
        {
            angle += 360.0f;
        }
        if (angle == 0)
        {
            return a;
        }
        return a * Quaternion.AngleAxis(angle * -val, axis);
    }
}
