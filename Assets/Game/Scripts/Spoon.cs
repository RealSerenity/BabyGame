using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Spoon : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Transform rotateTransform;
    private bool isThrowing = false;
    public Transform fillSphere;
    private Material mat;
    public Transform feed;
    public LineRenderer line;
    public int numPoints = 50;
    public float timeBetweenPoints = 0.1f;
    public int takenMamas = 0;
    public UIManager uManager;
    private float matGrowFloat;
    private float coroutineStop;
    [SerializeField] public GameObject projectilePredict;
    public GameObject projectileInstance;
    public GameObject mama;

    public bool IsThrowing { get { return isThrowing;} set { isThrowing= value ; }
}
    void Start()
    {
        mat = feed.GetComponent<MeshRenderer>().sharedMaterial;
        mat.SetFloat("Vector1_43ba9ed3ffb9421994c188c640ba95c8", 1);
        uManager = FindObjectOfType<UIManager>();
        projectileInstance = Instantiate(projectilePredict, new Vector3(0,0,0), Quaternion.identity);
        projectileInstance.SetActive(false);
    }

    void Update()
    {
        if (isThrowing)
        {
            float HorizontalRotation = Input.GetAxis("Mouse X");
            float VericalRotation = Input.GetAxis("Mouse Y");

            float x = VericalRotation + rotateTransform.eulerAngles.x;
            float y = HorizontalRotation + rotateTransform.eulerAngles.y;

            x = Mathf.Clamp(x, 275f, 355f);
            y = Mathf.Clamp(y, 85f, 265f);
            transform.rotation = Quaternion.Euler(x, y, 0);
            ShowLine();
        }
    }

    public void SetLineActive(bool active)
    {
        line.enabled = active;
        if(!active) projectileInstance.SetActive(active);
        if(GameManager.Instance.currentState == GameManager.GameState.Finish) uManager.OnFinishState();
    }


    void ShowLine()
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = fillSphere.position;
        Vector3 startingVelocity = fillSphere.up * 25f;
        float maxDuration = 5f;
        int maxSteps = (int) (maxDuration / timeBetweenPoints);
        float upSpeed = startingVelocity.y;
        float flightTime = upSpeed / Physics.gravity.y * 2 ;
        flightTime = Math.Abs(flightTime);
        int count = 0;
        flightTime += 0.2f;
        Vector3 newPoint = new Vector3();
        for (float t = 0; t < maxSteps; t+=timeBetweenPoints)
        {
            count++;
            newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
            points.Add(newPoint);
            if (CheckForCollision(newPoint))
            {
                break;
            }
        }
        line.positionCount = count;
        line.SetPositions(points.ToArray());
        projectileInstance.transform.position = newPoint;
        projectileInstance.SetActive(true);
    }

    private bool CheckForCollision(Vector3 position)
    {

        Collider[] hits = Physics.OverlapSphere(position, 0.3f); //Measure collision via a small circle at the latest position, dont continue simulating Arc if hit
        int count = 0;
        foreach(Collider hit in hits)
        {
            if (hit.gameObject.GetComponent<ProjectPredict>() == null && !hit.gameObject.tag.Equals("FinishLine"))
            {
                count++;
            }
        }
        if (count > 0) //Return true if something is hit, stopping Arc simulation
        {
            return true;
     
        }
        return false;
    }

    public void TakeFeed()
    {
        float current = mat.GetFloat("Vector1_43ba9ed3ffb9421994c188c640ba95c8");
        coroutineStop = current-0.2f;
        takenMamas++;
        uManager.UpdateMamaText();
        StartCoroutine(IncreaseFeed(current));
    }

    IEnumerator IncreaseFeed(float current)
    {
        mat.SetFloat("Vector1_43ba9ed3ffb9421994c188c640ba95c8", current);
        yield return new WaitForSeconds(0.001f);
        StartCoroutine(IncreaseFeed(current - 0.0075f));
        if (current <= coroutineStop)
        {
            StopAllCoroutines();
        }
    }
    
    public void ShowPraiseText(String text)
    {
        uManager.ShowPraiseText(text);
    }


}
