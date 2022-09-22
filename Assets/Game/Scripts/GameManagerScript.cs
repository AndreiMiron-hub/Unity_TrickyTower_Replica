using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] Shapes;
    public GameObject Stump;
    public GameObject HeightBar;
    public GameObject TextPoints;
    public GameObject TextHighScore;
    public GameObject FirstSkill;
    public GameObject SecondSkill;
    public GameObject ThirdSkill;
    public GameObject MainCamera;
    public GameObject InGameUI;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;

    private Vector3 SpawnPosition;
    public int Points = 0;
    public bool SpawnNow = false;
    public bool GamePaused = false;
    public int HighScore;

    public Piece ActivePiece { get; set; }
    public AudioClip SpawnAudio;
    public Material NewMaterial;

    private void Awake()
    {
        ActivePiece = GetComponentInChildren<Piece>();
        SpawnPosition = Stump.transform.position;
        SpawnPosition.y = Stump.transform.position.y + 3;
    }

    private void Start()
    {
        SpawnPiece();
        TextPoints.GetComponent<TextMeshProUGUI>().text = Points.ToString();
        HighScore = PlayerPrefs.GetInt("highscore", HighScore);
        TextHighScore.GetComponent<TextMeshProUGUI>().text = HighScore.ToString();
    }

    void Update()
    {
        #region Points System

        if (Int32.Parse(TextPoints.GetComponent<TextMeshProUGUI>().text) < 5)
        {
            var tempColor = FirstSkill.GetComponent<RawImage>().color;
            tempColor.a = 0.5f;
            FirstSkill.GetComponent<RawImage>().color = tempColor;
        }
        else
        {
            var tempColor = FirstSkill.GetComponent<RawImage>().color;
            tempColor.a = 1f;
            FirstSkill.GetComponent<RawImage>().color = tempColor;
        }

        if (Int32.Parse(TextPoints.GetComponent<TextMeshProUGUI>().text) < 10)
        {
            var tempColor = SecondSkill.GetComponent<RawImage>().color;
            tempColor.a = 0.5f;
            SecondSkill.GetComponent<RawImage>().color = tempColor;
        }
        else
        {
            var tempColor = SecondSkill.GetComponent<RawImage>().color;
            tempColor.a = 1f;
            SecondSkill.GetComponent<RawImage>().color = tempColor;
        }

        if (Int32.Parse(TextPoints.GetComponent<TextMeshProUGUI>().text) < 15)
        {
            var tempColor = ThirdSkill.GetComponent<RawImage>().color;
            tempColor.a = 0.5f;
            ThirdSkill.GetComponent<RawImage>().color = tempColor;
        }
        else
        {
            var tempColor = ThirdSkill.GetComponent<RawImage>().color;
            tempColor.a = 1f;
            ThirdSkill.GetComponent<RawImage>().color = tempColor;
        }
        #endregion

        #region Blocks Controlls + PowerUps

        if (ActivePiece.Block is not null)
        {
            if (ActivePiece.SuperPower.hasCollided)
            {
                SpawnPiece();
            }
        }
        if (SpawnNow is true && ActivePiece.Block is null)
        {
            SpawnPiece();
        }

        if (ActivePiece.Block is not null)
        {

            if (ActivePiece.Block.GetComponent<Rigidbody>().velocity.y != 0)
            {
                #region Blocks Controlls
                if (Input.GetKey(KeyCode.A))
                {
                    if (ActivePiece.Block.transform.rotation.x == 0)
                    {
                        ActivePiece.Move(new Vector3(0, 0, 0.01f));
                    }

                    if (ActivePiece.Block.transform.eulerAngles.x < 91 && 
                        ActivePiece.Block.transform.eulerAngles.x > 89)
                    {
                        ActivePiece.Move(new Vector3(0, 0.01f, 0));
                    }

                    if (ActivePiece.Block.transform.eulerAngles.x == 0 &&
                        ActivePiece.Block.transform.eulerAngles.y == 180 &&
                        ActivePiece.Block.transform.eulerAngles.z == 180)
                    {
                        ActivePiece.Move(new Vector3(0, 0, -0.01f)); 
                    }

                    if (ActivePiece.Block.transform.eulerAngles.x == 270)
                    {
                        ActivePiece.Move(new Vector3(0, -0.01f, 0)); 
                    }
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (ActivePiece.Block.transform.rotation.x == 0)
                    {
                        ActivePiece.Move(new Vector3(0, 0, -0.01f));
                    }

                    if (ActivePiece.Block.transform.eulerAngles.x < 91 &&
                        ActivePiece.Block.transform.eulerAngles.x > 89)
                    {
                        ActivePiece.Move(new Vector3(0, -0.01f, 0));
                    }

                    if (ActivePiece.Block.transform.eulerAngles.x == 0 &&
                        ActivePiece.Block.transform.eulerAngles.y == 180 &&
                        ActivePiece.Block.transform.eulerAngles.z == 180)
                    {
                        ActivePiece.Move(new Vector3(0, 0, 0.01f)); 
                    }

                    if (ActivePiece.Block.transform.eulerAngles.x == 270)
                    {
                        ActivePiece.Move(new Vector3(0, 0.01f, 0)); 
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    ActivePiece.Block.transform.rotation *= Quaternion.Euler(90, 0, 0);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ActivePiece.Block.transform.rotation *= Quaternion.Euler(-90, 0, 0);
                }
                #endregion

                #region Power-ups
                if (Input.GetKeyDown(KeyCode.J)) // Destroy last active object
                {
                    if (Points >= 5)
                    {
                        if (ActivePiece.LastActivePiece != null)
                        {
                            Destroy(ActivePiece.LastActivePiece);
                            Points -= 5;
                            TextPoints.GetComponent<TextMeshProUGUI>().text = Points.ToString();
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.K)) // Place a solid structure
                {
                    if (Points >= 10)
                    {
                        Points -= 10;
                        TextPoints.GetComponent<TextMeshProUGUI>().text = Points.ToString();
                        ActivePiece.Rotate(new Vector3(-90, 0, 0));
                    }
                }

                if (Input.GetKeyDown(KeyCode.L)) // Connect placed blocks
                {
                    if (Points >= 15)
                    {
                        Points -= 15;
                        TextPoints.GetComponent<TextMeshProUGUI>().text = Points.ToString();
                        ActivePiece.ConnectBlocks(ActivePiece.CollisionData);
                    }
                }
                #endregion
            }
        }
        #endregion

        #region Camera Movement
        VerticalCameraMovement();
        #endregion

        #region HighScore
        if (Points > HighScore)
        {
            HighScore = Points;
            TextHighScore.GetComponent<TextMeshProUGUI>().text = HighScore.ToString();
        }

        PlayerPrefs.SetInt("highscore", HighScore);
        PlayerPrefs.Save();
        #endregion

        #region Play/Pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused is false)
            {
                Time.timeScale = 0;
                GamePaused = true;

                InGameUI.SetActive(false);
                PauseMenu.SetActive(true);

            }
            else
            {
                InGameUI.SetActive(true);
                PauseMenu.SetActive(false);
                OptionsMenu.SetActive(false);

                Time.timeScale = 1;
                GamePaused = false;
            }
        }
        #endregion
    }

    private void SpawnPiece()
    {
        if (ActivePiece.Block is not null)
        {
            ActivePiece.LastActivePiece = ActivePiece.Block;
        }
        GameObject shape = Shapes[UnityEngine.Random.Range(0, Shapes.Length)];

        ActivePiece.Block = Instantiate(shape, ActivePiece.Position, Quaternion.identity);

        AudioManager.Instance.PlaySound(SpawnAudio, transform.position);

        ActivePiece.Initialize(SpawnPosition, ActivePiece.Block, this);
        Points += 4;
        TextPoints.GetComponent<TextMeshProUGUI>().text = Points.ToString();
        SpawnNow = false;
    }

    private void VerticalCameraMovement()
    {
        var highestPoint = new Ray(new Vector3(
            HeightBar.transform.position.x,
            HeightBar.transform.position.y,
            HeightBar.transform.position.z
            ),
            HeightBar.transform.forward);

        var lowtPoint = new Ray(new Vector3(
           HeightBar.transform.position.x,
           HeightBar.transform.position.y - 0.3f,
           HeightBar.transform.position.z
           ),
           HeightBar.transform.forward);

        Debug.DrawRay(new Vector3(
            HeightBar.transform.position.x,
            HeightBar.transform.position.y,
            HeightBar.transform.position.z
            ), HeightBar.transform.forward, Color.red);

        Debug.DrawRay(new Vector3(
            HeightBar.transform.position.x,
            HeightBar.transform.position.y - 0.3f,
            HeightBar.transform.position.z
            ), HeightBar.transform.forward, Color.blue);

        RaycastHit hit, hit2;

        if (Physics.Raycast(highestPoint, out hit, 10))
            if (hit.rigidbody is not null)
                if (hit.rigidbody.name != ActivePiece.Block.name)
                {
                    var cameraPosition = MainCamera.transform.position;
                    cameraPosition.y += 0.02f;
                    var heightBarPosition = HeightBar.transform.position;
                    heightBarPosition.y += 0.02f;
                    SpawnPosition.y += 0.2f;
                    MainCamera.transform.position = cameraPosition;
                    HeightBar.transform.position = heightBarPosition;
                }

        if (Physics.Raycast(lowtPoint, out hit2, 10))
            if (hit2.rigidbody is null)
            {
                var cameraPosition = MainCamera.transform.position;
                cameraPosition.y -= 0.02f;
                var heightBarPosition = HeightBar.transform.position;
                heightBarPosition.y -= 0.02f;
                SpawnPosition.y -= 0.2f;
                MainCamera.transform.position = cameraPosition;
                HeightBar.transform.position = heightBarPosition;
            }
    }
}
