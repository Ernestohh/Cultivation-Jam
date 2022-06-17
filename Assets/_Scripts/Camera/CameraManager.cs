using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    const float kCamMoveSpringStrength = 20.0f;
    const float kCamMoveSpringDamping = 0.00001f;
    Spring cam_move_spring = new Spring(0.0f, 0.0f, kCamMoveSpringStrength, kCamMoveSpringDamping, false);

    [SerializeField] Transform camWidePos;
    [SerializeField] Transform camPlaygroundPos;
    [SerializeField] Camera game_Camera;
    // Start is called before the first frame update
    void Start()
    {
        cam_move_spring.target_state = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamSpring();
        UpdateCamTransformation();
    }
    void UpdateCamSpring()
    {
        if (Input.GetKeyDown(KeyCode.V) && Camera.main != null)
        {
            if (cam_move_spring.target_state == 1f)
            {
                cam_move_spring.target_state = 0f;
            }
            else if (cam_move_spring.target_state == 0f)
            {
                cam_move_spring.target_state = 1f;
            }
        }
        cam_move_spring.Update();
    }
    void UpdateCamTransformation()
    {
        Vector3 wide_pos = camWidePos.transform.position;
        Quaternion wide_dir=camWidePos.transform.rotation;

        game_Camera.transform.position = mix(wide_pos, camPlaygroundPos.position, cam_move_spring.state);
        game_Camera.transform.rotation = mixRot(wide_dir, camPlaygroundPos.rotation, cam_move_spring.state);

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
