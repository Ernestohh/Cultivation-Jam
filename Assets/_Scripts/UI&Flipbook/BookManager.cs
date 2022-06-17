using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    [SerializeField] PageSituations[] situationOfPage;
    private PageSituations leftPage;
    private PageSituations rightPage;
    private bool thereIsSomeTurningGoingOn;

    private bool turnToRight;
    private bool turnToLeft;
    private void Start()
    {
        SetCurrentLeftAndRightPages();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !thereIsSomeTurningGoingOn && leftPage!=null)
        {
            turnToRight = true;
            //TurnThePageToRight();
        }
        if (Input.GetKeyDown(KeyCode.E) && !thereIsSomeTurningGoingOn && rightPage!=null)
        {
            turnToLeft = true;
            //TurnThePageToLeft();
        }
        if (turnToRight)
        {
            TurnThePageToRight();
        }
        if (turnToLeft)
        {
            TurnThePageToLeft();
        }
    }
    void UpdatePageTransformation(PageSituations page)
    {
        page.transform.rotation = mixRot(page.rightPos.rotation, page.leftPos.rotation, page.page_spring.state);
    }
    void SetCurrentLeftAndRightPages()
    {
        for (int i = 0; i < situationOfPage.Length; i++)
        {
            if (situationOfPage[i].page_spring.state == 0f && PageIsNotTurning(situationOfPage[i]))//eðer sýfýrsa sayfa saðdadýr. ve ilk eleman saðdaysa. yani kitabýn yüzü. o zaman kitap kapalýdýr
            {
                rightPage = situationOfPage[i];//saðdaki sayfa budur. 
                if (i != 0)
                {
                    leftPage = situationOfPage[i - 1];
                }
                else
                    leftPage = null;
                break;
            }
            else
            {
                rightPage = null;
                leftPage = situationOfPage[situationOfPage.Length - 1];
            }
        }
    }
    bool PageIsNotTurning(PageSituations page)
    {
        if (page.page_spring.state == 0f && page.page_spring.target_state == 0f)
            return true;
        if (page.page_spring.state == 1f && page.page_spring.target_state == 1f)
            return true;
        return false;
    }
    void TurnThePageToLeft()
    {
        rightPage.page_spring.target_state = 1f;

        rightPage.page_spring.Update();
        UpdatePageTransformation(rightPage);

        if (rightPage.page_spring.target_state == rightPage.page_spring.state)
        {
            Debug.Log(situationOfPage.Length + "uzunlyk");
            leftPage = rightPage;
            if (rightPage.page_number + 1 < situationOfPage.Length)
            {
                rightPage = situationOfPage[rightPage.page_number + 1];
            }
            else
                rightPage = null;

            turnToLeft = false;
            thereIsSomeTurningGoingOn = false;
        }
        else
        {
            thereIsSomeTurningGoingOn = true;
        }
    }
    void TurnThePageToRight()
    {
        leftPage.page_spring.target_state = 0f;

        leftPage.page_spring.Update();
        UpdatePageTransformation(leftPage);

        if (leftPage.page_spring.target_state == leftPage.page_spring.state)
        {
            rightPage = leftPage;

            if (leftPage.page_number - 1 >= 0)
            {
                leftPage = situationOfPage[rightPage.page_number - 1];
            }
            else
                leftPage = null;

            turnToRight = false;
            thereIsSomeTurningGoingOn = false;
        }
        else
        {
            thereIsSomeTurningGoingOn = true;
        }
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
