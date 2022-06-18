using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance = null;
   
    const float kCamMoveSpringStrength = 20f;
    const float kCamMoveSpringDamping = 0.00001f;
    Spring cam_wide_pos_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, false);
    Spring cam_player_pos_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, false);
    Spring cam_book_pos_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, false);
    Transform neededPos;

    [SerializeField] Transform camWidePos;
    [SerializeField] Transform camPlaygroundPos;
    [SerializeField] Transform bookReadPos;
    [SerializeField] Camera game_Camera;
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
        cam_wide_pos_spring.target_state = 1f;
        SetNeededCamPos();
        cam_book_pos_spring.target_state = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputAndChangeCamSpring();
        UpdateCamTransformation();
        UpdateSprings();
    }
    void UpdateSprings()
    {
        cam_wide_pos_spring.Update();
        cam_player_pos_spring.Update();
        cam_book_pos_spring.Update();
    }
    void GetInputAndChangeCamSpring()
    {
        if (Input.GetKeyDown(KeyCode.V) && Camera.main != null)
        {
            if (!Interactor.Instance.isInteractingWithBook)
            {
                if (cam_player_pos_spring.target_state == 1f)
                {
                    cam_player_pos_spring.target_state = 0f;
                    cam_wide_pos_spring.target_state = 1f;
                }
                else if (cam_player_pos_spring.target_state == 0f)
                {
                    cam_player_pos_spring.target_state = 1f;
                    cam_wide_pos_spring.target_state = 0f;
                }
            }
         
        }
    }
    void UpdateCamTransformation()
    {
        /*
        Vector3 wide_pos = camWidePos.transform.position;
        Quaternion wide_dir=camWidePos.transform.rotation;

        game_Camera.transform.position = mix(wide_pos, neededPos.position, cam_move_spring.state);
        game_Camera.transform.rotation = mixRot(wide_dir, neededPos.rotation, cam_move_spring.state);
        */
        //if (cam_wide_pos_spring.target_state == 1f)
            ApplyPose(camWidePos, cam_wide_pos_spring.state);
       
        //if (cam_player_pos_spring.target_state == 1f)
            ApplyPose(camPlaygroundPos, cam_player_pos_spring.state);
        
        //if (cam_book_pos_spring.target_state == 1f)
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
    public void SetNeededCamPos()
    {
        if (Interactor.Instance.isInteractingWithBook)
        {
            //neededPos = bookReadPos;
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
