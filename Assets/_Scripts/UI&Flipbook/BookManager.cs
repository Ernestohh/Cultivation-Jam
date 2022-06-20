using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    [SerializeField] PageSituations[] situationOfPage;
    public List<AudioClip> bookOpenCloseSounds;
    public List<AudioClip> bookPageFlipSounds;
    private PageSituations leftPage;
    private PageSituations rightPage;
    private bool thereIsSomeTurningGoingOn;

    private bool turnToRight;
    private bool turnToLeft;
    private bool bookIsClosing;

    private bool _bookIsClosed;
    public bool BookIsClosed
    {
        get { return _bookIsClosed; }
        set
        {
            var oldValue = _bookIsClosed;
            _bookIsClosed = value;
            if (_bookIsClosed && oldValue != value)
            {
                CameraManager.Instance.camIsAtCharacter = false;
                Interactor.Instance.isInteractingWithBook = false;
                CameraManager.Instance.SetNeededCamPos();
            }
            if (!_bookIsClosed && oldValue != value)
            {
                Interactor.Instance.isInteractingWithBook = true;
                CameraManager.Instance.SetNeededCamPos();
            }
        }
    }


    private List<PageSituations> pagesToBeClosed = new List<PageSituations>();
    private List<PageSituations> closedPages = new List<PageSituations>();
    private void Start()
    {
        BookIsClosed = true;
        SetCurrentLeftAndRightPages();
    }
    void Update()
    {
        if(!BookIsClosed)
            GetInputPageTurn();
        
        TurnPagesBasedOnInputAndInteraction();

        if (!BookIsClosed)
        {
            GetInputPageClose();
            if (pagesToBeClosed.Count > 0)
            {
                bookIsClosing = true;
                CloseChosenPages();
            }
            else
                bookIsClosing = false;
        }
      
    }
 
    public void OpenBook()
    {
        if (BookIsClosed)
        {
            //Debug.Log("unity eventle buraya girdi");
            turnToLeft = true;
        }
    }
    void GetInputPageClose()
    {
        if (Input.GetKeyDown(KeyCode.C) && !thereIsSomeTurningGoingOn)
        {
            //closeTheBook = true; 
            PreparePagesToBeClosed();
        }
    }
    void CloseChosenPages()
    {
        foreach (PageSituations page in pagesToBeClosed)
        {
            //bookIsClosed = false;
           
                
            float prevPageState = page.page_spring.state;
            page.page_spring.Update();
            UpdatePageTransformation(page);
            if (page.page_number ==0)
            {
                if (prevPageState > 0.65f && page.page_spring.state <= 0.65f)
                {
                    PlaySoundFromGroup(bookOpenCloseSounds, 1f);
                }
            }
            if (page.page_spring.state == page.page_spring.target_state)
            {
                closedPages.Add(page);
            }
        }
        if (closedPages.Count == pagesToBeClosed.Count)//all of the pages are closed
        {
            closedPages.Clear();
            pagesToBeClosed.Clear();
            SetCurrentLeftAndRightPages();
            BookIsClosed = true;
            //Interactor.Instance.isInteractingWithBook = false;
        }
    }
    void PreparePagesToBeClosed()
    {
        for (int i = situationOfPage.Length - 1; i >= 0; i--)
        {
            if (situationOfPage[i].page_spring.state == 1f && PageIsNotTurning(situationOfPage[i]))
            {
                bookIsClosing = true;//there is a page that is open 
                situationOfPage[i].page_spring.target_state = 0f;
                pagesToBeClosed.Add(situationOfPage[i]);
            }
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
        var prevState = rightPage.page_spring.state;
        rightPage.page_spring.Update();
        UpdatePageTransformation(rightPage);
        BookIsClosed = false;

        if (prevState < 0.35f && rightPage.page_spring.state >= 0.35f)
        {
            PlaySoundFromGroup(bookPageFlipSounds, 1f);
        }
        //Interactor.Instance.isInteractingWithBook = true;//could find a better solution for this. calling this only once when required could be nice.

        if (rightPage.page_spring.target_state == rightPage.page_spring.state)
        {
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
        var prevState = leftPage.page_spring.state;
        leftPage.page_spring.Update();
        UpdatePageTransformation(leftPage);
        if (prevState > 0.65f && leftPage.page_spring.state <= 0.65f)
        {
            if (leftPage.page_number != 0)
                PlaySoundFromGroup(bookPageFlipSounds, 1f);
            else if (leftPage.page_number == 0)
                PlaySoundFromGroup(bookOpenCloseSounds, 1f);
        }
        if (leftPage.page_spring.target_state == leftPage.page_spring.state)
        {
            rightPage = leftPage;

            if (leftPage.page_number - 1 >= 0)
            {
                leftPage = situationOfPage[rightPage.page_number - 1];
            }
            else
            {
                leftPage = null;
                BookIsClosed = true;
                //Interactor.Instance.isInteractingWithBook = false;
            }

            turnToRight = false;
            thereIsSomeTurningGoingOn = false;
        }
        else
        {
            thereIsSomeTurningGoingOn = true;
        }
    }
    void GetInputPageTurn()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !thereIsSomeTurningGoingOn && leftPage != null && !bookIsClosing)
        {
            turnToRight = true;
            //TurnThePageToRight();
        }
        if (Input.GetKeyDown(KeyCode.E) && !thereIsSomeTurningGoingOn && rightPage != null && !bookIsClosing && rightPage.page_number != situationOfPage.Length - 1)
        {
           
            turnToLeft = true;
       
            //TurnThePageToLeft();
        }
    }
    void TurnPagesBasedOnInputAndInteraction()
    {
        if (turnToRight)
        {
            TurnThePageToRight();
        }
        if (turnToLeft)
        {
            TurnThePageToLeft();
        }
    }

    public void PlaySoundFromGroup(List<AudioClip> group, float volume)
    {
        int which_shot = UnityEngine.Random.Range(0, group.Count);
        GetComponent<AudioSource>().PlayOneShot(group[which_shot], volume);
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
